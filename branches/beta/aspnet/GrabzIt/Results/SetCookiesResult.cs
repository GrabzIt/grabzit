using System;
using System.Xml.Serialization;

namespace GrabzIt.Results
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