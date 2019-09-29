using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LabirintOperations
{
    public class LabirintTester
    {
        private MapPlace _startMapPlace;
        private MapPlace _exitMapPlace;

        private char[,] _labirintMap;
        private int _mapWidth, _mapHeight;
        private int _startX, _startY;//координаты A
        private int _exitX, _exitY;//финишные координаты
        private string _solution;
        private int _currentPosSolution;//текущих ход решения

        public bool RunTest(string labirintFilePath)
        {
            LoadLabirint(labirintFilePath, out _mapHeight, out _mapWidth, ref _startMapPlace, ref _exitMapPlace);
            _startMapPlace = new MapPlace(_startX, _startY);
            _exitMapPlace = new MapPlace(_exitX, _exitY);
            var levelOk = IsLevelCorrect(_startMapPlace, _exitMapPlace, _labirintMap);
            if (levelOk != "")
                throw new Exception(levelOk);
            //LoadSolution(solutionFile);
            var appleSolver = new LabirintSolver(_labirintMap);
            _solution = appleSolver.GetLabirintSolution(_startMapPlace, _exitMapPlace);
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            PrintLabirint(_mapHeight, _mapWidth, _labirintMap);
            for (var i = 0; i < _solution.Length - 1; i++)
            {
                switch (_solution[i])
                {
                    case '4':
                        if (!MoveDirectBySolution(-1, 0, ref _startX, ref _startY, ref _mapHeight, ref _mapWidth, ref _labirintMap))
                            return false;
                        break;
                    case '6':
                        if (!MoveDirectBySolution(1, 0, ref _startX, ref _startY, ref _mapHeight, ref _mapWidth, ref _labirintMap))
                            return false;
                        break;
                    case '2':
                        if (!MoveDirectBySolution(0, 1, ref _startX, ref _startY, ref _mapHeight, ref _mapWidth, ref _labirintMap))
                            return false;
                        break;
                    case '8':
                        if (!MoveDirectBySolution(0, -1, ref _startX, ref _startY, ref _mapHeight, ref _mapWidth, ref _labirintMap))
                            return false;
                        break;
                    default: return false;//решение неверно
                }
                PrintSolutionPath(_solution[i], _solution[i + 1], _startX, _startY);
            }
            PrintStartFinishLabels(_startMapPlace, _exitMapPlace);
            PrintSolutionText(_solution, _mapHeight);
            return (_startX == _exitX && _startY == _exitY);
        }

        public bool MoveDirectBySolution(int solX, int solY,
            ref int startX, ref int startY, ref int mapHeight, ref int mapWidth, ref char[,] map)
        {
            if (startX + solX < 0 || startX + solX >= mapWidth)
                return false;
            if (startY + solY < 0 || startY + solY >= mapHeight)
                return false;
            if (map[startX + solX, startY + solY] == '#')
                return false;
            //если может идти - true
            if (map[startX + solX, startY + solY] == ' ' ||
                map[startX + solX, startY + solY] == '.')
            {
                startX += solX;
                startY += solY;
                return true;
            }

            return true;
        }

        public char[,] LoadLabirint(string labirintFilePath, out int height, out int width,
            ref MapPlace startPlace, ref MapPlace exitPlace)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(labirintFilePath);
            }
            catch
            {
                throw new Exception("Не удалось считать файл исходных данных!");
            }
            var wh = lines[0].Split();
            height = int.Parse(wh[0]);
            width = int.Parse(wh[1]);
            var map = new char[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[y, x] = lines[y + 1][x];
                    switch (map[y, x])
                    {
                        //если A - находим координаты
                        case 'A':
                            startPlace.X = x;
                            startPlace.Y = y;
                            map[y, x] = ' ';//не рисуем A
                            break;
                        case '.':
                            exitPlace.X = x;
                            exitPlace.Y = y;
                            map[y, x] = ' ';
                            break;
                    }
                }
            }

            return map;
        }

        public void PrintSolutionPath(char direction, char next, int startX, int startY)
        {
            //выводим A, меняем цвет
            Console.SetCursorPosition(startX, startY);
            Console.ForegroundColor = ConsoleColor.Green;
            switch (direction)
            {
                case '4':
                    Console.Write(direction == next ? '—' : '<');
                    break;
                case '6':
                    Console.Write(direction == next ? '—' : '>');
                    break;
                case '2':
                    Console.Write(direction == next ? '|' : '↓');
                    break;
                case '8':
                    Console.Write(direction == next ? '|' : '↑');
                    break;
            }
        }

        private void PrintLabirint(int height, int width, char[,] map)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Console.Write(map[j, i]);
                }

                Console.WriteLine();
            }
        }

        private void PrintStartFinishLabels(MapPlace alphaPlace, MapPlace finishPlace)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(alphaPlace.X, alphaPlace.Y);
            Console.Write('S');
            Console.SetCursorPosition(finishPlace.X, finishPlace.Y);
            Console.Write('F');
        }

        private void PrintSolutionText(string solution, int height)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine(solution);
        }

        public int CountMapItems(char c, char[,] map)
        {
            var counter = 0;
            foreach (var _ in map)
            {
                if (_ == c)
                {
                    counter++;
                }
            }
            return counter;
        }

        public string IsLevelCorrect(MapPlace alpha, MapPlace exit, char[,] map)
        {
            var targetsOnMap = CountMapItems('.', map);
            var wallOnMap = CountMapItems('#', map);
            if (wallOnMap == 0)
            {
                return "Уровень пуст!";
            }
            if (alpha.X == 0 && alpha.Y == 0)
            {
                return "На карте должен быть хотя бы один объект игрока!";
            }
            if (exit.X == 0 && exit.Y == 0)
            {
                return "На карте должна быть одна цель!";
            }
            return "";
        }
    }
}
