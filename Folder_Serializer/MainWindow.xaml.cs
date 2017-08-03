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
            Waiting_grid.Visibility = Visibility.Hidden;
        }

        private async void pickOutButton_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    Waiting_grid.Visibility = Visibility.Visible;
                    pickOutButton.IsEnabled = false;

                    var slowTask = Task<DirectoryTreeManager>.Factory.StartNew(() => CreateTreeManager(fbd));
                    await slowTask;
                    treeManager = slowTask.Result;

                    selectedFolderLabel.Content = fbd.SelectedPath;
                    ShowTree();

                    pickOutButton.IsEnabled = true;
                    Waiting_grid.Visibility = Visibility.Hidden;
                }
            }
        }

        private DirectoryTreeManager CreateTreeManager(FolderBrowserDialog fbd)
        {
            return new DirectoryTreeManager(fbd.SelectedPath);
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
                        serializeButton.IsEnabled = false;
                        serializeWaitLabel.Visibility = Visibility.Visible;

                        try
                        {
                            await SerializeTree(sfd);
                            System.Windows.MessageBox.Show("Object has been serialized");
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }

                        serializeButton.IsEnabled = true;
                        serializeWaitLabel.Visibility = Visibility.Hidden;
                    }
                }
            }
            else { System.Windows.MessageBox.Show("No files to serialize!"); }
        }

        private async Task SerializeTree(SaveFileDialog sfd)
        {
            var slowTask = Task.Factory.StartNew(() => treeManager.TrySerializeTree(sfd.FileName));
            await slowTask;
        }

        private async void deserializeButton_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new OpenFileDialog() { Filter = "Data file (.dat)|*.dat" })
            {
                DialogResult result = ofd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(ofd.FileName))
                {
                    deserializeButton.IsEnabled = false;
                    deserializeWaitLabel.Visibility = Visibility.Visible;
                    try
                    {
                        Task<Component> slowTask = await DeserealizeTree(ofd);
                        Component component = slowTask.Result;
                        if (component != null)
                        {
                            treeManager = new DirectoryTreeManager(component);
                            ShowTree();

                            selectedFolderLabel.Content = ofd.FileName;
                            System.Windows.MessageBox.Show("Files !");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }

                    deserializeButton.IsEnabled = true;
                    deserializeWaitLabel.Visibility = Visibility.Hidden;
                }
            }
        }

        private static async Task<Task<Component>> DeserealizeTree(OpenFileDialog ofd)
        {
            var slowTask = Task<Component>.Factory.StartNew(() => DirectoryTreeManager.TryDeserializeTree(ofd.FileName));
            await slowTask;
            return slowTask;
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
                        createFilesButton.IsEnabled = false;
                        createFileWaitLabel.Visibility = Visibility.Visible;
                        try
                        {
                            await CreateFiles(fbd);
                            System.Windows.MessageBox.Show("Files have been created!");
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }

                        createFilesButton.IsEnabled = true;
                        createFileWaitLabel.Visibility = Visibility.Hidden;
                    }
                }
            }
            else { System.Windows.MessageBox.Show("No data to creat files!"); }
        }

        private async Task CreateFiles(FolderBrowserDialog fbd)
        {
            var slowTask = Task.Factory.StartNew(() => treeManager.CreateFiles(fbd.SelectedPath));
            await slowTask;
        }
    }
}
