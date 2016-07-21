using System;

namespace GrabzIt.COM
{
    public interface IGrabzItFile
    {
        byte[] Bytes
        {
            get;
        }
        void Save(string path);
        string ToString();
    }
}
