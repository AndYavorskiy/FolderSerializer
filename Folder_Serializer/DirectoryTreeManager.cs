using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;

namespace Folder_Serializer
{
    class DirectoryTreeManager
    {
        public string CurrentPath { get; private set; }
        public Component Root { get; private set; }
        public bool IsRootDataExist { get; private set; } = false;

        public DirectoryTreeManager(string path)
        {
            CurrentPath = path;
            Root = new Folder(CurrentPath);
        }

        public DirectoryTreeManager(Component root)
        {
            Root = root;
            IsRootDataExist = true;
        }

        public void BuildTree()
        {
            DirectoryInfo directory = new DirectoryInfo(CurrentPath);
            Root = Get(directory);
        }

        private Component Get(DirectoryInfo di)
        {
            Component component = new Folder(di.FullName);

            DirectoryInfo[] directories = di.GetDirectories();
            if (directories.Length > 0)
            {
                foreach (var item in directories)
                {
                    component.Add(Get(item));
                }
            }

            FileInfo[] files = di.GetFiles();
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    component.Add(new File(file.FullName));
                }
            }

            return component;
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

        public void SerializeTree(string path)
        {
            Root.ReadFilesData();

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Root);
            }
        }

        public static Component DeserializeTree(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                Component deserilizeComponent = (Component)formatter.Deserialize(fs);
                if (deserilizeComponent!=null)
                {
                    return deserilizeComponent;
                }
                else
                {
                    throw new InvalidDataException("Deserealization error!");
                }
            }
        }

        public void CreateFiles(string path)
        {
            if (Root == null)
            {
                BuildTree();
            }

            if (IsRootDataExist)
            {
                Root.WriteFilesData(path);
            }
        }
    }
}
