using System;
using System.Xml.Serialization;
using GrabzIt.Cookies;
using System.Runtime.InteropServices;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    [ComVisible(false)]
    public class GetCookiesResult : IException
    {
        [XmlArray("Cookies")]
        [XmlArrayItem("Cookie")]
        public GrabzItCookie[] Cookies;

        public string Message
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public GetCookiesResult()
        {            
        }
    }
}