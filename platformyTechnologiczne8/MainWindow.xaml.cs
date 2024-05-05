using System.IO;
using System.Windows;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace platformyTechnologiczne8
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnMaximize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new WinForms.FolderBrowserDialog();
            if (dialog.ShowDialog() != WinForms.DialogResult.OK) return;

            fileTreeView.Items.Clear();
            PopulateTreeView(dialog.SelectedPath, fileTreeView);
        }

        private void PopulateTreeView(string directory, System.Windows.Controls.TreeView treeView)
        {
            var rootDirectoryInfo = new DirectoryInfo(directory);
            var rootNode = CreateTreeViewItem(rootDirectoryInfo);
            treeView.Items.Add(rootNode);

            foreach (var directoryInfo in rootDirectoryInfo.GetDirectories())
            {
                var directoryNode = CreateTreeViewItem(directoryInfo);
                rootNode.Items.Add(directoryNode);
                PopulateSubDirectories(directoryInfo, directoryNode);
            }
        }

        private static System.Windows.Controls.TreeViewItem CreateTreeViewItem(DirectoryInfo directoryInfo)
        {
            return new System.Windows.Controls.TreeViewItem()
            {
                Header = directoryInfo.Name,
                Tag = directoryInfo.FullName
            };
        }

        public static void PopulateSubDirectories(DirectoryInfo directoryInfo,
            System.Windows.Controls.TreeViewItem parentNode)
        {
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                var subDirectoryNode = CreateTreeViewItem(subDirectoryInfo);
                parentNode.Items.Add(subDirectoryNode);
                PopulateSubDirectories(subDirectoryInfo, subDirectoryNode);
            }
        }
    }
}