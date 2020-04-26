namespace MazeOperations
{
    public struct MazeCell
    {
        public MazeCell(int x, int y, CellType type = CellType.None)
        {
            if (x >= 0 && y >= 0)
            {
                X = x;
                Y = y;
            }
            else
            {
                X = 0;
                Y = 0;
            }
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
