using System;
using CivSandbox.Simulation;
using NUnit.Framework;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class FixedStepPumpTests
    {
        [Test]
        public void FixedStepWorkAndRetainedBacklogStayBoundedAndDroppedTimeIsReported()
        {
            var pump = new FixedStepPump(20, 4, 6);

            FixedStepPumpReport report = pump.Pump(TimeSpan.FromSeconds(10));

            Assert.That(report.StepsToRun, Is.EqualTo(4));
            Assert.That(report.PendingWholeSteps, Is.EqualTo(6));
            Assert.That(report.RetainedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(300)));
            Assert.That(report.DroppedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(9500)));
            Assert.That(report.TotalDroppedWallTime, Is.EqualTo(report.DroppedWallTime));
            Assert.That(report.IsOverloaded, Is.True);

            FixedStepPumpReport firstDrain = pump.Pump(TimeSpan.Zero);
            FixedStepPumpReport secondDrain = pump.Pump(TimeSpan.Zero);

            Assert.That(firstDrain.StepsToRun, Is.EqualTo(4));
            Assert.That(firstDrain.PendingWholeSteps, Is.EqualTo(2));
            Assert.That(secondDrain.StepsToRun, Is.EqualTo(2));
            Assert.That(secondDrain.PendingWholeSteps, Is.Zero);
            Assert.That(secondDrain.IsOverloaded, Is.False);
        }

        [Test]
        public void EqualElapsedTimeProducesEqualStepCountAfterBacklogDrains()
        {
            var singleFrame = new FixedStepPump(20, 8, 40);
            var partitionedFrames = new FixedStepPump(20, 8, 40);

            int singleFrameSteps = singleFrame.Pump(TimeSpan.FromMilliseconds(500)).StepsToRun;
            singleFrameSteps += singleFrame.Pump(TimeSpan.Zero).StepsToRun;

            int partitionedSteps = 0;
            for (int frame = 0; frame < 10; frame++)
            {
                partitionedSteps += partitionedFrames.Pump(TimeSpan.FromMilliseconds(50)).StepsToRun;
            }

            Assert.That(singleFrameSteps, Is.EqualTo(10));
            Assert.That(partitionedSteps, Is.EqualTo(singleFrameSteps));
            Assert.That(singleFrame.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(partitionedFrames.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void FractionalStepTimeIsRetainedUntilACompleteStepIsDue()
        {
            var pump = new FixedStepPump(20, 8, 40);

            FixedStepPumpReport first = pump.Pump(TimeSpan.FromMilliseconds(25));
            FixedStepPumpReport second = pump.Pump(TimeSpan.FromMilliseconds(25));

            Assert.That(first.StepsToRun, Is.Zero);
            Assert.That(first.RetainedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(25)));
            Assert.That(second.StepsToRun, Is.EqualTo(1));
            Assert.That(second.RetainedWallTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void DiscardPendingWholeStepsPreservesFractionalTimeAndAccountsForLoss()
        {
            var pump = new FixedStepPump(20, 1, 4);
            pump.Pump(TimeSpan.FromMilliseconds(125));

            FixedStepPumpReport report = pump.DiscardPendingWholeSteps();

            Assert.That(report.StepsToRun, Is.Zero);
            Assert.That(report.PendingWholeSteps, Is.Zero);
            Assert.That(report.DroppedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(50)));
            Assert.That(report.TotalDroppedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(50)));
            Assert.That(report.IsOverloaded, Is.True);
            Assert.That(report.RetainedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(25)));
            Assert.That(pump.RetainedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(25)));
        }

        [Test]
        public void SpeedChangeDiscardsOldBacklogBeforeNewSpeedAdvances()
        {
            var changedSpeed = new WorldViewerSession(170601UL, 1, 4);
            var explicitSchedule = new WorldViewerSession(170601UL, 1, 4);

            changedSpeed.AdvanceWallTime(TimeSpan.FromMilliseconds(100));
            changedSpeed.SetSpeed(SimulationSpeed.VeryFast);

            Assert.That(changedSpeed.IsClockOverloaded, Is.True);
            Assert.That(changedSpeed.TotalDroppedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(50)));

            changedSpeed.AdvanceWallTime(TimeSpan.FromMilliseconds(50));
            changedSpeed.AdvanceWallTime(TimeSpan.Zero);

            explicitSchedule.AdvanceWallTime(TimeSpan.FromMilliseconds(50));
            explicitSchedule.SetSpeed(SimulationSpeed.VeryFast);
            explicitSchedule.AdvanceWallTime(TimeSpan.FromMilliseconds(50));

            Assert.That(changedSpeed.Snapshot.Time, Is.EqualTo(new WorldTime(33)));
            Assert.That(changedSpeed.Snapshot.Time, Is.EqualTo(explicitSchedule.Snapshot.Time));
            Assert.That(
                changedSpeed.ComputeAuthoritativeChecksum(),
                Is.EqualTo(explicitSchedule.ComputeAuthoritativeChecksum()));
        }

        [Test]
        public void SelectingCurrentSpeedPreservesFractionalWallTime()
        {
            var session = new WorldViewerSession(170601UL);
            session.AdvanceWallTime(TimeSpan.FromMilliseconds(25));

            session.SetSpeed(SimulationSpeed.Normal);
            session.AdvanceWallTime(TimeSpan.FromMilliseconds(25));

            Assert.That(session.Snapshot.Time, Is.EqualTo(new WorldTime(3)));
            Assert.That(session.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void RepeatedSubTickSpeedChangesStillAccumulateCompleteTicks()
        {
            var session = new WorldViewerSession(170601UL);

            for (int frame = 0; frame < 10; frame++)
            {
                session.SetSpeed((frame & 1) == 0 ? SimulationSpeed.Double : SimulationSpeed.Normal);
                session.AdvanceWallTime(TimeSpan.FromMilliseconds(10));
            }

            Assert.That(session.Snapshot.Time, Is.EqualTo(new WorldTime(9)));
            Assert.That(session.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void ResetClearsRetainedAndDroppedWallTime()
        {
            var pump = new FixedStepPump(20, 4, 6);
            pump.Pump(TimeSpan.FromSeconds(10));

            pump.Reset();
            FixedStepPumpReport report = pump.Pump(TimeSpan.Zero);

            Assert.That(report.StepsToRun, Is.Zero);
            Assert.That(report.RetainedWallTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(report.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(report.IsOverloaded, Is.False);
        }

        [Test]
        public void SessionSurfacesOverloadAndResetClearsClockTelemetry()
        {
            var session = new WorldViewerSession(170601UL, 4, 6);

            FixedStepPumpReport report = session.AdvanceWallTime(TimeSpan.FromSeconds(10));

            Assert.That(report.StepsToRun, Is.EqualTo(4));
            Assert.That(session.Snapshot.Time, Is.EqualTo(new WorldTime(12)));
            Assert.That(session.IsClockOverloaded, Is.True);
            Assert.That(session.TotalDroppedWallTime, Is.EqualTo(TimeSpan.FromMilliseconds(9500)));

            session.Reset(170601UL);

            Assert.That(session.Snapshot.Time, Is.EqualTo(new WorldTime(0)));
            Assert.That(session.IsClockOverloaded, Is.False);
            Assert.That(session.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void NegativeElapsedTimeIsRejectedWithoutChangingState()
        {
            var pump = new FixedStepPump(20, 8, 40);

            Assert.Throws<ArgumentOutOfRangeException>(() => pump.Pump(TimeSpan.FromTicks(-1)));
            Assert.That(pump.RetainedWallTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(pump.TotalDroppedWallTime, Is.EqualTo(TimeSpan.Zero));
        }
    }
}
