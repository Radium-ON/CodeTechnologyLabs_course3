﻿using System;
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

        public string Solution
        {
            get { return _solution; }
        }

        public MapPlace StartMapPlace
        {
            get { return _startMapPlace; }
        }

        public MapPlace ExitMapPlace
        {
            get { return _exitMapPlace; }
        }

        public int MapWidth
        {
            get { return _mapWidth; }
        }

        public int MapHeight
        {
            get { return _mapHeight; }
        }

        public char[,] LabirintMap
        {
            get { return _labirintMap; }
        }

        public LabirintTester()
        {

        }

        public LabirintTester(MapPlace startMapPlace, MapPlace exitMapPlace, string solution)
        {
            _startMapPlace = startMapPlace;
            _exitMapPlace = exitMapPlace;
            _startX = startMapPlace.X;
            _startY = startMapPlace.Y;
            _exitX = exitMapPlace.X;
            _exitY = exitMapPlace.Y;
            _solution = solution;
        }
        /// <summary>
        /// Тестирование пути выхода из лабиринта
        /// </summary>
        /// <param name="labirintFilePath">Путь к файлу лабиринта</param>
        /// <param name="solution">строка пути решения лабиринта</param>
        /// <returns>True - лабиринт пройден. Иначе False</returns>
        public bool RunSolutionTest(string labirintFilePath, string solution)
        {
            _labirintMap = LoadLabirint(labirintFilePath, out _mapHeight, out _mapWidth, ref _startMapPlace, ref _exitMapPlace);
            _startMapPlace = new MapPlace(_startX, _startY);
            _exitMapPlace = new MapPlace(_exitX, _exitY);
            var levelOk = IsLevelCorrect(StartMapPlace, ExitMapPlace, LabirintMap);
            if (levelOk != "")
                throw new Exception(levelOk);
            //LoadSolution(solutionFile);
            //var appleSolver = new LabirintSolver(LabirintMap);
            //_solution = appleSolver.GetLabirintSolution(StartMapPlace, ExitMapPlace);
            //Console.OutputEncoding = System.Text.Encoding.Unicode;
            //PrintLabirint(MapHeight, MapWidth, LabirintMap);
            for (var i = 0; i < solution.Length - 1; i++)
            {
                switch (solution[i])
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
                //PrintSolutionPath(solution[i], solution[i + 1], _startX, _startY);
            }
            //PrintStartFinishLabels(StartMapPlace, ExitMapPlace);
            //PrintSolutionText(Solution, MapHeight);
            return (_startX == _exitX && _startY == _exitY);
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
