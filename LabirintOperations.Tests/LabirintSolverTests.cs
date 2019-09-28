using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LabirintOperations.Tests
{
    [TestClass]
    public class LabirintSolverTests
    {
        [TestMethod]
        public void GetLabirintSolutionTest()
        {
            //arrange

            //act

            //assert

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
            var actualInRange = solver.InRange(new MapPlace(startX+1, startY),ref map);
            //assert
            Assert.AreEqual(expectedInRange,actualInRange);
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
            var actualInRange = solver.InRange(new MapPlace(startX, startY+1),ref map);
            //assert
            Assert.AreEqual(expectedInRange,actualInRange);
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
            var actualInRangeX = solver.InRange(new MapPlace(startX+5, startY),ref map);
            var actualInRangeY = solver.InRange(new MapPlace(startX, startY+5),ref map);
            //assert
            Assert.AreEqual(expectedInRange,actualInRangeX);
            Assert.AreEqual(expectedInRange,actualInRangeY);
        }
    }
}
