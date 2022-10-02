using System.Windows;
using Pinax.UI.Windows;

namespace Pinax.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Help_OnClick(object sender, RoutedEventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.Owner = this;

            helpWindow.Show();
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.Owner = this;

            aboutWindow.ShowDialog();
        }
    }
}
