using System;
using System.Web;

namespace GrabzIt.Cookies
{
    [Serializable]
    public class GrabzItCookie
    {
        public string Name;
        public string Value;
        public string Domain;
        public string Path;
        public string HttpOnly;
        public string Expires;
        public string Type;

        public GrabzItCookie()
        {            
        }
    }
}
