﻿using System;
using System.Collections.Generic;

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
            if (solution.Count == 0)
                return false;
            var levelOk = IsLevelCorrect(_startMazeCell, _exitMazeCell, _mazeMap);
            if (levelOk != "")
                throw new Exception(levelOk);

            for (var i = 0; i < solution.Count; i++)
            {
                if (!MoveDirectBySolution(solution[i], _mapHeight, _mapWidth, _mazeMap))
                    break;
            }

            var x = solution[solution.Count - 1].X;
            var y = solution[solution.Count - 1].Y;
            return _mazeMap[y, x].CellType == _exitMazeCell.CellType;
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
            if (exit.X == 0 && exit.Y == 0 || targetsOnMap != 1)
            {
                return "На карте должна быть одна цель!";
            }
            return "";
        }
    }
}