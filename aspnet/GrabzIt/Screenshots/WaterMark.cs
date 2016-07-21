using System;
using System.Xml.Serialization;
using GrabzIt.Enums;
using System.Runtime.InteropServices;
using GrabzIt.COM;

namespace GrabzIt.Screenshots
{
    [Serializable]
    [XmlType(TypeName = "WebWaterMark")]
    [ClassInterface(ClassInterfaceType.None)]
    public class WaterMark : IWaterMark
    {
        public string Identifier
        {
            get;
            set;
        }

        public string Format
        {
            get;
            set;
        }

        public int XPosition
        {
            get;
            set;
        }

        public int YPosition
        {
            get;
            set;
        }

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
