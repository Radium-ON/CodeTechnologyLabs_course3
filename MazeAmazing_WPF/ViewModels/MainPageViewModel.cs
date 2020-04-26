using System;
using Prism.Mvvm;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using FirstFloor.ModernUI.Presentation;
using MazeOperations;
using Prism.Commands;
using Services;

namespace MazeAmazing_WPF.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public DelegateCommand OpenDialogCommand { get; private set; }
        public DelegateCommand OpenFromPathCommand { get; private set;}


        public MainPageViewModel()
        {
            _dialogService = new DefaultDialogService();
            
            OpenDialogCommand = new DelegateCommand(OpenMazeFileDialog);
            OpenFromPathCommand = new DelegateCommand(OpenFileByPath, CanOpenFileByPath)
                .ObservesProperty(() => DialogFilePath);

            DialogFilePath =
                @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\labirint4.txt";
            InfluteMazeControl(DialogFilePath);

        }

        private bool CanOpenFileByPath()
        {
            return !string.IsNullOrEmpty(DialogFilePath);
        }

        private void OpenFileByPath()
        {
            InfluteMazeControl(DialogFilePath);
        }


        private void OpenMazeFileDialog()
        {
            if (_dialogService.OpenFileDialog())
            {
                DialogFilePath = _dialogService.FilePath;
                InfluteMazeControl(DialogFilePath);
            }
        }

        private void InfluteMazeControl(string dialogFilePath)
        {
            MazeIO = new MazeIO(dialogFilePath);
            Maze = MazeIO.CreateMazeMatrix();
            var finder = new MazePathFinder(Maze);
            var startCell = Maze.StartCellPosition;
            var exitCell = Maze.ExitCellPosition;
            SolutionList = finder.GetCellsPath(startCell, exitCell);
            StartCellPosition = startCell;
            ExitCellPosition = exitCell;
        }

        #region Backing Fields

        private Maze _maze;
        private List<MazeCell> _solutionList;
        private MazeCell _startCellPosition;
        private MazeCell _exitCellPosition;
        private readonly IDialogService _dialogService;
        private string _dialogFilePath;

        #endregion

        public Maze Maze
        {
            get => _maze;
            set => SetProperty(ref _maze, value);
        }

        private MazeIO MazeIO { get; set; }

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

        public string DialogFilePath
        {
            get => _dialogFilePath;
            set => SetProperty(ref _dialogFilePath, value);
        }
    }
}