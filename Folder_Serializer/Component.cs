using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_Serializer
{
    abstract class Component
    {
        protected string fullName;
        public string FullName { get { return fullName; } }
        public bool HasChildren { get; protected set; }

        public Component(string fullName)
        {
            this.fullName = fullName;
        }

        public abstract void Add(Component component);
        public abstract int GetChildrenAmount();
        public abstract IReadOnlyList<Component> GetChildren();
        
    }
}
