using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MazeOperations
{
    public class MazeIO
    {
        private readonly List<string> _mazeSettingsList;

        public MazeIO(string mazeSetupFilePath)
        {
            if (File.Exists(mazeSetupFilePath))
                _mazeSettingsList = File.ReadAllLines(mazeSetupFilePath).ToList();
            else
                File.Create(mazeSetupFilePath);
        }

        private CellType CharToCellType(char x)
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

        private char CellTypeToChar(MazeCell cell)
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

        private int[] ParseParamsLine(string line)
        {
            var parts = line.Split();
            var paramSet = new int[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out paramSet[i]))
                {
                    paramSet[i] = 0;
                }
            }
            return paramSet;
        }

        /// <summary>
        /// Возвращает начальную позицию в лабиринте
        /// </summary>
        /// <returns>Клетка лабиринта</returns>
        public MazeCell GetStartPlaceFromFile()
        {
            var start = ParseParamsLine(_mazeSettingsList[1]);

            return new MazeCell(start[1], start[0], CellType.Start);
        }

        /// <summary>
        /// Возвращает точку выхода в лабиринте
        /// </summary>
        /// <returns>Клетка лабиринта</returns>
        public MazeCell GetExitPlaceFromFile()
        {
            var exit = ParseParamsLine(_mazeSettingsList[2]);

            return new MazeCell(exit[1], exit[0], CellType.Exit);
        }

        /// <summary>
        /// Загружает матрицу клеток лабиринта из файла
        /// </summary>
        /// <returns>Матрица клеток</returns>
        public Maze LoadMazeFromFile()
        {
            if (_mazeSettingsList.Count == 0)
            {
                throw new Exception("Файл не содержит ни одной строки");
            }
            var size = ParseParamsLine(_mazeSettingsList[0]);
            var height = size[0];
            var width = size[1];
            var map = new MazeCell[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[y, x] = new MazeCell(x, y, CharToCellType(_mazeSettingsList[y + 3][x]));
                }
            }
            return new Maze(map);
        }
    }
}
