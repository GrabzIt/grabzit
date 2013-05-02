using System;
using System.Xml.Serialization;
using GrabzIt.Screenshots;

namespace GrabzIt.Results
{
    [Serializable]
    [XmlType(TypeName = "WebResult")]
    public class GetWatermarksResult
    {
        [XmlArray("WaterMarks")]
        [XmlArrayItem("WaterMark")]
        public WaterMark[] WaterMarks;
        public string Message;

        public GetWatermarksResult()
        {            
        }
    }
}