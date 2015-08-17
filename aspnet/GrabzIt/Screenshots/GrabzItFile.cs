using GrabzIt.COM;
using System.IO;
using System.Runtime.InteropServices;

namespace GrabzIt.Screenshots
{
    [ClassInterface(ClassInterfaceType.None)]
    public class GrabzItFile : IGrabzItFile
    {
        public GrabzItFile(byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes
        {
            get;
            private set;
        }

        public void Save(string path)
        {
            File.WriteAllBytes(path, Bytes);
        }
    }
}
