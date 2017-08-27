using System;
using System.Collections.Generic;
using System.IO;

namespace Folder_Serializer
{
    [Serializable]
    internal class Folder : Component
    {
        private readonly List<Component> _children;

        public Folder(string fullName) : base(fullName)
        {
            _children = new List<Component>();
        }

        public override void Add(Component component)
        {
            _children.Add(component);
            HasChildren = true;
        }

        public override IReadOnlyList<Component> GetChildren()
        {
            return _children;
        }

        public override void ReadFilesData()
        {
            try
            {
                foreach (var item in _children)
                    item.ReadFilesData();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }

        public override void WriteFilesData(string path)
        {
            var currentPath = $"{path}\\{Name}";
            try
            {
                Directory.CreateDirectory(currentPath);
                foreach (var item in _children)
                    item.WriteFilesData(currentPath);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
    }
}