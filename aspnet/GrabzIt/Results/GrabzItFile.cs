using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrabzIt.Results
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
