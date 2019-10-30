using System;
using System.IO;

namespace MazeOperations
{
    public static class MazeIO
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

        public static char CellTypeToChar(MazeCell cell)
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

        private static string ReadLevelSettingLine(string filepath, int lineCount, int certainLine)
        {
            var listLines = new string[lineCount];

            using (var reader = new StreamReader(filepath))
            {
                for (var i = 0; i < lineCount; i++)
                {
                    listLines[i] = reader.ReadLine();
                }
            }

            return listLines[certainLine - 1];
        }

        private static int[] ParseParamsLine(string line)
        {
            var parts = line.Split();
            var paramSet = new int[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                int.TryParse(parts[i], out paramSet[i]);
            }
            return paramSet;
        }

        /// <summary>
        /// Возвращает начальную позицию в лабиринте
        /// </summary>
        /// <param name="mazeSetupFilePath">Путь к файлу лабиринта</param>
        /// <returns>Клетка лабиринта</returns>
        public static MazeCell GetStartPlace(string mazeSetupFilePath)
        {
            var paramString = ReadLevelSettingLine(mazeSetupFilePath, 2, 2);

            var start = ParseParamsLine(paramString);

            return new MazeCell(start[1], start[0], CellType.Start);
        }

        /// <summary>
        /// Возвращает точку выхода в лабиринте
        /// </summary>
        /// <param name="mazeSetupFilePath">Путь к файлу лабиринта</param>
        /// <returns>Клетка лабиринта</returns>
        public static MazeCell GetExitPlace(string mazeSetupFilePath)
        {
            var paramString = ReadLevelSettingLine(mazeSetupFilePath, 3, 3);

            var exit = ParseParamsLine(paramString);

            return new MazeCell(exit[1], exit[0], CellType.Exit);
        }

        /// <summary>
        /// Загружает матрицу клеток лабиринта из файла
        /// </summary>
        /// <param name="mazeSetupFilePath">Путь к файлу лабиринта</param>
        /// <returns>Матрица клеток</returns>
        public static Maze LoadLabirint(string mazeSetupFilePath)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(mazeSetupFilePath);
            }
            catch
            {
                throw new Exception("Не удалось считать файл исходных данных!");
            }

            if (lines.Length == 0)
            {
                throw new Exception("Файл не содержит ни одной строки");
            }
            var size = ParseParamsLine(lines[0]);
            var height = size[0];
            var width = size[1];
            var map = new MazeCell[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[y, x] = new MazeCell(x, y, CharToCellType(lines[y + 3][x]));
                }
            }
            return new Maze(map);
        }
    }
}
