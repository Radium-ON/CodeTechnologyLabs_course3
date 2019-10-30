namespace MazeOperations
{
    public struct MazeCell
    {
        public MazeCell(int x, int y, CellType type = CellType.None)
        {
            X = x;//j
            Y = y;//i
            CellType = type;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public CellType CellType { get; private set; }

        #region Overrides of Object

        public override string ToString()
        {
            return $"XY({X};{Y}), Type={CellType}";
        }

        #endregion
    }
}
