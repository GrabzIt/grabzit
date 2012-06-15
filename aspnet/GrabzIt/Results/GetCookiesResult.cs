using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GrabzIt.Cookies;

namespace GrabzIt.Result
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
