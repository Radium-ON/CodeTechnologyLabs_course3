using Prism.Mvvm;
using System.Collections.Generic;
using MazeOperations;

namespace MazeAmazing_WPF.ViewModels
{
    public class TaskOnePageViewModel : BindableBase
    {

        public TaskOnePageViewModel()
        {
            var path =
                @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint4.txt";
            //var path =
            //    @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeAmazing_ConsoleApp\bin\Debug\labirintDebug.txt";
            //var path =
            //    @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\output.txt";
            Maze = MazeIO.LoadMazeFromFile(path);
            var finder = new MazePathFinder(Maze);
            StartCellPosition = MazeIO.GetStartPlaceFromFile(path);
            ExitCellPosition = MazeIO.GetExitPlaceFromFile(path);
            SolutionList = finder.GetCellsPath(StartCellPosition, ExitCellPosition);
        }

        #region Backing Fields
        private Maze _maze;
        private List<MazeCell> _solutionList;
        private MazeCell _startCellPosition;
        private MazeCell _exitCellPosition;

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
        
        public MazeCell StartCellPosition
        {
            get => _startCellPosition;
            set => SetProperty(ref _startCellPosition, value);
        }
        
        public MazeCell ExitCellPosition
        {
            get => _exitCellPosition;
            set => SetProperty(ref _exitCellPosition, value);
        }
    }
}
