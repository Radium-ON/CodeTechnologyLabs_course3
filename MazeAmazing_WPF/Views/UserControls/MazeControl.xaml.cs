using MazeOperations;
using System.Collections.Generic;
using System.Linq;
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
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = OnMazeGridChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true
                });

        public static readonly DependencyProperty SolutionPathListProperty =
            DependencyProperty.Register(
                "SolutionPathList", typeof(List<MazeCell>), typeof(MazeControl),
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = OnSolutionChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true,
                });



        public List<MazeCell> SolutionPathList
        {
            get => (List<MazeCell>)GetValue(SolutionPathListProperty);
            set => SetValue(SolutionPathListProperty, value);
        }

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

        private static void OnSolutionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                if (d is MazeControl c)
                {
                    c.UpdateSolutionInstance((Grid)c.Content, (List<MazeCell>)e.NewValue);
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
                        Height = 10,
                        Width = 10,
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

        private void UpdateSolutionInstance(Grid grid, List<MazeCell> solution)
        {
            for (var _ = 0; _ < solution.Count; _++)
            {
                var rect = grid.Children
                    .Cast<UIElement>()
                    .First(e => Grid.GetRow(e) == solution[_].Y && Grid.GetColumn(e) == solution[_].X);
                if (rect is Rectangle r)
                {
                    r.Fill = _ == 0 ? Brushes.Lime : _ == solution.Count() - 1 ? Brushes.Red : Brushes.Gold;
                }
            }

        }
    }
}
