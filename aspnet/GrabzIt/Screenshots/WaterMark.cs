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
            private set;
        }

        public string Format
        {
            get;
            private set;
        }

        public int XPosition
        {
            get;
            private set;
        }

        public int YPosition
        {
            get;
            private set;
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
