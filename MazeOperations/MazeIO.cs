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
            {
                _mazeSettingsList = File.ReadAllLines(mazeSetupFilePath).ToList();
            }
            else
                File.Create(mazeSetupFilePath);
        }
        /// <summary>
        /// Возвращает ячейку в зависимости от считанного символа
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private CellType CharToCellType(char x)
        {
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
        /// <summary>
        /// Возвращает символ в зависимости от ячейки
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private char CellTypeToChar(MazeCell cell)
        {
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
        /// Загружает матрицу клеток лабиринта из файла
        /// </summary>
        /// <returns>Матрица клеток</returns>
        public Maze LoadMazeFromFile()
        {
            if (_mazeSettingsList.Count == 0)
            {
                throw new EmptyDataFileException("Файл не содержит ни одной строки");
            }
            var size = ParseParamsLine(_mazeSettingsList[0]);
            var height = size[0];
            var width = size[1];

            var map = new MazeCell[height, width];

            MazeCell start = default, exit = default;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[y, x] = new MazeCell(x, y, CharToCellType(_mazeSettingsList[y + 1][x]));

                    switch (map[y, x].CellType)
                    {
                        case CellType.Start:
                            start = map[y, x];
                            break;
                        case CellType.Exit:
                            exit = map[y, x];
                            break;
                    }
                }
            }
            return new Maze(map, start, exit);
        }
    }
}
