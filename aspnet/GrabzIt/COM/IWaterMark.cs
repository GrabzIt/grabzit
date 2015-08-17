using GrabzIt.Enums;
using System;

namespace GrabzIt.COM
{
    public interface IWaterMark
    {
        string Identifier
        {
            get;
        }

        string Format
        {
            get;
        }

        int XPosition
        {
            get;
        }

        int YPosition
        {
            get;
        }

        HorizontalPosition GetXPosition();

        VerticalPosition GetYPosition();
    }
}
