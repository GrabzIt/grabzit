using GrabzIt.Enums;
using System;

namespace GrabzIt.COM
{
    public interface IGrabzItException
    {
        ErrorCode Code
        {
            get;
        }
    }
}
