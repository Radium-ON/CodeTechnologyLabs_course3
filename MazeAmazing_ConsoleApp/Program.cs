using MazeOperations;
using System;
using System.Collections.Generic;
using System.IO;

namespace MazeAmazing_ConsoleApp
{
    public class Program
    {
        private static string _mazeMapFilePath = @"labirintDebug.txt";
        private static string _solutionFilePath = @"solutionDebug.txt";



        public MazePathSolutionTester Tester { get; set; }
        public MazePathFinder Finder { get; set; }

        public Maze Maze { get; set; }

        public List<MazeCell> Solution { get; set; }

        public MazeCell StartPlace { get; set; }
        public MazeCell ExitPlace { get; set; }

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                _mazeMapFilePath = args[0];
                if (args.Length == 2)
                    _solutionFilePath = args[1];
                if (Path.GetExtension(_mazeMapFilePath) != ".txt")
                    throw new Exception("Файл данных не существует или его расширение неверно!");
                if (Path.GetExtension(_solutionFilePath) != ".txt")
                    throw new Exception("Файл состояния не существует или его расширение неверно!");
            }
            var program = new Program();
            if (program.Run())
            {
                Console.WriteLine("Решение найдено");
            }
            else
                Console.WriteLine("Решение не найдено");
            Console.ReadKey(true);
        }

        private bool Run()
        {
            Maze = MazeIO.LoadMazeFromFile(_mazeMapFilePath);
            StartPlace = MazeIO.GetStartPlaceFromFile(_mazeMapFilePath);
            ExitPlace = MazeIO.GetExitPlaceFromFile(_mazeMapFilePath);

            Tester = new MazePathSolutionTester(Maze, StartPlace, ExitPlace);

            Finder = new MazePathFinder(Maze);

            Solution = Finder.GetCellsPath(StartPlace, ExitPlace);

            if (Tester.RunSolutionTest(Solution))
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                PrintMazeMap(Maze.Height, Maze.Width, Maze.MazeCells);
                for (var i = 0; i < Solution.Count - 1; i++)
                {
                    PrintSolutionPath(Solution[i], Solution[i + 1]);
                }
                PrintStartFinishLabels(StartPlace, ExitPlace);
            }
            else
            {
                return false;
            }
            return true;
        }
        //'>''<''↓''↑''|'
        public void PrintSolutionPath(MazeCell current, MazeCell next)
        {
            //выводим A, меняем цвет
            Console.SetCursorPosition(current.X, current.Y);
            Console.ForegroundColor = ConsoleColor.Green;
            if (next.X > current.X)
            {
                Console.Write('>');
            }
            else if (next.X < current.X)
            {
                Console.Write('<');
            }
            else if (next.Y > current.Y)
            {
                Console.Write('↓');
            }
            else if (next.Y < current.Y)
            {
                Console.Write('↑');
            }

        }

        private void PrintMazeMap(int height, int width, MazeCell[,] map)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Console.Write(CellTypeToChar(map[i, j]));
                }

                Console.WriteLine();
            }
        }

        private void PrintStartFinishLabels(MazeCell alphaPlace, MazeCell finishPlace)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(alphaPlace.X, alphaPlace.Y);
            Console.Write('S');
            Console.SetCursorPosition(finishPlace.X, finishPlace.Y);
            Console.Write('X');
            Console.SetCursorPosition(0, Maze.Height + 3);
        }

        private char CellTypeToChar(MazeCell cell)
        {
            switch (cell.CellType)
            {
                case CellType.None: return ' ';
                case CellType.Wall: return '#';
                default: return ' ';
            }
        }
    }
}
