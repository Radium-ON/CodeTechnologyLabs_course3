using Prism.Mvvm;
using System.Collections.Generic;
using MazeOperations;

namespace MazeAmazing_WPF.ViewModels
{
    public class TaskOnePageViewModel : BindableBase
    {

        public TaskOnePageViewModel()
        {
            //var path =
            //    @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint4.txt";
            var path =
                @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeAmazing_ConsoleApp\bin\Debug\labirintDebug.txt";
            Maze = MazeIO.LoadMazeFromFile(path);
            var finder = new MazePathFinder(Maze);
            var start = MazeIO.GetStartPlaceFromFile(path);
            var exit = MazeIO.GetExitPlaceFromFile(path);
            SolutionList = finder.GetCellsPath(start, exit);
        }

        #region Backing Fields
        private Maze _maze;
        private List<MazeCell> _solutionList;

        #endregion

        public Maze Maze
        {
            get => _maze;
            set => SetProperty(ref _maze, value);
        }

        public List<MazeCell> SolutionList
        {
            get => _solutionList;
            set => SetProperty(ref _solutionList, value);
        }
    }
}
