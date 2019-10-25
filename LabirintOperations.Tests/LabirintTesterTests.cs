﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LabirintOperations.Tests
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
    public class LabirintTesterTests
    {
        [DataTestMethod]
        #region DataRows
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\labirint4.txt", true)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\GetMazeSolutionTextNew.txt", true)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\labirint3.txt", true)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\labirint5.txt", false)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\output.txt", true)]
        [DataRow(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\test_outOfRange.txt", false)]
        #endregion
        public void RunSolutionTest_Solution_true(string labirintFilePath, bool correct)
        {
            //arrange
            var expectedPassed = correct;
            var map = LabirintIO.LoadLabirint(labirintFilePath);
            var startPlace = LabirintIO.GetStartPlace(labirintFilePath);
            var exitPlace = LabirintIO.GetExitPlace(labirintFilePath);
            //act
            var solver = new LabirintSolver(map.MazeCells);
            var tester = new LabirintTester(map.MazeCells, startPlace, exitPlace);
            var solution = solver.GetCellsPath(startPlace, exitPlace);
            var actualPassed = tester.RunSolutionTest(solution);
            //assert
            Assert.AreEqual(expectedPassed, actualPassed);
        }
    }
}
