using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace platformyTechnologiczne8.View
{
    public partial class PopUpWindow : Window
    {
        public bool Success { get; set; }
        public string FileName { get; private set; }
        public string FolderPath { get; private set; }
        public bool IsFile { get; private set; }
        public bool IsDirectory { get; private set; }
        public bool HasReadOnly { get; private set; }
        public bool HasArchive { get; private set; }
        public bool HasHidden { get; private set; }
        public bool HasSystem { get; private set; }


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

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            FileName = txtName.Text;

            IsFile = rdFile.IsChecked == true;
            IsDirectory = rdDir.IsChecked == true;

            HasReadOnly = chbxReadOnly.IsChecked == true;
            HasArchive = chbxArchive.IsChecked == true;
            HasHidden = chbxHidden.IsChecked == true;
            HasSystem = chbxSystem.IsChecked == true;

            Success = true;
            this.Close();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Success = false;
            this.Close();
        }
    }
}
