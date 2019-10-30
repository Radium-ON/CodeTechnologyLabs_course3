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
            //    @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\labirint4.txt";
            var path =
                @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeAmazing_ConsoleApp\bin\Debug\labirintDebug.txt";
            MazeMap = MazeIO.LoadMazeMapFromFile(path);
            var finder = new MazePathFinder(MazeMap);
            var start = MazeIO.GetStartPlaceFromFile(path);
            var exit = MazeIO.GetExitPlaceFromFile(path);
            SolutionList = finder.GetCellsPath(start, exit);
        }

        #region Backing Fields
        private Maze _mazeMap;
        private List<MazeCell> _solutionList;

        #endregion

        public Maze MazeMap
        {
            get => _mazeMap;
            set => SetProperty(ref _mazeMap, value);
        }

        public List<MazeCell> SolutionList
        {
            get => _solutionList;
            set => SetProperty(ref _solutionList, value);
        }
    }
}
