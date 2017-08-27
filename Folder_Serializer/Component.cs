using System;
using System.Collections.Generic;
using System.IO;

namespace Folder_Serializer
{
    [Serializable]
    internal abstract class Component
    {
        protected Component(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; }
        public string Name => Path.GetFileName(FullName);
        public bool HasChildren { get; protected set; }

        public abstract void Add(Component component);
        public abstract IReadOnlyList<Component> GetChildren();
        public abstract void ReadFilesData();
        public abstract void WriteFilesData(string path);
    }
}