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

        #region DependencyProperties

        public static readonly DependencyProperty MazeGridProperty =
            DependencyProperty.Register(
                "MazeGrid", typeof(Maze), typeof(MazeControl),
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = OnMazeGridChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true
                });

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



        public static readonly DependencyProperty SolutionPathListProperty =
            DependencyProperty.Register(
                "SolutionPathList", typeof(List<MazeCell>), typeof(MazeControl),
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = OnSolutionChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true,
                });

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



        public static readonly DependencyProperty StartPositionProperty =
            DependencyProperty.Register(
                "StartMazePosition", typeof(MazeCell), typeof(MazeControl),
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = OnStartExitPositionChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true,
                });

        public static readonly DependencyProperty ExitPositionProperty =
            DependencyProperty.Register(
                "ExitMazePosition", typeof(MazeCell), typeof(MazeControl),
                new FrameworkPropertyMetadata
                {
                    PropertyChangedCallback = OnStartExitPositionChanged,
                    AffectsArrange = true,
                    AffectsMeasure = true,
                });

        private static void OnStartExitPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                if (d is MazeControl c)
                {
                    c.SolutionPathList.Clear();
                    c.UpdateStartExitInstance((Grid)c.Content, (MazeCell)e.NewValue);
                }
            }
        }

        #endregion


        public MazeCell StartMazePosition
        {
            get => (MazeCell)GetValue(StartPositionProperty);
            set => SetValue(StartPositionProperty, value);
        }

        public MazeCell ExitMazePosition
        {
            get => (MazeCell)GetValue(ExitPositionProperty);
            set => SetValue(ExitPositionProperty, value);
        }

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
                    };

                    if (cell.CellType == CellType.Wall)
                    {
                        field.SetResourceReference(Shape.FillProperty, "Accent");
                    }
                    else
                    {
                        field.Fill = Brushes.White;
                    }
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
                    r.Fill = Brushes.Gold;
                }
            }
        }

        private void UpdateStartExitInstance(Grid grid, MazeCell cell)
        {
            var rect = grid.Children
                .Cast<UIElement>()
                .First(e => Grid.GetRow(e) == cell.Y && Grid.GetColumn(e) == cell.X);
            if (rect is Rectangle r)
            {
                r.Fill = cell.CellType == CellType.Start ? Brushes.Lime : Brushes.Red;
            }
        }
    }
}
