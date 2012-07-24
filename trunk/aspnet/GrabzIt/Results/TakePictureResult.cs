using System;
using System.Xml.Serialization;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class TakePictureResult
    {
        public string Result;
        public string ID;
        public string Message;

        public TakePictureResult()
        {            
        }
    }
}