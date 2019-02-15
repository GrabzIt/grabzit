using GrabzIt.Enums;
using System;
using System.Runtime.InteropServices;

namespace GrabzIt.COM
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
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
