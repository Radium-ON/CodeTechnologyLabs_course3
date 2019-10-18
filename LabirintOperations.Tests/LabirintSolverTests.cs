using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LabirintOperations.LabirintSolver;

namespace LabirintOperations.Tests
{
    public static class MSTestExtension
    {
        public delegate bool CompareFunc<in T>(T obj1, T obj2);

        private class LambdaComparer<T> : IComparer
        {
            private readonly CompareFunc<T> _compareFunc;

            public LambdaComparer(CompareFunc<T> compareFunc)
            {
                _compareFunc = compareFunc;
            }

            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (!(x is T t1) || !(y is T t2))
                {
                    return -1;
                }

                return _compareFunc(t1, t2) ? 0 : 1;
            }
        }
        public static void AreEqual<T>(this Assert assert, T expected, T actual, CompareFunc<T> compareFunc)
        {
            var comparer = new LambdaComparer<T>(compareFunc);

            CollectionAssert.AreEqual(
                new[] { expected },
                new[] { actual }, comparer,
                $"\nExpected: <{expected}>.\nActual: <{actual}>.");
        }
    }

    [TestClass]
    public class LabirintSolverTests
    {
        private class MazeCellComparer : Comparer<MazeCell>
        {
            public override int Compare(MazeCell x, MazeCell y)
            {
                // compare the two mountains
                // for the purpose of this tests they are considered equal when their identifiers (names) match
                var comp = x.X.CompareTo(y.X);
                return comp != 0 ? comp : x.Y.CompareTo(y.Y);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetLabirintSolutionTest()
        {
            //arrange
            var labirintPath = @"D:\ia_no\Desktop\GetMazeSolutionTextNew.txt";
            var labirintPath1 = @"D:\ia_no\Desktop\labirint3.txt";
            var labirint_out_of_range = @"D:\ia_no\Desktop\test_outOfRange.txt";
            var tester = new LabirintTester(labirintPath);
            var tester1 = new LabirintTester(labirintPath1);
            var tester2 = new LabirintTester(labirint_out_of_range);
            var expectedSolution = new List<MazeCell>
            {
                new MazeCell(2,1),
                new MazeCell(3,1),
                new MazeCell(4,1),
                new MazeCell(5,1),
                new MazeCell(5,2),
                new MazeCell(6,2),
                new MazeCell(7,2),
                new MazeCell(7,1),
            };
            var expectedSolution1 = new List<MazeCell>
            {
                new MazeCell(2,4),
                new MazeCell(3,4),
                new MazeCell(3,3),
                new MazeCell(4,3),
                new MazeCell(4,2),
                new MazeCell(5,2),
                new MazeCell(6,2),
                new MazeCell(7,2),
                new MazeCell(8,2),
                new MazeCell(8,1),
            };
            var expectedSolutionOor = new List<MazeCell>();
            //act
            var solver = new LabirintSolver(tester.LabirintMap);
            var solver1 = new LabirintSolver(tester1.LabirintMap);
            var solver2 = new LabirintSolver(tester2.LabirintMap);
            var actualSolution = solver.GetCellsPath(tester.StartMazeCell, tester.ExitMazeCell);
            var actualSolution1 = solver1.GetCellsPath(tester1.StartMazeCell, tester1.ExitMazeCell);
            var actualSolution2 = solver2.GetCellsPath(tester2.StartMazeCell, tester2.ExitMazeCell);
            //assert
            CollectionAssert.AreEqual(expectedSolution, actualSolution, new MazeCellComparer(),
                $"\nExpected:{expectedSolution.Count}\nActual:{actualSolution.Count}\n");
            CollectionAssert.AreEqual(expectedSolution1, actualSolution1, new MazeCellComparer(),
                $"\nExpected:{expectedSolution1.Count}\nActual:{actualSolution1.Count}\n");
            CollectionAssert.AreEqual(expectedSolutionOor, actualSolution2, new MazeCellComparer(),
                $"\nExpected:{expectedSolutionOor.Count}\nActual:{actualSolution2.Count}\n");
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetLabirintSolutionTest_NoSolution()
        {
            //arrange
            var labirintPath = @"D:\ia_no\Desktop\solver_test_no_solution.txt";

            var tester = new LabirintTester(labirintPath);
            var expectedSolution = new List<MazeCell>();
            //act
            var solver = new LabirintSolver(tester.LabirintMap);
            var actualSolution = solver.GetCellsPath(tester.StartMazeCell, tester.ExitMazeCell);
            //assert
            CollectionAssert.AreEqual(expectedSolution, actualSolution);
        }

        [TestMethod]
        public void TraverseTest()
        {
            //arrange
            var root = new Chain(
                new MazeCell(0, 0), next: new Chain(
                    new MazeCell(1, 1), next: new Chain(
                        new MazeCell(2, 2), next: new Chain(
                            new MazeCell(3, 3)))));

            var expectedPath = new List<MazeCell>()
            {
                new MazeCell(0,0),
                new MazeCell(1,1),
                new MazeCell(2,2),
                new MazeCell(3,3)
            };
            //act
            var actualPath = Chain.Traverse(root);
            //assert
            CollectionAssert.AreEqual(expectedPath, actualPath, new MazeCellComparer(),
                $"\nExpected:{expectedPath.Count}\nActual:{actualPath.Count}\n");
        }
        [TestMethod]
        public void TraverseTest_return_empty_path()
        {
            //arrange
            Chain root = null;

            var expectedPath = new List<MazeCell>();
            //act
            var actualPath = Chain.Traverse(root);
            //assert
            CollectionAssert.AreEqual(expectedPath, actualPath, new MazeCellComparer(),
                $"\nExpected:{expectedPath.Count}\nActual:{actualPath.Count}\n");
        }

        [TestMethod]
        public void GetLabirintSolutionTest_Source_equal_Destination_place()
        {
            //arrange
            var labirintPath = @"D:\ia_no\Desktop\labirintD.txt";
            var tester = new LabirintTester(labirintPath);
            var expectedSolution = "Точка начала совпадает с точкой выхода";

            //act
            var solver = new LabirintSolver(tester.LabirintMap);
            var ex = Assert.ThrowsException<Exception>(
                () => solver.GetCellsPath(tester.StartMazeCell, tester.StartMazeCell));

            //assert
            Assert.AreEqual(expectedSolution, ex.Message);
        }

        //[TestMethod]
        //public void InRangeTest_Move_right_true()
        //{
        //    //arrange
        //    int startX = 1, startY = 1;
        //    int mapHeight = 3, mapWidth = 4;
        //    var map = new char[,]
        //    {
        //        {'#','#','#','#'},
        //        {'#','A',' ','#'},
        //        {'#','#','#','#'},
        //    };
        //    var expectedInRange = true;

        //    //act
        //    var solver = new LabirintSolver(map);
        //    var actualInRange = solver.InRange(new MazeCell(startX+1, startY), ref map);
        //    //assert
        //    Assert.AreEqual(expectedInRange, actualInRange);
        //}

        //[TestMethod]
        //public void InRangeTest_Move_up_false()
        //{
        //    //arrange
        //    int startX = 1, startY = 1;
        //    int mapHeight = 3, mapWidth = 4;
        //    var map = new char[,]
        //    {
        //        {'#','#','#','#'},
        //        {'#','A',' ','#'},
        //        {'#','#','#','#'},
        //    };
        //    var expectedInRange = false;

        //    //act
        //    var solver = new LabirintSolver(map);
        //    var actualInRange = solver.InRange(new MazeCell(startX, startY+1), ref map);
        //    //assert
        //    Assert.AreEqual(expectedInRange, actualInRange);
        //}

        //[TestMethod]
        //public void InRangeTest_Move_out_of_range()
        //{
        //    //arrange
        //    int startX = 1, startY = 1;
        //    int mapHeight = 3, mapWidth = 4;
        //    var map = new char[,]
        //    {
        //        {'#','#','#','#'},
        //        {'#','A',' ','#'},
        //        {'#','#','#','#'},
        //    };
        //    var expectedInRange = false;

        //    //act
        //    var solver = new LabirintSolver(map);
        //    var actualInRangeX = solver.InRange(new MazeCell(startX + 5, startY), ref map);
        //    var actualInRangeY = solver.InRange(new MazeCell(startX, startY + 5), ref map);
        //    //assert
        //    Assert.AreEqual(expectedInRange, actualInRangeX);
        //    Assert.AreEqual(expectedInRange, actualInRangeY);
        //}
    }
}
