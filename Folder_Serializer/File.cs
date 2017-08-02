using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folder_Serializer
{
    class File : Component
    {
        public File(string fullName) : base(fullName)
        { }

        public override void Add(Component component)
        {
            throw new InvalidOperationException();
        }

        public override int GetChildrenAmount()
        {
            return 0;
        }

        public override IReadOnlyList<Component> GetChildren()
        {
            throw new InvalidOperationException();
        }
    }
}
