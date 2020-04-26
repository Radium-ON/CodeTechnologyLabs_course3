using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MazeOperations;

namespace MazeOperations.Tests
{
    [TestClass]
    public class MazePathSolutionTesterTests
    {
        [DataTestMethod]
        #region DataRows
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint4.txt", true)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\GetMazeSolutionTextNew.txt", true)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint3.txt", true)]
        //[DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint5.txt", false)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\output.txt", true)]
        //[DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\test_outOfRange.txt", false)]
        #endregion
        public void RunSolutionTest_Solution_true(string labirintFilePath, bool correct)
        {
            //arrange
            var expectedPassed = correct;
            var io = new MazeIO(labirintFilePath);
            var map = io.CreateMazeMatrix();
            var startPlace = map.StartCellPosition;
            var exitPlace = map.ExitCellPosition;
            //act
            var solver = new MazePathFinder(map);
            var tester = new MazePathSolutionTester(map, startPlace, exitPlace);
            var solution = solver.GetCellsPath(startPlace, exitPlace);
            var actualPassed = tester.RunSolutionTest(solution);
            //assert
            Assert.AreEqual(expectedPassed, actualPassed);
        }
    }
}
