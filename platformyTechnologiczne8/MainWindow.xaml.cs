using platformyTechnologiczne8.View;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using WinControls = System.Windows.Controls;
using WinForms = System.Windows.Forms;
using SymIO = System.IO;

namespace platformyTechnologiczne8
{
    public partial class MainWindow : Window
    {
        private String selectedDirectory { get; set; }

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
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Directory
                .GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName)
                .FullName;

            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                selectedDirectory = dialog.SelectedPath;
                PopulateTreeView(selectedDirectory, fileTreeView);
            }
        }

        private void PopulateTreeView(string directory, WinControls.TreeView treeView)
        {
            fileTreeView.Items.Clear();

            var rootDirectoryInfo = new DirectoryInfo(directory);
            var rootNode = CreateTreeViewItem(rootDirectoryInfo);
            treeView.Items.Add(rootNode);

            foreach (var directoryInfo in rootDirectoryInfo.GetDirectories("*", SearchOption.AllDirectories))
            {
                var directoryNode = CreateTreeViewItem(directoryInfo);
                rootNode.Items.Add(directoryNode);

                PopulateSubDirectories(directoryInfo, directoryNode);
            }

            rootNode.ContextMenu = CreateContextMenu(directory);
        }

        private static TreeViewItem CreateTreeViewItem(DirectoryInfo directoryInfo)
        {
            return new TreeViewItem
            {
                Header = directoryInfo.Name,
                Tag = directoryInfo.FullName
            };
        }

        private static TreeViewItem CreateTreeViewItem(FileInfo fileInfo)
        {
            return new TreeViewItem
            {
                Header = fileInfo.Name,
                Tag = fileInfo.FullName
            };
        }

        public static void PopulateSubDirectories(DirectoryInfo directoryInfo,
            TreeViewItem parentNode)
        {
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories("*", SearchOption.AllDirectories))
            {
                var subDirectoryNode = CreateTreeViewItem(subDirectoryInfo);
                parentNode.Items.Add(subDirectoryNode);

                PopulateSubDirectories(subDirectoryInfo, subDirectoryNode);

                foreach (var fileInfo in subDirectoryInfo.GetFiles())
                {
                    var fileNode = CreateTreeViewItem(fileInfo);
                    subDirectoryNode.Items.Add(fileNode);
                }
            }

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                var fileNode = CreateTreeViewItem(fileInfo);
                parentNode.Items.Add(fileNode); 
            }
        }

        private ContextMenu CreateContextMenu(string path)
        {
            var contextMenu = new ContextMenu();
            var isFolder = Directory.Exists(path);

            var deleteMenuItem = new MenuItem() { Header = "Delete" };
            deleteMenuItem.Click += deleteMenuItem_OnClick;
            contextMenu.Items.Add(deleteMenuItem);

            if (isFolder)
            {
                var createMenuItem = new MenuItem() { Header = "Create" };
                createMenuItem.Click += CreateMenuItem_Click;
                contextMenu.Items.Add(createMenuItem);
            }

            return contextMenu;
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow popUpWindow = new PopUpWindow();
            popUpWindow.ShowDialog();

            if (popUpWindow.Success)
            {
                TreeViewItem selectedItem = (TreeViewItem)fileTreeView.SelectedItem;
                string fileName = popUpWindow.FileName;
                string folderPath = selectedItem.Tag.ToString();

                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(folderPath))
                {
                    if (Regex.IsMatch(fileName, @"^[a-zA-Z0-9_~.-]{1,8}\.(txt|php|html)$"))
                    {
                        string fullPath = Path.Combine(folderPath, fileName);

                        if (popUpWindow.IsFile)
                        {
                            using (FileStream fs = File.Create(fullPath))
                            {
                                FileAttributes attributes = FileAttributes.Normal;

                                if (popUpWindow.HasReadOnly)
                                    attributes |= FileAttributes.ReadOnly;
                                if (popUpWindow.HasArchive)
                                    attributes |= FileAttributes.Archive;
                                if (popUpWindow.HasHidden)
                                    attributes |= FileAttributes.Hidden;
                                if (popUpWindow.HasSystem)
                                    attributes |= FileAttributes.System;

                                File.SetAttributes(fullPath, attributes);
                            }

                            TreeViewItem newFileNode = CreateTreeViewItem(new FileInfo(fullPath));
                            selectedItem.Items.Add(newFileNode);
                        }
                        else if (popUpWindow.IsDirectory)
                        {
                            Directory.CreateDirectory(fullPath);
                            TreeViewItem newDirectoryNode = CreateTreeViewItem(new DirectoryInfo(fullPath));
                            selectedItem.Items.Add(newDirectoryNode);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid file name! Please enter a valid file name with .txt, .php, or .html extension and up to 8 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
               
            }
        }

        private void deleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (TreeViewItem)fileTreeView.SelectedItem;
            var parentItem = (TreeViewItem)selectedItem.Parent;
            var path = (string)selectedItem.Tag;

            // decide whether the selected item is a file or a directory
            object entry = Directory.Exists(path) ? new DirectoryInfo(path) : new FileInfo(path);

            switch (entry)
            {
                case FileInfo fileInfo:
                    var attributes1 = fileInfo.Attributes;

                    if ((attributes1 & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        File.SetAttributes(path, attributes1 & ~FileAttributes.ReadOnly);
                    }

                    File.Delete(path);
                    break;
                case DirectoryInfo directoryInfo:
                    // delete all files and subdirectories
                    foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        var attributes2 = fileInfo.Attributes;

                        if ((attributes2 & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            File.SetAttributes(file, attributes2 & ~FileAttributes.ReadOnly);
                        }

                        File.Delete(file);
                    }

                    foreach (var subDir in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                        Directory.Delete(subDir, true);

                    Directory.Delete(path, true);
                    break;
            }

            parentItem.Items.Remove(selectedItem);
        }

        private void FileTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(fileTreeView.SelectedItem == null) return;

            var selectedItem = (TreeViewItem)fileTreeView.SelectedItem;

            var path = (string)selectedItem.Tag;

            // decide whether the selected item is a file or a directory
            object entry = Directory.Exists(path) ? new DirectoryInfo(path) : new FileInfo(path);

            switch (entry)
            {
                case FileInfo fileInfo:
                    var parentDirectory = fileInfo.Directory;
                    var numberOfFilesInParent =
                        Directory.GetFiles(parentDirectory?.FullName ?? string.Empty, "*", SearchOption.AllDirectories)
                            .Length;
                    var attributes = GetDosAttributes(fileInfo);

                    txtNumberOfItems.Text = numberOfFilesInParent > 1
                        ? " " + numberOfFilesInParent + " items | "
                        : numberOfFilesInParent + " item | ";

                    txtSizeOfFiles.Text = "1 selected file " + Math.Round((double)fileInfo.Length / 1024, 2) + " KB | ";
                    txtPermissions.Text = attributes + " |";

                    string fileContent = File.ReadAllText(fileInfo.FullName);
                    txtHeader.Text = fileInfo.Name;
                    txtFileContent.Text = fileContent;

                    break;
                case DirectoryInfo directoryInfo:
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);

                    txtNumberOfItems.Text = directories.Length + files.Length > 1
                        ? " " + (directories.Length + files.Length) + " items | "
                        : directories.Length + files.Length + " item | ";
                    txtSizeOfFiles.Text = "1 item selected |";
                    break;
            }
        }

        public static string GetDosAttributes(FileSystemInfo fileSystemInfo)
        {
            return $"{((fileSystemInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly ? "r" : "-")}" +
                   $"{((fileSystemInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive ? "a" : "-")}" +
                   $"{((fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden ? "h" : "-")}" +
                   $"{((fileSystemInfo.Attributes & FileAttributes.System) == FileAttributes.System ? "s" : "-")}";
        }
    }
}