using System;

namespace CivSandbox.Simulation
{
    public readonly struct FixedStepPumpReport
    {
        public FixedStepPumpReport(
            int stepsToRun,
            int pendingWholeSteps,
            TimeSpan retainedWallTime,
            TimeSpan droppedWallTime,
            TimeSpan totalDroppedWallTime)
        {
            StepsToRun = stepsToRun;
            PendingWholeSteps = pendingWholeSteps;
            RetainedWallTime = retainedWallTime;
            DroppedWallTime = droppedWallTime;
            TotalDroppedWallTime = totalDroppedWallTime;
        }

        public int StepsToRun { get; }

        public int PendingWholeSteps { get; }

        public TimeSpan RetainedWallTime { get; }

        public TimeSpan DroppedWallTime { get; }

        public TimeSpan TotalDroppedWallTime { get; }

        public bool IsOverloaded => PendingWholeSteps > 0 || DroppedWallTime > TimeSpan.Zero;
    }

    /// <summary>
    /// Converts elapsed wall time into a bounded number of fixed simulation steps.
    /// This scheduler is non-authoritative: callers decide what one returned step does.
    /// </summary>
    public sealed class FixedStepPump
    {
        private readonly long fixedStepWallTicks;
        private readonly long maximumRetainedWallTicks;
        private readonly long maximumAdmissionWallTicks;
        private long retainedWallTicks;
        private long totalDroppedWallTicks;

        public FixedStepPump(int stepsPerSecond, int maximumStepsPerPump, int maximumRetainedSteps)
        {
            if (stepsPerSecond <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stepsPerSecond));
            }

            if (TimeSpan.TicksPerSecond % stepsPerSecond != 0)
            {
                throw new ArgumentException(
                    "The step rate must divide TimeSpan.TicksPerSecond exactly.",
                    nameof(stepsPerSecond));
            }

            if (maximumStepsPerPump <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumStepsPerPump));
            }

            if (maximumRetainedSteps < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumRetainedSteps));
            }

            if ((long)maximumStepsPerPump + maximumRetainedSteps > int.MaxValue)
            {
                throw new ArgumentException("The combined process and backlog step limit is too large.");
            }

            fixedStepWallTicks = TimeSpan.TicksPerSecond / stepsPerSecond;
            maximumRetainedWallTicks = checked(fixedStepWallTicks * maximumRetainedSteps);
            maximumAdmissionWallTicks = checked(
                fixedStepWallTicks * ((long)maximumStepsPerPump + maximumRetainedSteps));
            MaximumStepsPerPump = maximumStepsPerPump;
            MaximumRetainedSteps = maximumRetainedSteps;
        }

        public TimeSpan FixedStepDuration => TimeSpan.FromTicks(fixedStepWallTicks);

        public int MaximumStepsPerPump { get; }

        public int MaximumRetainedSteps { get; }

        public TimeSpan RetainedWallTime => TimeSpan.FromTicks(retainedWallTicks);

        public TimeSpan TotalDroppedWallTime => TimeSpan.FromTicks(totalDroppedWallTicks);

        public FixedStepPumpReport Pump(TimeSpan elapsedWallTime)
        {
            if (elapsedWallTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(elapsedWallTime));
            }

            long incomingWallTicks = elapsedWallTime.Ticks;
            long admissionRoom = maximumAdmissionWallTicks - retainedWallTicks;
            long admittedWallTicks = Math.Min(incomingWallTicks, admissionRoom);
            long droppedWallTicks = incomingWallTicks - admittedWallTicks;
            long availableWallTicks = retainedWallTicks + admittedWallTicks;
            int dueSteps = (int)(availableWallTicks / fixedStepWallTicks);
            int stepsToRun = Math.Min(dueSteps, MaximumStepsPerPump);

            retainedWallTicks = availableWallTicks - stepsToRun * fixedStepWallTicks;
            totalDroppedWallTicks = SaturatingAdd(totalDroppedWallTicks, droppedWallTicks);

            return new FixedStepPumpReport(
                stepsToRun,
                (int)(retainedWallTicks / fixedStepWallTicks),
                TimeSpan.FromTicks(retainedWallTicks),
                TimeSpan.FromTicks(droppedWallTicks),
                TimeSpan.FromTicks(totalDroppedWallTicks));
        }

        public void Reset()
        {
            retainedWallTicks = 0;
            totalDroppedWallTicks = 0;
        }

        public FixedStepPumpReport DiscardPendingWholeSteps()
        {
            long droppedWallTicks = retainedWallTicks / fixedStepWallTicks * fixedStepWallTicks;
            retainedWallTicks -= droppedWallTicks;
            totalDroppedWallTicks = SaturatingAdd(totalDroppedWallTicks, droppedWallTicks);

            return new FixedStepPumpReport(
                0,
                0,
                TimeSpan.FromTicks(retainedWallTicks),
                TimeSpan.FromTicks(droppedWallTicks),
                TimeSpan.FromTicks(totalDroppedWallTicks));
        }

        private static long SaturatingAdd(long left, long right)
        {
            return right > long.MaxValue - left ? long.MaxValue : left + right;
        }
    }
}
