using System.Windows.Controls;
using MazeAmazing_WPF_MVC.Pages.Settings;

namespace MazeAmazing_WPF_MVC.Views.Settings
{
    /// <summary>
    /// Interaction logic for Appearance.xaml
    /// </summary>
    public partial class Appearance : UserControl
    {
        public Appearance()
        {
            InitializeComponent();

            // create and assign the appearance view model
            this.DataContext = new AppearanceViewModel();
        }
    }
}
