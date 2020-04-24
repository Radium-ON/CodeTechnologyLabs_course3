using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using MazeOperations;
using Services;
using OperationCanceledException = System.OperationCanceledException;

namespace MazeAmazing_WPF_MVC.Views
{

    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl, INotifyPropertyChanged
    {
        private CancellationTokenSource _cts;
        private string _dialogFilePath;
        private IDialogService _dialogService;
        private MazeIO _mazeIO;
        private Maze _maze;
        private List<MazeCell> _solutionList;
        private MazeCell _startCellPosition;
        private MazeCell _exitCellPosition;

        public MainPage()
        {
            InitializeComponent();
            main_grid.DataContext = this;
            _dialogService = new DefaultDialogService();

            DialogFilePath =
                @"C:\Users\ia_no\Source\Repos\CodeTechnologyLabs_course3\MazeOperations.Tests\TestInput\output.txt";
        }

        public string DialogFilePath
        {
            get { return _dialogFilePath; }
            set
            {
                _dialogFilePath = value;
                OnPropertyChanged("DialogFilePath");
            }
        }

        public Maze Maze
        {
            get => _maze;
            set
            {
                _maze = value;
                OnPropertyChanged("Maze");
            }
        }

        public List<MazeCell> SolutionList
        {
            get => _solutionList;
            set
            {
                _solutionList = value;
                OnPropertyChanged("SolutionList");
            }
        }

        public MazeCell StartCellPosition
        {
            get => _startCellPosition;
            set
            {
                _startCellPosition = value;
                OnPropertyChanged("StartCellPosition");
            }
        }

        public MazeCell ExitCellPosition
        {
            get => _exitCellPosition;
            set
            {
                _exitCellPosition = value;
                OnPropertyChanged("ExitCellPosition");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        /// <summary>
        /// Ожидается:
        /// Кнопка гаснет, появляется progress_ring, идёт чтение файла, потом создание объекта Maze.
        /// Свойство Maze обновляет MazeControl новым лабиринтом, кнопка доступна, прогресс скрыт.
        /// Фактически: UI поток блокируется на LoadMazeFromFileAsync.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenFromPathButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                button.IsEnabled = false;
                text_block_cancelled.Visibility = Visibility.Collapsed;
                border_progress.Visibility = Visibility.Visible;
                progress_ring.IsActive = true;

                _cts = new CancellationTokenSource();
                _mazeIO = new MazeIO();

                //метод работает асинхронно за счёт StreamReader
                //возвращает результат в объект MazeIO
                try
                {
                    await _mazeIO.ReadMazeFromFileTaskAsync(DialogFilePath, _cts.Token);
                    //Преобразует строки файла в матрицу с ячейками, возвращает Maze
                    //Свойство Maze привязано к MazeAmazing_WPF.Views.UserControls.MazeControl

                    var maze = await _mazeIO.LoadMazeFromFileAsync(_cts.Token);

                    var finder = new MazePathFinder(maze);
                    var startCell = maze.StartCellPosition;
                    var exitCell = maze.ExitCellPosition;
                    var solutionCellsPath = await finder.GetCellsPathAsync(startCell, exitCell, _cts.Token);

                    Maze = maze;
                    SolutionList = solutionCellsPath;
                    StartCellPosition = startCell;
                    ExitCellPosition = exitCell;
                }
                catch (OperationCanceledException)
                {
                    text_block_cancelled.Visibility = Visibility.Visible;
                }
                button.IsEnabled = true;
            }

            progress_ring.IsActive = false;
            border_progress.Visibility = Visibility.Collapsed;
        }

        private void CancelTaskAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
        }
    }
}
