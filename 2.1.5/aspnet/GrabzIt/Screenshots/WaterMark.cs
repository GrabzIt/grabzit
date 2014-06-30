using System;
using System.Xml.Serialization;
using GrabzIt.Enums;

namespace GrabzIt.Screenshots
{
    [Serializable]
    [XmlType(TypeName = "WebWaterMark")]
    public class WaterMark
    {
        public string Identifier;
        public string Format;
        public int XPosition;
        public int YPosition;

        public WaterMark()
        {
        }

        public HorizontalPosition GetXPosition()
        {
            return (HorizontalPosition)XPosition;
        }

        public VerticalPosition GetYPosition()
        {
            return (VerticalPosition)YPosition;
        }
    }
}
