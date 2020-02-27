using System.Windows;

namespace MazeAmazing_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Произошло необработанное исключение: " + e.Exception.Message, e.Exception.Source, MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
