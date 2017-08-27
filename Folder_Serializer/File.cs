using System;
using System.Collections.Generic;
using System.IO;

namespace Folder_Serializer
{
    [Serializable]
    internal class File : Component
    {
        private byte[] _fileData;

        public File(string fullName) : base(fullName)
        {
        }

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
                using (var fs = new FileStream(FullName, FileMode.Open))
                {
                    _fileData = new byte[fs.Length];
                    fs.Read(_fileData, 0, (int) fs.Length);
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
                using (var fs = new FileStream($"{path}\\{Name}", FileMode.CreateNew))
                {
                    fs.Write(_fileData, 0, _fileData.Length);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(ex.Message);
            }
        }
    }
}