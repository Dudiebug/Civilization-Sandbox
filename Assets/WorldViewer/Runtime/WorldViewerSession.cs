using System;
using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.Simulation;
using CivSandbox.UI;
using CivSandbox.World;

namespace CivSandbox.WorldViewer
{
    /// <summary>
    /// Owns Build 01 simulation state without depending on Unity frame, camera, or visibility state.
    /// </summary>
    public sealed class WorldViewerSession : IWorldViewerSession
    {
        private readonly WorldSimulation simulation;
        private readonly WorldSelectionState selection = new WorldSelectionState();
        private readonly FixedStepPump fixedStepPump;

        public WorldViewerSession(
            ulong seed,
            int maximumFixedStepsPerPump = 8,
            int maximumRetainedBacklogSteps = 40)
        {
            simulation = new WorldSimulation(seed);
            fixedStepPump = new FixedStepPump(
                SimulationClock.FixedWallTicksPerSecond,
                maximumFixedStepsPerPump,
                maximumRetainedBacklogSteps);
            Snapshot = simulation.CreateSnapshot();
        }

        public WorldViewerSession(
            GeneratedWorld world,
            GeneratedWorldCell foundingCell,
            ulong seed,
            int maximumFixedStepsPerPump = 8,
            int maximumRetainedBacklogSteps = 40)
        {
            simulation = new WorldSimulation(world, foundingCell, seed);
            fixedStepPump = new FixedStepPump(
                SimulationClock.FixedWallTicksPerSecond,
                maximumFixedStepsPerPump,
                maximumRetainedBacklogSteps);
            Snapshot = simulation.CreateSnapshot();
        }

        public ulong Seed => simulation.Seed.Value;

        public SimulationSpeed Speed { get; private set; } = SimulationSpeed.Normal;

        public WorldSnapshot Snapshot { get; private set; }

        public StableEntityId? SelectedPersonId => selection.SelectedPersonId;

        public bool IsCampSelected { get; private set; }

        public bool IsClockOverloaded => LastPumpReport.IsOverloaded;

        public TimeSpan TotalDroppedWallTime => LastPumpReport.TotalDroppedWallTime;

        public FixedStepPumpReport LastPumpReport { get; private set; }

        public FixedStepPumpReport AdvanceWallTime(TimeSpan elapsedWallTime)
        {
            FixedStepPumpReport report = fixedStepPump.Pump(elapsedWallTime);
            LastPumpReport = report;
            for (int tick = 0; tick < report.StepsToRun; tick++)
            {
                simulation.AdvanceFixedWallTick(Speed);
            }

            if (report.StepsToRun > 0 && Speed != SimulationSpeed.Paused)
            {
                Snapshot = simulation.CreateSnapshot();
            }

            return report;
        }

        public void Reset(ulong seed)
        {
            simulation.Reset(seed);
            fixedStepPump.Reset();
            LastPumpReport = default;
            selection.Clear();
            IsCampSelected = false;
            Speed = SimulationSpeed.Normal;
            Snapshot = simulation.CreateSnapshot();
        }

        public void SetSpeed(SimulationSpeed speed)
        {
            if (speed == Speed)
            {
                return;
            }

            LastPumpReport = fixedStepPump.DiscardPendingWholeSteps();
            Speed = speed;
        }

        public void SelectPerson(StableEntityId? personId)
        {
            IsCampSelected = false;
            if (personId.HasValue)
            {
                selection.Select(personId.Value);
            }
            else
            {
                selection.Clear();
            }
        }

        public void SelectCamp()
        {
            selection.Clear();
            IsCampSelected = true;
        }

        public ulong ComputeAuthoritativeChecksum()
        {
            return simulation.ComputeChecksum();
        }
    }
}
