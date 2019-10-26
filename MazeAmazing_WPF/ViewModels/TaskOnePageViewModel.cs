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
            Labirint = LabirintIO.LoadLabirint(@"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\LabirintOperations.Tests\TestInput\labirint4.txt");
        }

        #region Backing Fields
        private Maze _labirint;



        #endregion

        public Maze Labirint
        {
            get => _labirint;
            set => SetProperty(ref _labirint, value);
        }
    }
}
