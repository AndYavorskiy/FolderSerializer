using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace Folder_Serializer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void pickOutButton_Click(object sender, RoutedEventArgs e)
        {
            treeViev.Items.Clear();

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    selectedFolderLabel.Content = fbd.SelectedPath;

                    TreeBuilder treeBuilder = new TreeBuilder(fbd.SelectedPath);
                    treeBuilder.BuildTree();
                    Component component = treeBuilder.Root;

                    treeViev.Items.Add(treeBuilder.GetTreeView());

                    #region PrevCode
                    //selectedFolderLabel.Content = fbd.SelectedPath;

                    //DirectoryInfo[] directories = new DirectoryInfo(fbd.SelectedPath).GetDirectories();

                    //TreeViewItem tvi = new TreeViewItem();
                    //tvi.Header = fbd.SelectedPath;

                    //foreach (var item in directories)
                    //{
                    //    TreeViewItem treenode = new TreeViewItem() { Header = $"{item.FullName}" };
                    //    foreach (var nodeItem in item.GetDirectories())
                    //    {
                    //        treenode.Items.Add(new TreeViewItem() { Header = $"{nodeItem.FullName}" });
                    //    }
                    //    tvi.Items.Add(treenode);
                    //}
                    //treeViev.Items.Add(tvi);
                    #endregion
                }
            }
        }

        
    }
}
