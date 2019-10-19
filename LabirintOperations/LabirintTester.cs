using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LabirintOperations
{
    public class LabirintTester
    {
        private MazeCell _startMazeCell = new MazeCell(0, 0);
        private MazeCell _exitMazeCell = new MazeCell(0, 0);

        private MazeCell[,] _labirintMap;
        private int _mapWidth, _mapHeight;
        private int _startX, _startY;//координаты A
        private int _exitX, _exitY;//финишные координаты
        private int _currentPosSolution;//текущих ход решения

        public string Solution { get; set; }

        public MazeCell StartMazeCell
        {
            get { return _startMazeCell; }
        }

        public MazeCell ExitMazeCell
        {
            get { return _exitMazeCell; }
        }

        public int MapWidth
        {
            get { return _mapWidth; }
        }

        public int MapHeight
        {
            get { return _mapHeight; }
        }

        public MazeCell[,] LabirintMap
        {
            get { return _labirintMap; }
        }

        public LabirintTester(string labirintFilePath)
        {
            _labirintMap = LoadLabirint(labirintFilePath, out _mapHeight, out _mapWidth, ref _startMazeCell, ref _exitMazeCell);
            _startX = _startMazeCell.X;
            _startY = _startMazeCell.Y;
            _exitX = _exitMazeCell.X;
            _exitY = _exitMazeCell.Y;
        }

        /// <summary>
        /// Тестирование пути выхода из лабиринта
        /// </summary>
        /// <param name="solution">Строка пути решения лабиринта</param>
        /// <returns>True - лабиринт пройден. Иначе False</returns>
        public bool RunSolutionTest(List<MazeCell> solution)
        {
            if (solution.Count==0)
                return false;
            var levelOk = IsLevelCorrect(_startMazeCell, _exitMazeCell, _labirintMap);
            if (levelOk != "")
                throw new Exception(levelOk);

            for (var i = 0; i < solution.Count; i++)
            {
                if (!MoveDirectBySolution(solution[i], _mapHeight, _mapWidth, _labirintMap))
                    break;
            }

            var x = solution[solution.Count - 1].X;
            var y = solution[solution.Count - 1].Y;
            return LabirintMap[y, x].CellType == ExitMazeCell.CellType;
        }

        private bool MoveDirectBySolution(MazeCell cell, int mapHeight, int mapWidth, MazeCell[,] map)
        {
            if (cell.X < 0 || cell.X >= mapWidth)
                return false;
            if (cell.Y < 0 || cell.Y >= mapHeight)
                return false;
            if (map[cell.Y, cell.X].CellType == CellType.Wall)
                return false;
            //если может идти - true
            if (map[cell.Y, cell.X].CellType == CellType.None ||
                map[cell.Y, cell.X].CellType == CellType.Exit)
            {
                return true;
            }

            return true;
        }

        public CellType CharToCellType(char x)
        {
            //возвращает ячейку в зависимости от считанного символа
            switch (x)
            {
                case ' ': return CellType.None;
                case '#': return CellType.Wall;
                case 'S': return CellType.Start;
                case 'X': return CellType.Exit;
                default:
                    return CellType.None;
            }
        }

        public char CellTypeToChar(MazeCell cell)
        {
            //возвращает символ в зависимости от ячейки
            switch (cell.CellType)
            {
                case CellType.None: return ' ';
                case CellType.Wall: return '#';
                case CellType.Start: return 'S';
                case CellType.Exit: return 'X';
                default: return ' ';


            }
        }

        private MazeCell[,] LoadLabirint(string labirintFilePath, out int height, out int width,
            ref MazeCell startPlace, ref MazeCell exitPlace)
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
            var map = new MazeCell[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[y, x] = new MazeCell(x, y, CharToCellType(lines[y + 3][x]));
                    switch (map[y, x].CellType)
                    {
                        //если A - находим координаты
                        case CellType.Start:
                            startPlace = new MazeCell(x, y, CellType.Start);
                            break;
                        case CellType.Exit:
                            exitPlace = new MazeCell(x, y, CellType.Exit);
                            break;
                    }
                }
            }

            return map;
        }

        private int CountMapItems(CellType type, MazeCell[,] map)
        {
            var counter = 0;
            foreach (var _ in map)
            {
                if (_.CellType == type)
                {
                    counter++;
                }
            }
            return counter;
        }

        public string IsLevelCorrect(MazeCell alpha, MazeCell exit, MazeCell[,] map)
        {
            var targetsOnMap = CountMapItems(CellType.Exit, map);
            var wallOnMap = CountMapItems(CellType.Wall, map);
            if (wallOnMap == 0)
            {
                return "Уровень пуст!";
            }
            if (alpha.X == 0 && alpha.Y == 0)
            {
                return "На карте должен быть хотя бы один объект игрока!";
            }
            if (exit.X == 0 && exit.Y == 0 || targetsOnMap!=1)
            {
                return "На карте должна быть одна цель!";
            }
            return "";
        }
    }
}
