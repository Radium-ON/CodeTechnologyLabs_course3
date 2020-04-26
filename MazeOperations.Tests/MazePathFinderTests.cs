using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeOperations;
using static MazeOperations.MazePathFinder;

namespace MazeOperations.Tests
{
    [TestClass]
    public class MazePathFinderTests
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
        public void GetLabirintSolutionTest()
        {
            //arrange
            var labirintPath = @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\GetMazeSolutionTextNew.txt";
            var io = new MazeIO(labirintPath);
            var map = io.CreateMazeMatrix();
            var startPlace = map.StartCellPosition;
            var exitPlace = map.ExitCellPosition;

            var expectedSolution = new List<MazeCell>
            {
                new MazeCell(1,1),
                new MazeCell(2,1),
                new MazeCell(3,1),
                new MazeCell(4,1),
                new MazeCell(5,1),
                new MazeCell(5,2),
                new MazeCell(6,2),
                new MazeCell(7,2),
                new MazeCell(7,1),
            };

            //act
            var solver = new MazePathFinder(map);

            var actualSolution = solver.GetCellsPath(startPlace, exitPlace);

            //assert
            CollectionAssert.AreEqual(expectedSolution, actualSolution, new MazeCellComparer(),
                $"\nExpected:{expectedSolution.Count}\nActual:{actualSolution.Count}\n");
        }

        [TestMethod]
        public void GetLabirintSolutionTest_NoSolution()
        {
            //arrange
            var labirintPath = @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\solver_test_no_solution.txt";
            var io = new MazeIO(labirintPath);
            var map = io.CreateMazeMatrix();
            var startPlace = map.StartCellPosition;
            var exitPlace = map.ExitCellPosition;

            var expectedSolution = new List<MazeCell>();
            //act
            var solver = new MazePathFinder(map);
            //assert
            Assert.ThrowsException<SolutionNotExistException>(() => solver.GetCellsPath(startPlace, exitPlace));
        }

        [TestMethod]
        public void GetLabirintSolutionTest_Source_equal_Destination_place()
        {
            //arrange
            var labirintPath = @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirintD.txt";
            var io = new MazeIO(labirintPath);
            var map = io.CreateMazeMatrix();
            var startPlace = map.StartCellPosition;
            var expectedSolution = "Точка начала совпадает с точкой выхода";

            //act
            var solver = new MazePathFinder(map);
            var ex = Assert.ThrowsException<StartEqualsFinishException>(
                () => solver.GetCellsPath(startPlace, startPlace));

            //assert
            Assert.AreEqual(expectedSolution, ex.Message);
        }
    }
}
