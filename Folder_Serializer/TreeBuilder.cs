using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Folder_Serializer
{
    class TreeBuilder
    {
        public string CurrentPath { get; private set; }
        public Component Root { get; private set; }

        public TreeBuilder(string path)
        {
            CurrentPath = path;
            Root = new Folder(CurrentPath);
        }

        public void BuildTree()
        {
            DirectoryInfo directory = new DirectoryInfo(CurrentPath);
            Root = Get(directory);
        }

        private Component Get(DirectoryInfo di)
        {
            Component component = new Folder(di.Name);

            DirectoryInfo[] directories = di.GetDirectories();
            if (directories.Length>0)
            {
                foreach (var item in directories)
                {
                    component.Add(Get(item));
                }
            }

            FileInfo[] files =  di.GetFiles();
            if (files.Length>0)
            {
                foreach (var file in files)
                {
                    component.Add(new File(file.Name));
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
            node.Header = (component.FullName);

            if (component.HasChildren)
            {
                foreach (var item in component.GetChildren())
                {
                    node.Items.Add( BuildTreeView(item));
                }
            }

            return node;
        }
    }
}
