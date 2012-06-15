using System;
using System.IO;
using System.Xml.Serialization;

namespace GrabzIt
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