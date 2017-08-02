using System;
using System.Collections.Generic;
using System.IO;

namespace Folder_Serializer
{
    [Serializable]
    class Folder : Component
    {
        List<Component> children;


        public Folder(string fullName) : base(fullName)
        {
            children = new List<Component>();
        }

        public override void Add(Component component)
        {
            children.Add(component);
            HasChildren = true;
        }

        public override IReadOnlyList<Component> GetChildren()
        {
            return children;
        }

        public override void ReadFilesData()
        {
            foreach (var item in children)
            {
                item.ReadFilesData();
            }
        }

        public override void WriteFilesData(string path)
        {
            string currentPath = $"{path}\\{Name}";
            Directory.CreateDirectory(currentPath);

            foreach (var item in children)
            {
                item.WriteFilesData(currentPath);
            }
        }
    }
}
