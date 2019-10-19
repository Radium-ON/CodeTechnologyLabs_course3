using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LabirintOperations
{
    public class LabirintSolver
    {
        int _width, _height;

        List<Point> _directionChains;//направления движения в координатах
        MazeCell[,] _labirintMap;

        public class Chain //путь до ячейки (ребро графа)
        {
            public MazeCell CellCurrent { get; set; }
            //public Chain Previous { get; set; }
            public Chain Next { get; set; }
            public List<MazeCell> Path { get; set; } = new List<MazeCell>();

            public Chain(MazeCell current, Chain next = null)
            {
                CellCurrent = current;
                Next = next;
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
                    if (chain.Next == null)
                        return path;

                    stack.Push(chain.Next);
                }
                return path;
            }
        }

        public LabirintSolver(MazeCell[,] labirintMap)//обход направлений в CreateChainPath
        {
            _labirintMap = labirintMap;
            _height = labirintMap.GetLength(0);
            _width = labirintMap.GetLength(1);
            _directionChains = new List<Point>
            {
                new Point(-1,0),
                new Point(0, 1),
                new Point( 1, 0),
                new Point(0, -1)
            };
        }

        public List<MazeCell> GetCellsPath(MazeCell source, MazeCell destination)
        {
            return CreateChainPath(source, destination);
        }

        private List<MazeCell> CreateChainPath(MazeCell source, MazeCell destination)
        {
            if (source.X == destination.X && source.Y == destination.Y)
            {
                throw new Exception("Точка начала совпадает с точкой выхода");
            }

            var queueChains = new Queue<Chain>();//очередь для поиска в ширину
            var visitedInLabirintPlaces = new List<MazeCell>();//уже посещённые вершины (ячейки)
            var stepUser = new Chain(new MazeCell(source.X, source.Y));

            //начальная позиция игрока
            var startChain = new Chain(new MazeCell(source.X, source.Y));
            //MazeCell place;//новые координаты
            queueChains.Enqueue(startChain);

            while (queueChains.Count > 0)
            {
                var chain = queueChains.Dequeue();
                //chain.Previous = stepUser;
                foreach (var side in _directionChains)
                {
                    var place = new MazeCell(chain.CellCurrent.X + side.X, chain.CellCurrent.Y + side.Y, CellType.Exit);
                    if (visitedInLabirintPlaces.Contains(place))
                        continue;
                    if (!InRange(place, _labirintMap))
                        continue;
                    visitedInLabirintPlaces.Add(place);//добавили координату, куда переместились

                    stepUser = new Chain(place);
                    var newlist = new List<MazeCell>(chain.Path) { place };
                    stepUser.Path = new List<MazeCell>(newlist);
                    //отобразили, куда сдвинулись
                    //chain.Next = stepUser;
                    if (place.Equals(destination))
                        return stepUser.Path;
                    queueChains.Enqueue(stepUser);
                }

            }
            return new List<MazeCell>();
        }

        private bool InRange(MazeCell place, MazeCell[,] map)//проверка позиций
        {
            if (place.X < 0 || place.X >= _width)
                return false;
            if (place.Y < 0 || place.Y >= _height)
                return false;
            if (map[place.Y, place.X].CellType != CellType.None && map[place.Y, place.X].CellType != CellType.Exit)
                return false;
            return true;
        }
    }
}
