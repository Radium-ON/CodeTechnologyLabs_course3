using System;
using System.Collections.Generic;
using System.Text;

namespace LabirintOperations
{
    public class Maze
    {
        public int Height => MazeCells.GetLength(0);

        public int Width => MazeCells.GetLength(1);

        public MazeCell[,] MazeCells { get; private set; }

        public Maze(MazeCell[,] mazeCells)
        {
            MazeCells = mazeCells;
        }
    }
}
