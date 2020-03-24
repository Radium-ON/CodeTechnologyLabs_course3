using System;

namespace MazeOperations
{
    public class Maze
    {
        public int Height => MazeCells.GetLength(0);

        public int Width => MazeCells.GetLength(1);

        public MazeCell StartCellPosition { get; }

        public MazeCell ExitCellPosition { get; }

        public MazeCell[,] MazeCells { get; }

        public Maze(MazeCell[,] mazeCells, MazeCell startCell, MazeCell exitCell)
        {
            MazeCells = mazeCells ?? throw new ArgumentNullException(nameof(mazeCells));
            StartCellPosition = startCell;
            ExitCellPosition = exitCell;
        }
    }
}
