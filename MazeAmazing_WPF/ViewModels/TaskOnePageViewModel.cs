using LabirintOperations;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Labirint = LabirintIO.LoadLabirint(path);
            var solver = new LabirintSolver(Labirint);
            var start = LabirintIO.GetStartPlace(path);
            var exit = LabirintIO.GetExitPlace(path);
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
