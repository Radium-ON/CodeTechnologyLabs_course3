using System;
using Prism.Mvvm;
using System.Collections.Generic;
using System.IO;
using FirstFloor.ModernUI.Presentation;
using MazeOperations;
using Services;

namespace MazeAmazing_WPF.ViewModels
{
    public class TaskOnePageViewModel : BindableBase
    {
        public RelayCommand OpenCommand { get; private set; }

        public TaskOnePageViewModel()
        {
            _dialogService = new DefaultDialogService();
            
            OpenCommand = new RelayCommand(o =>
            {
                if (_dialogService.OpenFileDialog())
                {
                    DialogFilePath = _dialogService.FilePath;
                    MazeIO = new MazeIO(DialogFilePath);
                    Maze = MazeIO.LoadMazeFromFile();
                    var finder = new MazePathFinder(Maze);
                    StartCellPosition = MazeIO.GetStartPlaceFromFile();
                    ExitCellPosition = MazeIO.GetExitPlaceFromFile();
                    SolutionList = finder.GetCellsPath(StartCellPosition, ExitCellPosition);

                    _dialogService.ShowMessage($"Лабиринт {Path.GetFileNameWithoutExtension(_dialogService.FilePath)} открыт.");
                }
            });
        }

        #region Backing Fields

        private Maze _maze;
        private List<MazeCell> _solutionList;
        private MazeCell _startCellPosition;
        private MazeCell _exitCellPosition;
        private IDialogService _dialogService;
        private RelayCommand _openCommand;
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