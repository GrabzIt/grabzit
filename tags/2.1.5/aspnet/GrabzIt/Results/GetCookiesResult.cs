using System;
using System.Xml.Serialization;
using GrabzIt.Cookies;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
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