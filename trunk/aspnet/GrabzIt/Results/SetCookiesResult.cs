using System;
using System.IO;
using System.Xml.Serialization;

namespace GrabzIt.Result
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class SetCookiesResult
    {
        public string Result;
        public string Message;

        public SetCookiesResult()
        {            
        }
    }
}
