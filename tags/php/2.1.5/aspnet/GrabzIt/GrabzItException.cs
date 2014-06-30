using System;
using GrabzIt.Enums;

namespace GrabzIt
{
    public class GrabzItException : Exception
    {
        public ErrorCode code;

        internal GrabzItException(string message, string code)
            : this(message, (ErrorCode)Convert.ToInt32(code))
        {
        }

        internal GrabzItException(string message, ErrorCode code)
            : base(message)
        {
            this.code = code;
        }

        public ErrorCode Code
        {
            get
            {
                return code;
            }
            set
            {
                Code = code;
            }
        }
    }
}
