using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;

namespace Folder_Serializer
{
    class DirectoryTreeManager
    {
        public string CurrentPath { get; private set; }
        public Component Root { get; private set; }
        public bool FilesDataExist { get; set; }

        public DirectoryTreeManager(string path)
        {
            CurrentPath = path;
            BuildTree();
        }

        public DirectoryTreeManager(Component root)
        {
            Root = root;
            FilesDataExist = true;
        }

        public void BuildTree()
        {
            DirectoryInfo directory = new DirectoryInfo(CurrentPath);
            Root = ScanDirectory(directory);
        }

        private Component ScanDirectory(DirectoryInfo di)
        {
            Component component = new Folder(di.FullName);

            DirectoryInfo[] directories = di.GetDirectories();
            if (directories.Length > 0)
            {
                foreach (var item in directories)
                {
                    component.Add(ScanDirectory(item));
                }
            }
            ScanFiles(di, component);

            return component;
        }

        private static void ScanFiles(DirectoryInfo di, Component component)
        {
            FileInfo[] files = di.GetFiles();
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    component.Add(new File(file.FullName));
                }
            }
        }

        public TreeViewItem GetTreeView()
        {
            if (Root == null)
            {
                BuildTree();
            }

            return BuildTreeView(Root);
        }

        private TreeViewItem BuildTreeView(Component component)
        {
            TreeViewItem node = new TreeViewItem();
            node.Header = component.Name;

            if (component.HasChildren)
            {
                foreach (var item in component.GetChildren())
                {
                    node.Items.Add(BuildTreeView(item));
                }
            }

            return node;
        }

        public void TrySerializeTree(string path)
        {
            try
            {
                ReadFileData();

                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, Root);
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidDataException("Serealization error!\n Details: " + ex.Message);
            }
        }

        private void ReadFileData()
        {
            try
            {
                Root.ReadFilesData();
                FilesDataExist = true;
            }
            catch (System.Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public static Component TryDeserializeTree(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    Component deserilizeComponent = (Component)formatter.Deserialize(fs);
                    if (deserilizeComponent != null)
                    {
                        return deserilizeComponent;
                    }
                    else
                    {
                        throw new InvalidDataException("Deserealized file is null!");
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidDataException("Deserealization error!\n Details: " + ex.Message);
            }


        }

        public void CreateFiles(string path)
        {
            try
            {
                Root.WriteFilesData(path);
            }
            catch (System.Exception ex)
            {
                throw new InvalidDataException("Files creation error!\n Details: " + ex.Message);
            }

        }
    }
}
