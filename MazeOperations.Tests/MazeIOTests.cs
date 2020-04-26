using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MazeOperations;

namespace MazeOperations.Tests
{
    [TestClass]
    public class MazeIOTests
    {
        [TestMethod]
        public void LoadLabirintTest()
        {
            //arrange
            const string labirintFilePath = @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint_test1.txt";

            var expectedMap = new MazeCell[3, 4];
            expectedMap[0, 0] = new MazeCell(0, 0, CellType.Wall);
            expectedMap[0, 1] = new MazeCell(1, 0, CellType.Wall);
            expectedMap[0, 2] = new MazeCell(2, 0, CellType.Wall);
            expectedMap[0, 3] = new MazeCell(3, 0, CellType.Wall);
            expectedMap[1, 0] = new MazeCell(0, 1, CellType.Wall);
            expectedMap[1, 1] = new MazeCell(1, 1, CellType.Start);
            expectedMap[1, 2] = new MazeCell(2, 1, CellType.Exit);
            expectedMap[1, 3] = new MazeCell(3, 1, CellType.Wall);
            expectedMap[2, 0] = new MazeCell(0, 2, CellType.Wall);
            expectedMap[2, 1] = new MazeCell(1, 2, CellType.Wall);
            expectedMap[2, 2] = new MazeCell(2, 2, CellType.Wall);
            expectedMap[2, 3] = new MazeCell(3, 2, CellType.Wall);

            //act
            var io = new MazeIO(labirintFilePath);
            var actualMap = io.CreateMazeMatrix().MazeCells;

            //assert

            CollectionAssert.AreEqual(expectedMap, actualMap, "Матрицы не равны");

        }

        [TestMethod]
        public void LoadLabirint_NoLinesInFile()
        {
            //arrange
            var labirintPath = @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\empty_maze.txt";

            var expectedSolution = "Файл не содержит ни одной строки";
            //act
            var io = new MazeIO(labirintPath);
            var ex = Assert.ThrowsException<EmptyDataFileException>(
                () => io.CreateMazeMatrix());

            //assert
            Assert.AreEqual(expectedSolution, ex.Message);
        }
    }
}
