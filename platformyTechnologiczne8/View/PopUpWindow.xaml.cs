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

        private void TxtName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
