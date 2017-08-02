using System;
using System.Collections.Generic;
using System.IO;

namespace Folder_Serializer
{
    [Serializable]
    abstract class Component
    {
        protected string fullName;
        public string FullName { get { return fullName; } }
        public string Name { get { return Path.GetFileName(fullName); } }

        public bool HasChildren { get; protected set; }

        public Component(string fullName)
        {
            this.fullName = fullName;
        }

        public abstract void Add(Component component);
        public abstract IReadOnlyList<Component> GetChildren();
        public abstract void ReadFilesData();
        public abstract void WriteFilesData(string path);

    }
}
