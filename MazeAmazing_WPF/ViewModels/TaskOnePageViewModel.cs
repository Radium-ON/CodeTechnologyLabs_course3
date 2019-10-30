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
            Labirint = MazeIO.LoadLabirint(path);
            var solver = new MazePathFinder(Labirint);
            var start = MazeIO.GetStartPlace(path);
            var exit = MazeIO.GetExitPlace(path);
            SolutionList = solver.GetCellsPath(start, exit);
        }

        #region Backing Fields
        private Maze _labirint;
        private List<MazeCell> _solutionList;

        #endregion

        public Maze Labirint
        {
            get => _labirint;
            set => SetProperty(ref _labirint, value);
        }

        public List<MazeCell> SolutionList
        {
            get => _solutionList;
            set => SetProperty(ref _solutionList, value);
        }
    }
}
