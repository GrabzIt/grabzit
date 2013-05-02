using System;
using System.Xml.Serialization;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class GenericResult
    {
        public string Result;
        public string Message;

        public GenericResult()
        {            
        }
    }
}