using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.Simulation;
using NUnit.Framework;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class SelectionIndependenceTests
    {
        [Test]
        public void SelectSwitchAndClearDoNotMutateSimulation()
        {
            var session = new WorldViewerSession(170601UL);
            WorldSnapshot snapshot = session.Snapshot;
            ulong before = session.ComputeAuthoritativeChecksum();

            session.SelectPerson(snapshot[0].Id);
            Assert.That(session.SelectedPersonId, Is.EqualTo(snapshot[0].Id));
            Assert.That(session.ComputeAuthoritativeChecksum(), Is.EqualTo(before));

            session.SelectPerson(snapshot[11].Id);
            Assert.That(session.SelectedPersonId, Is.EqualTo(snapshot[11].Id));
            Assert.That(session.ComputeAuthoritativeChecksum(), Is.EqualTo(before));

            session.SelectPerson(null);
            Assert.That(session.SelectedPersonId, Is.Null);
            Assert.That(session.ComputeAuthoritativeChecksum(), Is.EqualTo(before));
        }

        [Test]
        public void SelectionStateHasNoAuthoritativeSimulationField()
        {
            var fields = typeof(WorldSelectionState).GetFields(
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                Assert.That(typeof(WorldSimulation).IsAssignableFrom(field.FieldType), Is.False, field.Name);
            }
        }
    }
}
