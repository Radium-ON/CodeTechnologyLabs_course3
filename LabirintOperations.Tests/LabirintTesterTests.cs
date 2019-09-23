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
            var labirintFilePath = @"D:\ia_no\Desktop\labirint_test1.txt";
            var baseHeight = 3;
            var baseWidth = 4;
            //var expectedMap = new char[,]
            //{
            //    {'#','#','#','#','#','#','#','#','#','#'},
            //    {'#',' ',' ',' ',' ','#',' ',' ',' ','#'},
            //    {'#','#','#',' ',' ',' ',' ',' ',' ','#'},
            //    {'#','#','#',' ',' ','#',' ',' ',' ','#'},
            //    {'#',' ',' ',' ','#','#','#',' ','#','#'},
            //    {'#','#','#',' ','#','#','#',' ','#','#'},
            //    {'#',' ',' ',' ',' ',' ',' ',' ','#','#'},
            //    {'#','#','#','#','#','#','#','#','#','#'},
            //};

            //var expectedMap = new char[,]
            //{
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //    {'#','#','#','#','#','#','#','#'},
            //};

            var expectedMap = new char[,]
            {
                {'#', '#', '#','#'},
                {'#', ' ', ' ','#'},
                {'#', '#', '#','#'},
            };
            //var expectedMap = new char[,] //транспонированная (нелогично)
            //{
            //    {'#', '#', '#'},
            //    {'#', ' ', '#'},
            //    {'#', ' ', '#'},
            //    {'#', '#', '#'},
            //};

            //act
            var tester = new LabirintTester();
            int height;
            int width;
            var actualMap = tester.LoadLabirint(labirintFilePath, out height, out width);
            //assert
            Assert.AreEqual(baseWidth, width, "Неверная ширина лабиринта");
            Assert.AreEqual(baseHeight, height, "Неверная высота лабиринта");
            CollectionAssert.AreEqual(expectedMap, actualMap, "Матрицы не равны");

        }
    }
}
