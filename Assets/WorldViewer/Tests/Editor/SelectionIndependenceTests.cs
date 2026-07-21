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
            var simulation = new WorldSimulation(170601UL);
            WorldSnapshot snapshot = simulation.CreateSnapshot();
            ulong before = simulation.ComputeChecksum();
            var selection = new WorldSelectionState();

            selection.Select(snapshot[0].Id);
            Assert.That(selection.SelectedPersonId, Is.EqualTo(snapshot[0].Id));
            Assert.That(simulation.ComputeChecksum(), Is.EqualTo(before));

            selection.Select(snapshot[11].Id);
            Assert.That(selection.SelectedPersonId, Is.EqualTo(snapshot[11].Id));
            Assert.That(simulation.ComputeChecksum(), Is.EqualTo(before));

            selection.Clear();
            Assert.That(selection.SelectedPersonId, Is.Null);
            Assert.That(simulation.ComputeChecksum(), Is.EqualTo(before));
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
