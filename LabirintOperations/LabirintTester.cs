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
            _labirintMap = LabirintIO.LoadLabirint(labirintFilePath, out _mapHeight, out _mapWidth, ref _startMazeCell, ref _exitMazeCell);
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
