using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeOperations
{
    public class MazeIO
    {
        private List<string> _mazeSettingsList;



        /// <summary>
        /// Возвращает ячейку в зависимости от считанного символа
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private CellType CharToCellType(char x)
        {
            return x switch
            {
                ' ' => CellType.None,
                '#' => CellType.Wall,
                'S' => CellType.Start,
                'X' => CellType.Exit,
                _ => CellType.None
            };
        }

        /// <summary>
        /// Возвращает символ в зависимости от ячейки
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private char CellTypeToChar(MazeCell cell)
        {
            return cell.CellType switch
            {
                CellType.None => ' ',
                CellType.Wall => '#',
                CellType.Start => 'S',
                CellType.Exit => 'X',
                _ => ' '
            };
        }

        /// <summary>
        /// Разбивает строку входных данных на числовые параметры. Возвращает целочисленный массив параметров.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
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
        /// Асинхронно читает файл лабиринта в список массивов <see cref="string"/> и поддерживает отмену операции
        /// </summary>
        /// <param name="mazeSetupFilePath"></param>
        /// <param name="token">Объект <see cref="CancellationToken"/> для отмены операции.</param>
        /// <returns></returns>
        public async Task ReadMazeFromFileTaskAsync(string mazeSetupFilePath, CancellationToken token = default)
        {
            if (File.Exists(mazeSetupFilePath))
            {
                _mazeSettingsList = await ReadAllLinesAsync(mazeSetupFilePath, token);
            }
        }

        /// <summary>
        /// Создает матрицу клеток лабиринта
        /// </summary>
        /// <returns>Матрица клеток</returns>
        public Maze CreateMazeMatrix()
        {
            return CreateMazeMatrixAsync(CancellationToken.None);
        }

        /// <summary>
        /// Асинхронный метод, создающий матрицу клеток лабиринта с поддержкой отмены
        /// </summary>
        /// <param name="token">Токен отмены задачи</param>
        /// <returns></returns>
        private Maze CreateMazeMatrixAsync(CancellationToken token)
        {
            var size = ParseParamsLine(_mazeSettingsList[0]);
            var height = size[0];
            var width = size[1];

            var map = new MazeCell[height, width];

            MazeCell start = default, exit = default;

            for (var y = 0; y < height; y++)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                for (var x = 0; x < width; x++)
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

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

        /// <summary>
        /// Асихронная версия File.ReadAllLines
        /// </summary>
        /// <param name="filePath">Путь к файлу лабиринта</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        private async Task<List<string>> ReadAllLinesAsync(string filePath, CancellationToken token)
        {
            using var fs = new FileStream(filePath,
                 FileMode.Open, FileAccess.Read, FileShare.Read,
                 4096, true);

            using var sr = new StreamReader(fs, Encoding.UTF8);

            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }
            var content = await sr.ReadToEndAsync();

            if (string.IsNullOrEmpty(content))
            {
                throw new EmptyDataFileException("Файл не содержит ни одной строки");
            }

            return content.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        /// <summary>
        /// Асинхронно загружает матрицу клеток лабиринта из файла
        /// </summary>
        /// <param name="token">Объект <see cref="CancellationToken"/> для отмены операции.</param>
        /// <returns></returns>
        public Task<Maze> СreateMazeMatrixAsync(CancellationToken token)
        {
            return Task.Run(() => CreateMazeMatrixAsync(token), token);
        }
    }
}
