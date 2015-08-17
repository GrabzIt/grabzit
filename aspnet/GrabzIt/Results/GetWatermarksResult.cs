using System;
using System.Xml.Serialization;
using GrabzIt.Screenshots;
using System.Runtime.InteropServices;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    [ComVisible(false)]
    public class GetWatermarksResult : IException
    {
        [XmlArray("WaterMarks")]
        [XmlArrayItem("WaterMark")]
        public WaterMark[] WaterMarks;

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

        public GetWatermarksResult()
        {            
        }
    }
}