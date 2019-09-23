using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabirintOperations.Tests
{
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
            var expectedStartPlace = new InLabirintPlace(2, 4);
            var expectedExitPlace = new InLabirintPlace(8, 1);
            var actualStart = new InLabirintPlace();
            var actualExit = new InLabirintPlace();

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
    }
}
