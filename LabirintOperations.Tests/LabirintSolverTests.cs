using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabirintOperations.Tests
{
    [TestClass]
    public class LabirintSolverTests
    {
        [TestMethod]
        public void GetLabirintSolutionTest()
        {
            //arrange
            var labirintPath = @"D:\ia_no\Desktop\labirintD.txt";
            int height, width;
            var startPlace = new MapPlace();
            var exitPlace = new MapPlace();
            var map = new LabirintTester().LoadLabirint(labirintPath, out height, out width, ref startPlace, ref exitPlace);
            var expectedSolution = "4424448868";
            //act
            var solver = new LabirintSolver(map);
            var actualSolution = solver.GetLabirintSolution(startPlace, exitPlace);
            //assert
            Assert.AreEqual(expectedSolution, actualSolution);
        }
        
        [TestMethod]
        public void GetLabirintSolutionTest_NoSolution()
        {
            //arrange
            var labirintPath = @"D:\ia_no\Desktop\labirintD.txt";
            int height, width;
            var startPlace = new MapPlace();
            var exitPlace = new MapPlace();
            var map = new LabirintTester().LoadLabirint(labirintPath, out height, out width, ref startPlace, ref exitPlace);
            map[8, 2] = '#';
            var expectedSolution = "-";
            //act
            var solver = new LabirintSolver(map);
            var actualSolution = solver.GetLabirintSolution(startPlace, exitPlace);
            //assert
            Assert.AreEqual(expectedSolution, actualSolution);
        }
        
        [TestMethod]
        public void GetLabirintSolutionTest_Source_equal_Destination_place()
        {
            //arrange
            var labirintPath = @"D:\ia_no\Desktop\labirintD.txt";
            int height, width;
            var startPlace = new MapPlace();
            var exitPlace = new MapPlace();
            var map = new LabirintTester().LoadLabirint(labirintPath, out height, out width, ref startPlace, ref exitPlace);
            var expectedSolution = "Точка начала совпадает с точкой выхода";
            //act
            var solver = new LabirintSolver(map);
            var actualSolution = solver.GetLabirintSolution(startPlace, startPlace);
            //assert
            Assert.AreEqual(expectedSolution, actualSolution);
        }

        [TestMethod]
        public void InRangeTest_Move_right_true()
        {
            //arrange
            int startX = 1, startY = 1;
            int mapHeight = 3, mapWidth = 4;
            var map = new char[,]
            {
                {'#','#','#','#'},
                {'#','A',' ','#'},
                {'#','#','#','#'},
            };
            var expectedInRange = true;

            //act
            var solver = new LabirintSolver(map);
            var actualInRange = solver.InRange(new MapPlace(startX+1, startY), ref map);
            //assert
            Assert.AreEqual(expectedInRange, actualInRange);
        }

        [TestMethod]
        public void InRangeTest_Move_up_false()
        {
            //arrange
            int startX = 1, startY = 1;
            int mapHeight = 3, mapWidth = 4;
            var map = new char[,]
            {
                {'#','#','#','#'},
                {'#','A',' ','#'},
                {'#','#','#','#'},
            };
            var expectedInRange = false;

            //act
            var solver = new LabirintSolver(map);
            var actualInRange = solver.InRange(new MapPlace(startX, startY+1), ref map);
            //assert
            Assert.AreEqual(expectedInRange, actualInRange);
        }

        [TestMethod]
        public void InRangeTest_Move_out_of_range()
        {
            //arrange
            int startX = 1, startY = 1;
            int mapHeight = 3, mapWidth = 4;
            var map = new char[,]
            {
                {'#','#','#','#'},
                {'#','A',' ','#'},
                {'#','#','#','#'},
            };
            var expectedInRange = false;

            //act
            var solver = new LabirintSolver(map);
            var actualInRangeX = solver.InRange(new MapPlace(startX + 5, startY), ref map);
            var actualInRangeY = solver.InRange(new MapPlace(startX, startY + 5), ref map);
            //assert
            Assert.AreEqual(expectedInRange, actualInRangeX);
            Assert.AreEqual(expectedInRange, actualInRangeY);
        }
    }
}
