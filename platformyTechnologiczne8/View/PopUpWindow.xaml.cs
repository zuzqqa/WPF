using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace platformyTechnologiczne8.View
{
    public partial class PopUpWindow : Window
    {
        public PopUpWindow()
        {
            InitializeComponent();
        }

        private void PopUpWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            return true;
        }

        private bool BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            return false;
        }
    }
}
