using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_Serializer
{
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

        public override int GetChildrenAmount()
        {
            return children.Count;
        }

        public override IReadOnlyList<Component> GetChildren()
        {
            return children;
        }
    }
}
