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

namespace MazeAmazing_WPF_MVC.Views
{

    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl, INotifyPropertyChanged
    {

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
                _mazeIO = new MazeIO();
                progress_ring.IsActive = true;
                //метод работает асинхронно за счёт StreamReader
                //возвращает результат в объект MazeIO
                await _mazeIO.ReadMazeFromFileTaskAsync(DialogFilePath);
                //Преобразует строки файла в матрицу с ячейками, возвращает Maze
                //Свойство Maze привязано к MazeAmazing_WPF.Views.UserControls.MazeControl
                var msm = await _mazeIO.LoadMazeFromFileAsync();

                var finder = new MazePathFinder(msm);
                var startCell = msm.StartCellPosition;
                var exitCell = msm.ExitCellPosition;
                var solutionCellsPath = await finder.GetCellsPathAsync(startCell, exitCell);
                Maze = msm;
                SolutionList = solutionCellsPath;
                StartCellPosition = startCell;
                ExitCellPosition = exitCell;
                button.IsEnabled = true;
            }

            progress_ring.IsActive = false;
        }
    }
}
