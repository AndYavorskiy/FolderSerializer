using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;

namespace Folder_Serializer
{
    /// <summary>
    ///     sdsd
    /// </summary>
    internal class DirectoryTreeManager
    {
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

        public string CurrentPath { get; }

        public Component Root { get; private set; }

        public bool FilesDataExist { get; set; }

        public void BuildTree()
        {
            var directory = new DirectoryInfo(CurrentPath);
            Root = ScanDirectory(directory);
        }

        private Component ScanDirectory(DirectoryInfo di)
        {
            Component component = new Folder(di.FullName);

            var directories = di.GetDirectories();
            if (directories.Length > 0)
                foreach (var item in directories)
                    component.Add(ScanDirectory(item));
            ScanFiles(di, component);

            return component;
        }

        private static void ScanFiles(DirectoryInfo di, Component component)
        {
            var files = di.GetFiles();
            if (files.Length <= 0) return;
            foreach (var file in files)
                component.Add(new File(file.FullName));
        }

        public TreeViewItem GetTreeView()
        {
            if (Root == null)
                BuildTree();

            return BuildTreeView(Root);
        }

        private TreeViewItem BuildTreeView(Component component)
        {
            var node = new TreeViewItem {Header = component.Name};

            if (!component.HasChildren) return node;
            foreach (var item in component.GetChildren())
                node.Items.Add(BuildTreeView(item));

            return node;
        }

        public void TrySerializeTree(string path)
        {
            try
            {
                ReadFileData();

                var formatter = new BinaryFormatter();
                using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, Root);
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public static Component TryDeserializeTree(string path)
        {
            var formatter = new BinaryFormatter();

            try
            {
                using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    var deserilizeComponent = (Component) formatter.Deserialize(fs);
                    return deserilizeComponent;
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                throw new InvalidDataException("Files creation error!\n Details: " + ex.Message);
            }
        }
    }
}