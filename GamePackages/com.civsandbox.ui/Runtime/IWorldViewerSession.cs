using System;
using CivSandbox.People;
using CivSandbox.Simulation;

namespace CivSandbox.UI
{
    public interface IWorldViewerSession
    {
        ulong Seed { get; }
        SimulationSpeed Speed { get; }
        WorldSnapshot Snapshot { get; }
        StableEntityId? SelectedPersonId { get; }
        bool IsClockOverloaded { get; }
        TimeSpan TotalDroppedWallTime { get; }
        void Reset(ulong seed);
        void SetSpeed(SimulationSpeed speed);
        void SelectPerson(StableEntityId? personId);
    }
}
