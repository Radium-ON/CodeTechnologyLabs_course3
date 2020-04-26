using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeOperations
{
    public class MazePathSolutionTester
    {
        private readonly MazeCell _startMazeCell;
        private readonly MazeCell _exitMazeCell;

        private readonly MazeCell[,] _mazeMap;

        private readonly int _mapWidth, _mapHeight;

        public MazePathSolutionTester(Maze maze, MazeCell start, MazeCell exit)
        {
            if (maze == null)
            {
                throw new ArgumentNullException(nameof(maze));
            }

            _mazeMap = maze.MazeCells;
            _startMazeCell = start;
            _exitMazeCell = exit;
            _mapHeight = maze.Height;
            _mapWidth = maze.Width;
        }

        /// <summary>
        /// Тестирование пути выхода из лабиринта
        /// </summary>
        /// <param name="solution">Строка пути решения лабиринта</param>
        /// <returns>True - лабиринт пройден. Иначе False</returns>
        public bool RunSolutionTest(List<MazeCell> solution)
        {
            if (solution == null)
            {
                throw new SolutionNotExistException(
                    "Решение лабиринта не было создано или найдено!",
                    new ArgumentNullException(nameof(solution)));
            }

            if (solution.Count == 0)
            {
                return false;
            }

            var levelOk = IsLevelCorrect(_startMazeCell, _exitMazeCell, _mazeMap);
            if (levelOk != "")
            {
                throw new LevelIsNotCorrectException(levelOk);
            }

            foreach (var step in solution.Where(step => !MoveDirectBySolution(step, _mapHeight, _mapWidth, _mazeMap)))
            {
                break;
            }

            var x = solution[solution.Count - 1].X;
            var y = solution[solution.Count - 1].Y;
            return _mazeMap[y, x].CellType == _exitMazeCell.CellType;
        }


        private bool MoveDirectBySolution(MazeCell cell, int mapHeight, int mapWidth, MazeCell[,] map)
        {
            if (cell.X < 0 || cell.X >= mapWidth)
            {
                return false;
            }

            if (cell.Y < 0 || cell.Y >= mapHeight)
            {
                return false;
            }

            switch (map[cell.Y, cell.X].CellType)
            {
                case CellType.Wall:
                    return false;
                //если может идти - true
                case CellType.None:
                case CellType.Exit:
                    return true;
                default:
                    return true;
            }
        }


        private int CountMapItems(CellType type, MazeCell[,] map)
        {
            return map.Cast<MazeCell>().Count(_ => _.CellType == type);
        }

        //TODO: обрабатывает ошибки через возвращаемое значение. Следовало бы использовать исключения 
        private string IsLevelCorrect(MazeCell alpha, MazeCell exit, MazeCell[,] map)
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

            if (exit.X == 0 && exit.Y == 0 || targetsOnMap != 1)
            {
                return "На карте должна быть одна цель!";
            }

            return "";
        }
    }
}