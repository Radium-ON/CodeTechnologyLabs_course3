using LabirintOperations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeAmazing_ConsoleApp
{
    public class Program
    {
        private static string labirintFile = @"labirintDebug.txt";
        private static string solutionFile = @"solutionDebug.txt";



        public LabirintTester Tester { get; set; }
        public LabirintSolver Solver { get; set; }

        public MazeCell[,] MazeMap { get; set; }

        public int MazeHeight => MazeMap.GetLength(0);

        public int MazeWidth => MazeMap.GetLength(1);

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
            MazeMap = LabirintIO.LoadLabirint(labirintFile);
            StartPlace = LabirintIO.GetStartPlace(labirintFile);
            ExitPlace = LabirintIO.GetExitPlace(labirintFile);

            Tester = new LabirintTester(MazeMap, StartPlace, ExitPlace);

            Solver = new LabirintSolver(MazeMap);

            Solution = Solver.GetCellsPath(StartPlace, ExitPlace);

            if (Tester.RunSolutionTest(Solution))
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                PrintLabirint(MazeHeight, MazeWidth, MazeMap);
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
                    Console.Write(LabirintIO.CellTypeToChar(map[i, j]));
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
            Console.SetCursorPosition(0, MazeHeight + 3);
        }
    }
}
