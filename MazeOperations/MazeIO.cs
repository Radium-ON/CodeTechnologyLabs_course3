using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeOperations
{
    public class MazeIO
    {
        private List<string> _mazeSettingsList;

        public MazeIO()
        {

        }

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

        public async Task ReadMazeFromFileTaskAsync(string mazeSetupFilePath)
        {
            if (File.Exists(mazeSetupFilePath))
            {
                _mazeSettingsList = await ReadAllLinesAsync(mazeSetupFilePath);
                //имитация долгой работы
                await Task.Delay(2000);
            }
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

        private async Task<List<string>> ReadAllLinesAsync(string filePath)
        {
            using var fs = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true);

            using var sr = new StreamReader(fs, Encoding.UTF8);
            
            var content = await sr.ReadToEndAsync();

            return content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        public async Task<Maze> LoadMazeFromFileAsync()
        {
            var task = Task.Run(LoadMazeFromFile);

            var result = await task;

            return result;
        }
    }
}
