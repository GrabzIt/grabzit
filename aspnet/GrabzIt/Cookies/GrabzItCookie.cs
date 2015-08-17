using GrabzIt.COM;
using System;
using System.Runtime.InteropServices;
using System.Web;

namespace GrabzIt.Cookies
{
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    public class GrabzItCookie : IGrabzItCookie
    {
        public string Name
        {
            get;
            private set;
        }

        public string Value
        {
            get;
            private set;
        }

        public string Domain
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }

        public string HttpOnly
        {
            get;
            private set;
        }

        public string Expires
        {
            get;
            private set;
        }

        public string Type
        {
            get;
            private set;
        }

        public GrabzItCookie()
        {            
        }
    }
}
