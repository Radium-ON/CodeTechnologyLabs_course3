using LabirintOperations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeAmazing_ConsoleApp
{
    //public class Program
    //{
    //    private static string labirintFile = @"labirintDebug.txt";
    //    private static string solutionFile = @"solutionDebug.txt";


    //    List<MazeCell> _solution;

    //    public LabirintTester Tester { get; set; }
    //    public LabirintSolver Solver { get; set; }


    //    static void Main(string[] args)
    //    {
    //        if (args.Length != 0)
    //        {
    //            labirintFile = args[0];
    //            if (args.Length == 2)
    //                solutionFile = args[1];
    //            if (Path.GetExtension(labirintFile) != ".txt")
    //                throw new Exception("Файл данных не существует или его расширение неверно!");
    //            if (Path.GetExtension(solutionFile) != ".txt")
    //                throw new Exception("Файл состояния не существует или его расширение неверно!");
    //        }
    //        var program = new Program();
    //        if (program.Run())
    //        {
    //            Console.WriteLine("Решение найдено");
    //            //File.WriteAllText(solutionFile, program._solution);
    //        }
    //        else
    //            Console.WriteLine("Решение не найдено");
    //        Console.ReadKey(true);
    //    }

    //    private bool Run()
    //    {
    //        Tester = new LabirintTester(map, TODO, TODO);

    //        Solver = new LabirintSolver(Tester.LabirintMap);

    //        _solution = Solver.GetCellsPath(Tester.StartMazeCell, Tester.ExitMazeCell);

    //        //if (Tester.RunSolutionTest(_solution))
    //        //{
    //        //    Console.OutputEncoding = System.Text.Encoding.Unicode;
    //        //    PrintLabirint(Tester.MapHeight, Tester.MapWidth, Tester.LabirintMap);
    //        //    for (var i = 0; i < _solution.Length - 1; i++)
    //        //    {
    //        //        //PrintSolutionPath(_solution[i], _solution[i + 1]);
    //        //    }
    //        //    PrintStartFinishLabels(Tester.StartMazeCell, Tester.ExitMazeCell);
    //        //    PrintSolutionText(_solution, Tester.MapHeight);
    //        //}
    //        //else
    //        //{
    //        //    return false;
    //        //}

    //        return true;
    //    }
    //    //TODO координаты точек направлений из матрицы (для отрисовки пути)
    //    public void PrintSolutionPath(char direction, char next, int startX, int startY)
    //    {
    //        //выводим A, меняем цвет
    //        Console.SetCursorPosition(startX, startY);
    //        Console.ForegroundColor = ConsoleColor.Green;
    //        switch (direction)
    //        {
    //            case '4':
    //                Console.Write(direction == next ? '—' : '<');
    //                break;
    //            case '6':
    //                Console.Write(direction == next ? '—' : '>');
    //                break;
    //            case '2':
    //                Console.Write(direction == next ? '|' : '↓');
    //                break;
    //            case '8':
    //                Console.Write(direction == next ? '|' : '↑');
    //                break;
    //        }
    //    }

    //    private void PrintLabirint(int height, int width, MazeCell[,] map)
    //    {
    //        Console.Clear();
    //        Console.SetCursorPosition(0, 0);
    //        for (var i = 0; i < height; i++)
    //        {
    //            for (var j = 0; j < width; j++)
    //            {
    //                Console.Write(map[i, j]);
    //            }

    //            Console.WriteLine();
    //        }
    //    }

    //    private void PrintStartFinishLabels(MazeCell alphaPlace, MazeCell finishPlace)
    //    {
    //        Console.ForegroundColor = ConsoleColor.Yellow;
    //        Console.SetCursorPosition(alphaPlace.X, alphaPlace.Y);
    //        Console.Write('S');
    //        Console.SetCursorPosition(finishPlace.X, finishPlace.Y);
    //        Console.Write('F');
    //    }

    //    private void PrintSolutionText(string solution, int height)
    //    {
    //        Console.ForegroundColor = ConsoleColor.Gray;
    //        Console.SetCursorPosition(0, height + 1);
    //        Console.WriteLine(solution);
    //    }
    //}
}
