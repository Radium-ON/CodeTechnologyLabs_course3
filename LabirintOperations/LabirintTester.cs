using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LabirintOperations
{
    public class LabirintTester
    {
        InLabirintPlace _startInLabirintPlace;
        InLabirintPlace _exitInLabirintPlace;

        char[,] _labirintMap;
        int _mapWidth, _mapHeight;
        int _startX, _startY;//координаты A
        int _finishX, _finishY;//финишные координаты
        string _solution;
        int _currentPosSolution;//текущих ход решения

        public bool RunTest(string labirintFilePath)
        {
            LoadLabirint(labirintFilePath, out _mapHeight, out _mapWidth);
            _startInLabirintPlace = new InLabirintPlace(_startX, _startY);
            _exitInLabirintPlace = new InLabirintPlace(_finishX, _finishY);
            var levelOk = IsLevelCorrect(_startInLabirintPlace, _exitInLabirintPlace, _labirintMap);
            if (levelOk != "")
                throw new Exception(levelOk);
            //LoadSolution(solutionFile);
            var appleSolver = new LabirintSolver(_labirintMap);
            _solution = appleSolver.GetLabirintSolution(_startInLabirintPlace, _exitInLabirintPlace);
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
            PrintStartFinishLabels(_startInLabirintPlace, _exitInLabirintPlace);
            PrintSolutionText(_solution, _mapHeight);
            return (_startX == _finishX && _startY == _finishY);
        }
        private bool MoveDirectBySolution(int solX, int solY,
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

        public char[,] LoadLabirint(string labirintFilePath, out int height, out int width)
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
            string[] wh = lines[0].Split();
            height = int.Parse(wh[0]);
            width = int.Parse(wh[1]);
            var map = new char[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = lines[y + 1][x];
                    if (map[x, y] == 'A')//если A - находим координаты
                    {
                        _startX = x;
                        _startY = y;
                        map[x, y] = ' ';//не рисуем A
                    }
                    else if (map[x, y] == '.')
                    {
                        _finishX = x;
                        _finishY = y;
                        map[x, y] = ' ';
                    }
                }
            }

            return map;
        }

        private void PrintSolutionPath(char direction, char next, int startX, int startY)
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

        private void PrintStartFinishLabels(InLabirintPlace alphaPlace, InLabirintPlace finishPlace)
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

        private int CountMapItems(char c, char[,] map)
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

        private string IsLevelCorrect(InLabirintPlace alpha, InLabirintPlace exit, char[,] map)
        {
            int targetsOnMap = CountMapItems('.', map);
            int wallOnMap = CountMapItems('#', map);
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
