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
            try
            {
                foreach (var item in children)
                {
                    item.ReadFilesData();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }

        }

        public override void WriteFilesData(string path)
        {
            string currentPath = $"{path}\\{Name}";
            try
            {
                Directory.CreateDirectory(currentPath);
                foreach (var item in children)
                {
                    item.WriteFilesData(currentPath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
    }
}
