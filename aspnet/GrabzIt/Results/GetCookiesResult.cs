using System;
using System.Xml.Serialization;
using GrabzIt.Cookies;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class GetCookiesResult
    {
        [XmlArray("Cookies")]
        [XmlArrayItem("Cookie")]
        public GrabzItCookie[] Cookies;
        public string Message;

        public GetCookiesResult()
        {            
        }
    }
}