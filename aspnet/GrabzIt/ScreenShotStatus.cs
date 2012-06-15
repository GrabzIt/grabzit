using System;
using System.IO;
using System.Xml.Serialization;

namespace GrabzIt
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class ScreenShotStatus
    {
        public bool Processing;
        public bool Cached;
        public bool Expired;
        public string Message;

        public ScreenShotStatus()
        {            
        }
    }
}
