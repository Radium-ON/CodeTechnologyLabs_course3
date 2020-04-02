using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MazeOperations
{
    public class MazePathFinder
    {
        readonly int _mapWidth;
        readonly int _mapHeight;

        readonly MazeCell[,] _mazeMap;

        /// <summary>
        /// Содержит путь от начальной позиции до конечной в виде графа вершин
        /// </summary>
        private class Chain
        {
            public MazeCell CurrentCell { get; }
            public Chain PreviousChain { get; }

            public Chain(MazeCell current, Chain prev = null)
            {
                CurrentCell = current;
                PreviousChain = prev;
            }

            public static List<MazeCell> Traverse(Chain root)
            {
                var stack = new Stack<Chain>();
                var path = new List<MazeCell>();

                if (root != null)
                {
                    stack.Push(root);
                }

                while (stack.Count > 0)
                {
                    var chain = stack.Pop();
                    path.Add(chain.CurrentCell);
                    if (chain.PreviousChain == null)
                    {
                        path.Reverse();
                        return path;
                    }

                    stack.Push(chain.PreviousChain);
                }
                return path;
            }
        }

        public MazePathFinder(Maze maze)
        {
            if (maze == null)
            {
                throw new ArgumentNullException(nameof(maze));
            }

            _mazeMap = maze.MazeCells;
            _mapHeight = maze.Height;
            _mapWidth = maze.Width;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">Начальная точка пути в лабиринте</param>
        /// <param name="destination">Конечная точка пути в лабиринте</param>
        /// <returns>Список координат точек между начальной и конечной точкой лабиринта</returns>
        public List<MazeCell> GetCellsPath(MazeCell source, MazeCell destination)
        {
            return Chain.Traverse(CreateChainTree(source, destination));
        }

        private Chain CreateChainTree(MazeCell source, MazeCell destination)
        {
            if (source.X == destination.X && source.Y == destination.Y)
            {
                throw new StartEqualsFinishException("Точка начала совпадает с точкой выхода");
            }

            var queueChains = new Queue<Chain>();//очередь для поиска в ширину
            var visitedInLabirintPlaces = new List<MazeCell>();//уже посещённые вершины (ячейки)

            //начальная позиция
            var startChain = new Chain(new MazeCell(source.X, source.Y));

            queueChains.Enqueue(startChain);

            while (queueChains.Count > 0)
            {
                var chain = queueChains.Dequeue();
                if (chain.CurrentCell.Equals(destination))
                {
                    return chain;
                }

                if (visitedInLabirintPlaces.Contains(chain.CurrentCell))
                {
                    continue;
                }

                visitedInLabirintPlaces.Add(chain.CurrentCell);

                foreach (var place in GetProperStepDirections(chain,GetNeighbours(chain),_mazeMap,_mapHeight,_mapWidth))
                {
                    queueChains.Enqueue(place);
                }
            }
            throw new SolutionNotExistException("Путь между точками не найден!");
        }

        private bool StepInRange(MazeCell place, MazeCell[,] map, int h, int w)//проверка позиций
        {
            if (place.X < 0 || place.X >= w)
            {
                return false;
            }

            if (place.Y < 0 || place.Y >= h)
            {
                return false;
            }

            return map[place.Y, place.X].CellType == CellType.None || map[place.Y, place.X].CellType == CellType.Exit;
        }

        private IEnumerable<MazeCell> GetNeighbours(Chain chain)
        {
            var neighborCells = new MazeCell[4];

            neighborCells[0] = new MazeCell(chain.CurrentCell.X + 1, chain.CurrentCell.Y, CellType.Exit);
            neighborCells[1] = new MazeCell(chain.CurrentCell.X - 1, chain.CurrentCell.Y, CellType.Exit);
            neighborCells[2] = new MazeCell(chain.CurrentCell.X, chain.CurrentCell.Y + 1, CellType.Exit);
            neighborCells[3] = new MazeCell(chain.CurrentCell.X, chain.CurrentCell.Y - 1, CellType.Exit);

            return neighborCells;
        }

        private ICollection<Chain> GetProperStepDirections(Chain chain, IEnumerable<MazeCell> neighborCells, MazeCell[,] map, int h, int w)
        {
            var result = new Collection<Chain>();

            foreach (var cell in neighborCells)
            {
                if (!StepInRange(cell, map, h, w))
                {
                    continue;
                }

                // Заполняем данные для точки маршрута.
                var neighbourNode = new Chain(cell, chain);

                result.Add(neighbourNode);
            }

            return result;
        }
    }
}
