using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabirintOperations;

namespace MazeAmazing_ConsoleApp
{
    public struct Place //координаты ячейки
    {
        public Place(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
    public class Program
    {
        //static string labirintFile = @"D:\ia_no\Desktop\labirint3.txt";
        //static string labirintFile = @"D:\ia_no\Desktop\labirintD.txt";
        private static string labirintFile = @"labirintDebug.txt";
        private static string solutionFile = @"solutionDebug.txt";

        Place _alphaPlace;
        Place _exitPlace;

        char[,] map;
        int width, height;
        int startX, startY;//координаты A
        int finishX, finishY;//финишные координаты
        string solution;
        int currentPosSolution;//текущих ход решения
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
            Program program = new Program();
            if (program.Run())
            {
                Console.WriteLine("Решение найдено");
                File.WriteAllText(solutionFile, program.solution);
            }
            else
                Console.WriteLine("Решение не найдено");
            Console.ReadKey(true);
        }

        private bool Run()
        {
            LoadLabirint(labirintFile);
            _alphaPlace = new Place(startX, startY);
            _exitPlace = new Place(finishX, finishY);
            var levelOK = IsLevelCorrect(_alphaPlace, _exitPlace);
            if (levelOK != "")
                throw new Exception(levelOK);
            //LoadSolution(solutionFile);
            LabirintSolver appleSolver = new LabirintSolver(map);
            solution = appleSolver.MoveDirect(_alphaPlace, _exitPlace);
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            PrintLabirint();
            for (var i = 0; i < solution.Length - 1; i++)
            {
                switch (solution[i])
                {
                    case '4': if (!MoveDirect(-1, 0)) return false; break;
                    case '6': if (!MoveDirect(1, 0)) return false; break;
                    case '2': if (!MoveDirect(0, 1)) return false; break;
                    case '8': if (!MoveDirect(0, -1)) return false; break;
                    default: return false;//решение неверно
                }
                PrintSolutionPath(solution[i], solution[i + 1]);
            }
            PrintStartFinishLabels();
            PrintSolutionText();
            return (startX == finishX && startY == finishY);
        }

        public void PrintSolutionPath(char direction, char next, int startX, int startY)
        {
            //выводим A, меняем цвет
            Console.SetCursorPosition(startX, startY);
            Console.ForegroundColor = ConsoleColor.Green;
            switch (direction)
            {
                case '4':
                    Console.Write(direction == next ? '—' : '<');
                    break;
                case '6':
                    Console.Write(direction == next ? '—' : '>');
                    break;
                case '2':
                    Console.Write(direction == next ? '|' : '↓');
                    break;
                case '8':
                    Console.Write(direction == next ? '|' : '↑');
                    break;
            }
        }

        private void PrintLabirint(int height, int width, char[,] map)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Console.Write(map[j, i]);
                }

                Console.WriteLine();
            }
        }

        private void PrintStartFinishLabels(MapPlace alphaPlace, MapPlace finishPlace)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(alphaPlace.X, alphaPlace.Y);
            Console.Write('S');
            Console.SetCursorPosition(finishPlace.X, finishPlace.Y);
            Console.Write('F');
        }

        private void PrintSolutionText(string solution, int height)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine(solution);
        }
    }
}
