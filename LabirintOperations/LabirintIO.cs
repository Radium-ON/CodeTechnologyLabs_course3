using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LabirintOperations
{
    public static class LabirintIO
    {
        private static CellType CharToCellType(char x)
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

        private static char CellTypeToChar(MazeCell cell)
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

        private static int[] ReadLeverHeader(string line)
        {
            var parts = line.Split();
            var paramSet = new int[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                int.TryParse(parts[i], out paramSet[i]);
            }
            return paramSet;
        }

        public static MazeCell[,] LoadLabirint(string labirintFilePath, out int height, out int width, ref MazeCell startPlace, ref MazeCell exitPlace)
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

            var header = ReadLeverHeader(lines[0]);
            height = header[0];
            width = header[1];
            var map = new MazeCell[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[y, x] = new MazeCell(x, y, CharToCellType(lines[y + 1][x]));
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

    }
}
