using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Folder_Serializer
{
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
                    pickOutButton.IsEnabled = false;

                    treeManager = new DirectoryTreeManager(fbd.SelectedPath);
                    ShowTree();
                }
                pickOutButton.IsEnabled = true;
            }
        }

        private void ShowTree()
        {
            treeViev.Items.Clear();
            treeViev.Items.Add(treeManager.GetTreeView());
        }

        private async void serializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeManager != null)
            {
                using (var sfd = new SaveFileDialog() { Filter = "Data file (.dat)|*.dat" })
                {
                    DialogResult result = sfd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(sfd.FileName))
                    {
                        try
                        {
                            serializeButton.IsEnabled = false;
                            var slowTask = Task.Factory.StartNew(() => treeManager.TrySerializeTree(sfd.FileName));
                            await slowTask;
                            System.Windows.MessageBox.Show("Object has been serialized");
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            serializeButton.IsEnabled = true;
                        }
                    }
                }
            }
            else { System.Windows.MessageBox.Show("No files to serialize!"); }
        }

        private async void deserializeButton_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new OpenFileDialog() { Filter = "Data file (.dat)|*.dat" })
            {
                DialogResult result = ofd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(ofd.FileName))
                {
                    try
                    {
                        deserializeButton.IsEnabled = false;

                        var slowTask = Task<Component>.Factory.StartNew(() => DirectoryTreeManager.TryDeserializeTree(ofd.FileName));
                        await slowTask;

                        Component component = slowTask.Result;
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
                    finally
                    {
                        deserializeButton.IsEnabled = true;
                    }
                }
            }
        }

        private async void createFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeManager != null && treeManager.FilesDataExist)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                    {
                        try
                        {
                            createFilesButton.IsEnabled = false;

                            var slowTask = Task.Factory.StartNew(() => treeManager.CreateFiles(fbd.SelectedPath));
                            await slowTask;
                            
                            System.Windows.MessageBox.Show("Files are created!");
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            createFilesButton.IsEnabled = true;
                        }
                    }
                }
            }
            else { System.Windows.MessageBox.Show("No data to creat files!"); }
        }
    }
}
