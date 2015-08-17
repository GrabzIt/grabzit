using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    [ComVisible(false)]
    public class GenericResult : IException
    {
        public string Result;

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

        public GenericResult()
        {            
        }
    }
}