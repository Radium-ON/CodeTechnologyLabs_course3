using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LabirintOperations.Tests
{
    public class ConsoleOutput : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOuput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }

    [TestClass]
    public class LabirintTesterTests
    {
        [TestMethod]
        public void LoadLabirintTest()
        {
            //arrange
            const string labirintFilePath = @"D:\ia_no\Desktop\labirint3.txt";
            const int baseHeight = 8;
            const int baseWidth = 10;
            var expectedStartPlace = new MapPlace(2, 4);
            var expectedExitPlace = new MapPlace(8, 1);
            var actualStart = new MapPlace();
            var actualExit = new MapPlace();

            var expectedMap = new char[,]
            {
                {'#','#','#','#','#','#','#','#','#','#'},
                {'#',' ',' ',' ',' ','#',' ',' ',' ','#'},
                {'#','#','#',' ',' ',' ',' ',' ',' ','#'},
                {'#','#','#',' ',' ','#',' ',' ',' ','#'},
                {'#',' ',' ',' ','#','#','#',' ','#','#'},
                {'#','#','#',' ','#','#','#',' ','#','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ','#','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };

            //act
            var tester = new LabirintTester();
            var actualMap = tester.LoadLabirint(labirintFilePath, out var height, out var width,
                ref actualStart, ref actualExit);

            //assert
            Assert.AreEqual(baseWidth, width, "Неверная ширина лабиринта");
            Assert.AreEqual(baseHeight, height, "Неверная высота лабиринта");
            Assert.AreEqual(expectedStartPlace, actualStart,
                "Координаты начала неверны\n" + $"Actual: x={actualStart.X} y={actualStart.Y}\n" +
                      $"Expected: x={expectedStartPlace.X} y={expectedStartPlace.Y}");
            Assert.AreEqual(expectedExitPlace, actualExit,
                "Координаты выхода неверны\n" + $"Actual: x={actualExit.X} y={actualExit.Y}\n" +
                      $"Expected: x={expectedExitPlace.X} y={expectedExitPlace.Y}");
            CollectionAssert.AreEqual(expectedMap, actualMap, "Матрицы не равны");

        }

        [DataTestMethod]
        #region DataRows
        [DataRow(new char[] { '2', '2', '6', '6', '8', '8', '4', '4' },
                 new char[] { '2', '6', '6', '8', '8', '4', '4', '2' },
                 new int[] { 0, 0, 1, 2, 3, 3, 2, 1, 0 },
                 new int[] { 0, 1, 1, 1, 1, 0, 0, 0, 0 })]
        #endregion
        public void PrintSolutionPathTest(char[] direction, char[] next, int[] startX, int[] startY)
        {
            //arrange
            var expectedPath = "|↓—>|↑—<";//путь в одну строку
            var currentConsoleOut = Console.Out;
            //act
            var tester = new LabirintTester();
            using (var consoleOutput = new ConsoleOutput())
            {
                for (var i = 0; i < direction.Length; i++)
                {
                    tester.PrintSolutionPath(direction[i], next[i], startX[i], startY[i]);
                }

                var actualConsole = consoleOutput.GetOuput();
                //assert
                Assert.AreEqual(expectedPath, actualConsole, "\nExpect:\n" +
                             $"{expectedPath}\n" + "Actual:\n" + $"{actualConsole}");
            }
        }

        [TestMethod]
        public void CountMapItemsTest()
        {
            //arrange
            var c = 'A';
            var map = new char[,]
            {
                {'#','#','#','#','#','#','#','#','#','#'},
                {'#',' ',' ',' ',' ','#','A',' ',' ','#'},
                {'#','#','#',' ',' ',' ',' ',' ',' ','#'},
                {'#','#','#','A',' ','#',' ',' ',' ','#'},
                {'#',' ',' ',' ','#','#','#','A','#','#'},
                {'#','#','#',' ','#','#','#',' ','#','#'},
                {'#',' ',' ','A',' ',' ',' ',' ','#','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };
            var expectedCountA = 4;
            //act
            var tester = new LabirintTester();
            var actualCountA = tester.CountMapItems(c, map);
            //assert
            Assert.AreEqual(expectedCountA, actualCountA);
        }

        //[TestMethod]
        //public void MoveDirectBySolutionTest()
        //{
        //    //arrange
        //    var solX = new int[] { 0, 0, 1, -1 };
        //    var solY = new int[] { -1, 1, 0, 0 };
        //    int startX = 1, startY = 1;
        //    int mapHeight = 3, mapWidth = 4;
        //    var map = new char[,]
        //    {
        //        {'#','#','#','#'},
        //        {'#','A','.','#'},
        //        {'#','#','#','#'},
        //    };
        //    var expectedBool = new bool[] { false, true, false, false }; //двигаться влево, вверх, вниз нельзя, вправо можно
        //    //act
        //    var tester = new LabirintTester();
        //    var actualBool = new bool[4];
        //    for (var i = 0; i < solX.Length; i++)
        //    {
        //        actualBool[i] = tester.MoveDirectBySolution(solX[i], solY[i], ref startX, ref startY, ref mapHeight,
        //            ref mapWidth, ref map);// можно ли двигаться

        //    }
        //    //assert
        //    CollectionAssert.AreEqual(expectedBool, actualBool);

        //}

        [TestMethod]
        public void IsLevelCorrectTest()
        {
            //arrange
            const string labirintFilePath = @"D:\ia_no\Desktop\labirint3.txt";
            var start = new MapPlace();
            var exit = new MapPlace();
            var tester = new LabirintTester();
            var map = tester.LoadLabirint(labirintFilePath, out var height, out var width, ref start, ref exit);
            var map_assert2 = new char[,]
            {
                {' ',' '},
            };
            var map_assert3 = new char[,]
            {
                {'#','#','#','#','#','#','#','#','#','#'},
                {'#',' ',' ',' ',' ','#',' ',' ',' ','#'},
                {'#','#','#',' ',' ',' ',' ',' ',' ','#'},
                {'#','#','#',' ',' ','#',' ',' ',' ','#'},
                {'#',' ',' ',' ','#','#','#',' ','#','#'},
                {'#','#','#',' ','#','#','#',' ','#','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ','#','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };
            var expected_level_correct = "";
            var expected_wall_on_map = "Уровень пуст!";
            var expected_no_start = "На карте должен быть хотя бы один объект игрока!";
            var expected_no_exit = "На карте должна быть одна цель!";
            //act
            var actual_level_correct = tester.IsLevelCorrect(start, exit, map);
            var actual_wall_on_map = tester.IsLevelCorrect(start, exit, map_assert2);
            var actual_no_start = tester.IsLevelCorrect(new MapPlace(0,0), exit, map);
            var actual_no_exit = tester.IsLevelCorrect(start, new MapPlace(0,0), map);
            //assert
            Assert.AreEqual(expected_level_correct,actual_level_correct);
            Assert.AreEqual(expected_wall_on_map,actual_wall_on_map);
            Assert.AreEqual(expected_no_start,actual_no_start);
            Assert.AreEqual(expected_no_exit,actual_no_exit);
        }

        [TestMethod]
        public void RunSolutionTest_Solution_true()
        {
            //arrange
            var start = new MapPlace(2, 4);
            var exit = new MapPlace(8, 1);
            var tester = new LabirintTester(start,exit);
            var solver = new LabirintSolver();
            //act

            //assert

        }
    }
}
