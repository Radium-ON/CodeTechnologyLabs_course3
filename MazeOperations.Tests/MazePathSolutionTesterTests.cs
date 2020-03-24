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
    //public class ConsoleOutput : IDisposable
    //{
    //    private readonly StringWriter _stringWriter;
    //    private readonly TextWriter _originalOutput;

    //    public ConsoleOutput()
    //    {
    //        _stringWriter = new StringWriter();
    //        _originalOutput = Console.Out;
    //        Console.SetOut(_stringWriter);
    //    }

    //    public string GetOuput()
    //    {
    //        return _stringWriter.ToString();
    //    }

    //    public void Dispose()
    //    {
    //        Console.SetOut(_originalOutput);
    //        _stringWriter.Dispose();
    //    }
    //}

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
            var map = io.LoadMazeFromFile();
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
