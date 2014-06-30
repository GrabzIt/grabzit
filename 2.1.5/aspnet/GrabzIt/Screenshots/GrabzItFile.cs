using System.IO;

namespace GrabzIt.Screenshots
{
    public class GrabzItFile
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
