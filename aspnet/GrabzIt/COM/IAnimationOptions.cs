using GrabzIt.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt.COM
{
    interface IAnimationOptions
    {
        Country Country
        {
            get;
            set;
        }

        string CustomId
        {
            get;
            set;
        }

        int Width
        {
            get;
            set;
        }

        int Height
        {
            get;
            set;
        }

        int Start
        {
            get;
            set;
        }

        int Duration
        {
            get;
            set;
        }

        float Speed
        {
            get;
            set;
        }

        float FramesPerSecond
        {
            get;
            set;
        }

        int Repeat
        {
            get;
            set;
        }

        bool Reverse
        {
            get;
            set;
        }

        string CustomWaterMarkId
        {
            get;
            set;
        }

        int Quality
        {
            get;
            set;
        }

    }
}
