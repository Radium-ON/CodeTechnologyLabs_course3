using LabirintOperations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeAmazing_WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MazeControl
    /// </summary>
    public partial class MazeControl : UserControl
    {
        public MazeControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MazeGridProperty =
            DependencyProperty.Register(
                "MazeGrid", typeof(Maze), typeof(MazeControl),
                new FrameworkPropertyMetadata()
                {
                    PropertyChangedCallback = OnMazeGridChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true
                });

        public Maze MazeGrid
        {
            get => (Maze)GetValue(MazeGridProperty);
            set => SetValue(MazeGridProperty, value);
        }

        private static void OnMazeGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                if (d is UserControl c)
                {
                    c.Content = UpdateGridInstance((Maze)e.NewValue);
                }
            }
        }

        private static Grid UpdateGridInstance(Maze maze)
        {
            var grid = new Grid();
            for (var row = 0; row < maze.Height; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                for (var column = 0; column < maze.Width; column++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var cell = maze.MazeCells[row, column];
                    var field = new Rectangle
                    {
                        Height = 40,
                        Width = 40,
                        Fill = (cell.CellType == CellType.Wall) ? Brushes.Black : Brushes.White
                    };

                    grid.Children.Add(field);

                    Grid.SetColumn(field, column);
                    Grid.SetRow(field, row);

                    grid.Name = "grid_maze_view";
                }
            }
            return grid;
        }
    }
}
