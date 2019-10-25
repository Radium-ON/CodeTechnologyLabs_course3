using System.Windows;
using System.Windows.Controls;
using LabirintOperations;

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
                "MazeGrid", typeof(string), typeof(MazeControl), 
                new FrameworkPropertyMetadata() { 
                    PropertyChangedCallback = OnMazeGridChanged, 
                    BindsTwoWayByDefault = true });
 
        public MazeCell[,] MazeGrid
        {
            get { return (MazeCell[,])GetValue(MazeGridProperty ); }
            set { this.SetValue(MazeGridProperty , value); }
        }
 
        private static void OnMazeGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                // code to be executed on value update
            }
        }

        private void UpdateGridInstance()
        {

        }
    }
}
