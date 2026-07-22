using System;
using CivSandbox.Simulation;

namespace CivSandbox.World
{
    public sealed class GeneratedWorld
    {
        private const ulong FoundingLocationStream = 0x73746172742d6c6fUL;
        public const int FoundingClearanceCells = 22;
        private readonly GeneratedWorldCell[] cells;
        private readonly int[] waterPrefix;

        internal GeneratedWorld(WorldGenerationSettings settings, ulong worldId, int columns, int rows, GeneratedWorldCell[] generatedCells)
        {
            Settings = settings;
            WorldId = worldId;
            Columns = columns;
            Rows = rows;
            cells = generatedCells ?? throw new ArgumentNullException(nameof(generatedCells));
            if (cells.Length != columns * rows)
            {
                throw new ArgumentException("Generated cell count does not match world dimensions.", nameof(generatedCells));
            }

            waterPrefix = BuildWaterPrefix();
            Checksum = ComputeChecksum();
        }

        public WorldGenerationSettings Settings { get; }

        public ulong WorldId { get; }

        public int Columns { get; }

        public int Rows { get; }

        public int CellCount => cells.Length;

        public ulong Checksum { get; }

        public GeneratedWorldCell this[int index] => cells[index];

        public GeneratedWorldCell CellAt(int column, int row)
        {
            if (column < 0 || column >= Columns) throw new ArgumentOutOfRangeException(nameof(column));
            if (row < 0 || row >= Rows) throw new ArgumentOutOfRangeException(nameof(row));
            return cells[row * Columns + column];
        }

        public bool TryGetCell(StableEntityId id, out GeneratedWorldCell cell)
        {
            if (id.World != WorldId || id.Local == 0 || id.Local > (ulong)cells.Length)
            {
                cell = default;
                return false;
            }

            cell = cells[(int)id.Local - 1];
            return true;
        }

        public bool TryFindFirstFoundableCell(out GeneratedWorldCell cell)
        {
            for (int index = 0; index < cells.Length; index++)
            {
                if (cells[index].IsFoundable)
                {
                    cell = cells[index];
                    return true;
                }
            }

            cell = default;
            return false;
        }

        public bool TryChooseFoundingCell(out GeneratedWorldCell cell)
        {
            int interiorMargin = Math.Min(
                FoundingClearanceCells,
                Math.Min((Columns - 1) / 3, (Rows - 1) / 3));
            int foundableCount = CountFoundingCandidates(interiorMargin);
            if (foundableCount == 0)
            {
                cell = default;
                return false;
            }

            int chosen = (int)(KeyedRandom.Sample(
                Settings.Seed.Value,
                FoundingLocationStream,
                WorldId,
                (ulong)foundableCount,
                0) % (ulong)foundableCount);
            for (int index = 0; index < cells.Length; index++)
            {
                if (!IsFoundingCandidate(cells[index], interiorMargin))
                {
                    continue;
                }

                if (chosen-- == 0)
                {
                    cell = cells[index];
                    return true;
                }
            }

            cell = default;
            return false;
        }

        private int CountFoundingCandidates(int margin)
        {
            int count = 0;
            for (int index = 0; index < cells.Length; index++)
            {
                if (IsFoundingCandidate(cells[index], margin))
                {
                    count++;
                }
            }

            return count;
        }

        private bool IsFoundingCandidate(GeneratedWorldCell cell, int margin)
        {
            return cell.IsFoundable &&
                   cell.Column >= margin &&
                   cell.Column < Columns - margin &&
                   cell.Row >= margin &&
                   cell.Row < Rows - margin &&
                   IsFoundingSite(cell);
        }

        public bool IsFoundingSite(GeneratedWorldCell cell)
        {
            if (cell.Id.World != WorldId || !cell.IsFoundable)
            {
                return false;
            }

            int minimumColumn = cell.Column - FoundingClearanceCells;
            int maximumColumn = cell.Column + FoundingClearanceCells;
            int minimumRow = cell.Row - FoundingClearanceCells;
            int maximumRow = cell.Row + FoundingClearanceCells;
            if (minimumColumn < 0 || maximumColumn >= Columns || minimumRow < 0 || maximumRow >= Rows)
            {
                return false;
            }

            return WaterCount(minimumColumn, minimumRow, maximumColumn, maximumRow) == 0;
        }

        private int[] BuildWaterPrefix()
        {
            int stride = Columns + 1;
            var prefix = new int[stride * (Rows + 1)];
            for (int row = 0; row < Rows; row++)
            {
                int runningWater = 0;
                for (int column = 0; column < Columns; column++)
                {
                    if (cells[row * Columns + column].IsWater)
                    {
                        runningWater++;
                    }

                    prefix[(row + 1) * stride + column + 1] = prefix[row * stride + column + 1] + runningWater;
                }
            }

            return prefix;
        }

        private int WaterCount(int minimumColumn, int minimumRow, int maximumColumn, int maximumRow)
        {
            int stride = Columns + 1;
            int left = minimumColumn;
            int right = maximumColumn + 1;
            int bottom = minimumRow;
            int top = maximumRow + 1;
            return waterPrefix[top * stride + right] -
                   waterPrefix[bottom * stride + right] -
                   waterPrefix[top * stride + left] +
                   waterPrefix[bottom * stride + left];
        }

        private ulong ComputeChecksum()
        {
            CanonicalChecksum checksum = CanonicalChecksum.Create();
            checksum.Add("CIV-BUILD02-GENERATED-WORLD-v1");
            checksum.Add(WorldId);
            checksum.Add(Settings.GenerationVersion);
            checksum.Add(Settings.Seed.Value);
            checksum.Add((int)Settings.Size);
            checksum.Add(Settings.LandPercent);
            checksum.Add(Settings.TemperaturePercent);
            checksum.Add(Settings.RainfallPercent);
            checksum.Add(Settings.MountainPercent);
            checksum.Add(Settings.ForestPercent);
            checksum.Add(Settings.ResourcePercent);
            checksum.Add(Columns);
            checksum.Add(Rows);
            for (int index = 0; index < cells.Length; index++)
            {
                GeneratedWorldCell cell = cells[index];
                checksum.Add(cell.Id.World);
                checksum.Add(cell.Id.Local);
                checksum.Add(cell.Column);
                checksum.Add(cell.Row);
                checksum.Add(cell.ElevationPermille);
                checksum.Add(cell.MoisturePermille);
                checksum.Add(cell.TemperaturePermille);
                checksum.Add(cell.FertilityPermille);
                checksum.Add(cell.ResourcePermille);
                checksum.Add(cell.Resources.FreshWaterPermille);
                checksum.Add(cell.Resources.StapleFoodPermille);
                checksum.Add(cell.Resources.ProteinFoodPermille);
                checksum.Add(cell.Resources.TimberPermille);
                checksum.Add(cell.Resources.StonePermille);
                checksum.Add(cell.Resources.ClayPermille);
                checksum.Add(cell.Resources.FiberPermille);
                checksum.Add(cell.Resources.IronOrePermille);
                checksum.Add(cell.Resources.CoalPermille);
                checksum.Add(cell.Resources.MedicinalInputsPermille);
                checksum.Add((int)cell.Biome);
            }

            return checksum.Value;
        }
    }
}
