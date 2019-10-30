using System;
using System.Collections.Generic;
using System.IO;
using MazeOperations;

namespace MazeAmazing_ConsoleApp
{
    public class Program
    {
        private static string labirintFile = @"labirintDebug.txt";
        private static string solutionFile = @"solutionDebug.txt";



        public MazePathSolutionTester Tester { get; set; }
        public MazePathFinder Solver { get; set; }

        public Maze Maze { get; set; }

        public List<MazeCell> Solution { get; set; }

        public MazeCell StartPlace { get; set; }
        public MazeCell ExitPlace { get; set; }

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                labirintFile = args[0];
                if (args.Length == 2)
                    solutionFile = args[1];
                if (Path.GetExtension(labirintFile) != ".txt")
                    throw new Exception("Файл данных не существует или его расширение неверно!");
                if (Path.GetExtension(solutionFile) != ".txt")
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
            Maze = MazeIO.LoadLabirint(labirintFile);
            StartPlace = MazeIO.GetStartPlace(labirintFile);
            ExitPlace = MazeIO.GetExitPlace(labirintFile);

            Tester = new MazePathSolutionTester(Maze, StartPlace, ExitPlace);

            Solver = new MazePathFinder(Maze);

            Solution = Solver.GetCellsPath(StartPlace, ExitPlace);

            if (Tester.RunSolutionTest(Solution))
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                PrintLabirint(Maze.Height, Maze.Width, Maze.MazeCells);
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

        private void PrintLabirint(int height, int width, MazeCell[,] map)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Console.Write(MazeIO.CellTypeToChar(map[i, j]));
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
    }
}
