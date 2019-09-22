using System;
using System.Collections.Generic;
using System.Text;

namespace LabirintOperations
{
    public class LabirintSolver
    {
        int _width, _height;

        List<Chain> _directionChains;//направления движения в координатах
        char[,] _labirintMap;

        struct Chain //путь до ячейки (ребро графа)
        {
            public int X;
            public int Y;
            public string Path;

            public Chain(int x, int y, string path)
            {
                X = x;
                Y = y;
                Path = path;
            }

            public Chain(InLabirintPlace place, string path)
            {
                X = place.X;
                Y = place.Y;
                Path = path;
            }
        }

        public LabirintSolver(char[,] labirintMap)//обход направлений в MoveDirect
        {
            this._labirintMap = labirintMap;
            _width = labirintMap.GetLength(0);
            _height = labirintMap.GetLength(1);
            _directionChains = new List<Chain>
            {
                new Chain(-1, 0, "4"), new Chain(1, 0, "6"), new Chain(0, -1, "8"), new Chain(0, 1, "2")
            };
        }
        public string MoveDirect(InLabirintPlace source, InLabirintPlace destination)
        {
            if (source.X==destination.X && source.Y==destination.Y)
            {
                return "";
            }

            Queue<Chain> queueChains = new Queue<Chain>();//очередь для поиска в ширину
            List<InLabirintPlace> visitedInLabirintPlaces = new List<InLabirintPlace>();//уже посещённые вершины (ячейки)

            queueChains.Clear();
            visitedInLabirintPlaces.Clear();

            //начальная позиция игрока
            Chain chain;
            chain.X = source.X;
            chain.Y = source.Y;
            chain.Path = "";

            InLabirintPlace place;//новые координаты
            queueChains.Enqueue(chain);

            while (queueChains.Count > 0)
            {
                chain = queueChains.Dequeue();
                foreach (var side in _directionChains)
                {
                    place.X = chain.X + side.X;
                    place.Y = chain.Y + side.Y;
                    if (!InRange(place))
                        continue;
                    if (visitedInLabirintPlaces.Contains(place))
                        continue;
                    visitedInLabirintPlaces.Add(place);//добавили координату, куда переместились
                    //отобразили, куда сдвинулись
                    var stepUser = new Chain(place, chain.Path + side.Path);
                    if (place.Equals(destination))
                        return stepUser.Path+" ";
                    queueChains.Enqueue(stepUser);
                }

            }
            return "-";
        }

        private bool InRange(InLabirintPlace place)//проверка позиций
        {
            if (place.X < 0 || place.X >= _width)
                return false;
            if (place.Y < 0 || place.Y >= _height)
                return false;
            if (_labirintMap[place.X, place.Y] != ' ')
                return false;
            return true;
        }
    }
}
