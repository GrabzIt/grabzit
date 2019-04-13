using GrabzIt.COM;
using System;
using System.Runtime.InteropServices;

namespace GrabzIt.Cookies
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    public class GrabzItCookie : IGrabzItCookie
    {
        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public string Domain
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public string HttpOnly
        {
            get;
            set;
        }

        public string Expires
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public GrabzItCookie()
        {            
        }
    }
}
