using CivSandbox.Simulation;

namespace CivSandbox.Presentation
{
    public sealed class WorldSelectionState
    {
        public StableEntityId? SelectedPersonId { get; private set; }

        public void Select(StableEntityId personId)
        {
            SelectedPersonId = personId;
        }

        public void Clear()
        {
            SelectedPersonId = null;
        }
    }
}
