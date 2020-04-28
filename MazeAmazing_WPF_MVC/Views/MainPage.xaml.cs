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
            //TODO: Принимать сервис через параметр при запуске в App.xaml.cs
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



        private async void CreateMazeFromFilePathOpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenButtonProgressActivate();

            _cts = new CancellationTokenSource();
            _mazeIO = new MazeIO();

            try
            {
                await _mazeIO.ReadMazeFromFileTaskAsync(DialogFilePath, _cts.Token);

                //Преобразует строки файла в матрицу с ячейками, возвращает Maze
                //Свойство Maze привязано к MazeAmazing_WPF.Views.UserControls.MazeControl
                Maze = await _mazeIO.СreateMazeMatrixAsync(_cts.Token);

                StartCellPosition = Maze.StartCellPosition;
                ExitCellPosition = Maze.ExitCellPosition;
            }
            catch (OperationCanceledException x)
            {

                text_block_cancelled.Visibility = Visibility.Visible;
            }

            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                text_block_exception.Text = x.Message;
                text_block_exception.Visibility = Visibility.Visible;
            }

            OpenButtonProgressDeactivate();
        }

        private async void FindPathInMazeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenButtonProgressActivate();

            _cts = new CancellationTokenSource();

            try
            {
                var finder = new MazePathFinder(Maze);
                var startCell = Maze.StartCellPosition;
                var exitCell = Maze.ExitCellPosition;
                var solutionCellsPath = await finder.GetCellsPathAsync(startCell, exitCell, _cts.Token);

                SolutionList = solutionCellsPath;
                //чтобы dependency property обновили значение при повторном нажатии кнопок,
                //пришлось присвоить им пустую структуру.
                StartCellPosition = new MazeCell();
                ExitCellPosition = new MazeCell();
                StartCellPosition = startCell;
                ExitCellPosition = exitCell;
            }
            catch (OperationCanceledException x)
            {

                text_block_cancelled.Visibility = Visibility.Visible;
            }

            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                text_block_exception.Text = x.Message;
                text_block_exception.Visibility = Visibility.Visible;
            }

            OpenButtonProgressDeactivate();
        }

        private void CancelTaskAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
        }

        private void OpenButtonProgressDeactivate()
        {
            btn_open.IsEnabled = true;
            btn_cancel.IsEnabled = false;

            progress_ring.IsActive = false;
            border_progress.Visibility = Visibility.Collapsed;
        }

        private void OpenButtonProgressActivate()
        {
            btn_open.IsEnabled = false;
            btn_cancel.IsEnabled = true;

            text_block_cancelled.Visibility = Visibility.Collapsed;
            text_block_exception.Visibility = Visibility.Collapsed;

            border_progress.Visibility = Visibility.Visible;
            progress_ring.IsActive = true;
        }
    }
}
