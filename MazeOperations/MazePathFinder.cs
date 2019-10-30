using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MazeOperations
{
    public class MazePathFinder
    {
        int _mapWidth, _mapHeight;

        MazeCell[,] _mazeMap;

        public class Chain //путь до ячейки (ребро графа)
        {
            public MazeCell CellCurrent { get; set; }
            public Chain Previous { get; set; }

            public Chain(MazeCell current, Chain prev = null)
            {
                CellCurrent = current;
                Previous = prev;
            }

            public static List<MazeCell> Traverse(Chain root)
            {
                var stack = new Stack<Chain>();
                var path = new List<MazeCell>();

                if (root != null) stack.Push(root);

                while (stack.Count > 0)
                {
                    var chain = stack.Pop();
                    path.Add(chain.CellCurrent);
                    if (chain.Previous == null)
                    {
                        path.Reverse();
                        return path;
                    }

                    stack.Push(chain.Previous);
                }
                return path;
            }
        }

        public MazePathFinder(Maze maze)//обход направлений в CreateChainTree
        {
            _mazeMap = maze.MazeCells;
            _mapHeight = maze.Height;
            _mapWidth = maze.Width;
        }

        public List<MazeCell> GetCellsPath(MazeCell source, MazeCell destination)
        {
            return Chain.Traverse(CreateChainTree(source, destination));
        }

        private Chain CreateChainTree(MazeCell source, MazeCell destination)
        {
            if (source.X == destination.X && source.Y == destination.Y)
            {
                throw new Exception("Точка начала совпадает с точкой выхода");
            }

            var queueChains = new Queue<Chain>();//очередь для поиска в ширину
            var visitedInLabirintPlaces = new List<MazeCell>();//уже посещённые вершины (ячейки)

            //начальная позиция
            var startChain = new Chain(new MazeCell(source.X, source.Y));

            queueChains.Enqueue(startChain);

            while (queueChains.Count > 0)
            {
                var chain = queueChains.Dequeue();
                if (chain.CellCurrent.Equals(destination))
                    return chain;

                if (visitedInLabirintPlaces.Contains(chain.CellCurrent))
                    continue;
                visitedInLabirintPlaces.Add(chain.CellCurrent);

                foreach (var place in GetNeighbours(chain, _mazeMap, _mapHeight, _mapWidth))
                {
                    queueChains.Enqueue(place);
                }
            }
            return null;
        }

        private bool InRange(MazeCell place, MazeCell[,] map, int h, int w)//проверка позиций
        {
            if (place.X < 0 || place.X >= w)
                return false;
            if (place.Y < 0 || place.Y >= h)
                return false;
            if (map[place.Y, place.X].CellType != CellType.None && map[place.Y, place.X].CellType != CellType.Exit)
                return false;
            return true;
        }

        private Collection<Chain> GetNeighbours(Chain chain, MazeCell[,] map, int h, int w)
        {
            var result = new Collection<Chain>();

            var neighborCells = new MazeCell[4];
            neighborCells[0] = new MazeCell(chain.CellCurrent.X + 1, chain.CellCurrent.Y, CellType.Exit);
            neighborCells[1] = new MazeCell(chain.CellCurrent.X - 1, chain.CellCurrent.Y, CellType.Exit);
            neighborCells[2] = new MazeCell(chain.CellCurrent.X, chain.CellCurrent.Y + 1, CellType.Exit);
            neighborCells[3] = new MazeCell(chain.CellCurrent.X, chain.CellCurrent.Y - 1, CellType.Exit);

            foreach (var cell in neighborCells)
            {
                if (!InRange(cell, map, h, w))
                    continue;
                // Заполняем данные для точки маршрута.
                var neighbourNode = new Chain(cell, chain);

                result.Add(neighbourNode);
            }
            return result;
        }
    }
}
