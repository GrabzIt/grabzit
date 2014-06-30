using System;
using System.Xml.Serialization;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class TakePictureResult : IException
    {
        public string Result;
        public string ID;

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

        public TakePictureResult()
        {            
        }
    }
}