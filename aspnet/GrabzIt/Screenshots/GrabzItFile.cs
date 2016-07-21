using GrabzIt.COM;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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

        /// <summary>
        /// Only use this with text based formats such as CSV or JSON otherwise it will return a string that represents a byte array.
        /// </summary>
        /// <returns>a string representing the file</returns>
        public override string ToString()
        {
            if (Bytes != null)
            {
                return Encoding.UTF8.GetString(Bytes);
            }
            return string.Empty;
        }
    }
}
