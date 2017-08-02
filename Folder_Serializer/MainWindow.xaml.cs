using System.Windows;
using System.Windows.Forms;

namespace Folder_Serializer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DirectoryTreeManager treeManager;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void pickOutButton_Click(object sender, RoutedEventArgs e)
        {

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    selectedFolderLabel.Content = fbd.SelectedPath;

                    treeManager = new DirectoryTreeManager(fbd.SelectedPath);
                    treeManager.BuildTree();
                    ShowTree();

                }
            }
        }

        private void ShowTree()
        {
            treeViev.Items.Clear();
            treeViev.Items.Add(treeManager.GetTreeView());
        }

        private void serializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeManager != null)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Data file (.dat)|*.dat";

                    DialogResult result = sfd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(sfd.FileName))
                    {
                        treeManager.SerializeTree(sfd.FileName);
                        System.Windows.MessageBox.Show("Object has been serialized");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("No files to serialize!");
            }


        }

        private void deserializeButton_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Data file (.dat)|*.dat";
                DialogResult result = ofd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(ofd.FileName))
                {
                    try
                    {
                        Component component = DirectoryTreeManager.DeserializeTree(ofd.FileName);
                        if (component != null)
                        {
                            treeManager = new DirectoryTreeManager(component);
                            ShowTree();

                            selectedFolderLabel.Content = ofd.FileName;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        private void createFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeManager != null && treeManager.IsRootDataExist)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                    {
                        treeManager.CreateFiles(fbd.SelectedPath);
                        System.Windows.MessageBox.Show("Files are created!");
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("No data to creat files!");
            }

        }
    }
}
