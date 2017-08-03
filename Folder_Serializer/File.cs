using System;
using System.Collections.Generic;
using System.IO;

namespace Folder_Serializer
{
    [Serializable]
    class File : Component
    {
        private byte[] fileData;

        public File(string fullName) : base(fullName)
        { }

        public override void Add(Component component)
        {
            throw new InvalidOperationException();
        }

        public override IReadOnlyList<Component> GetChildren()
        {
            throw new InvalidOperationException();
        }

        public override void ReadFilesData()
        {
            try
            {
                using (FileStream fs = new FileStream(FullName, FileMode.Open))
                {
                    fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, (int)fs.Length);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }


        public override void WriteFilesData(string path)
        {
            try
            {
                using (FileStream fs = new FileStream($"{path}\\{Name}", FileMode.CreateNew))
                {
                    fs.Write(fileData, 0, (int)fileData.Length);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
    }
}
