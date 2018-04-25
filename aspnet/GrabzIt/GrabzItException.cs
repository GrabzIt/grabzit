using System;
using GrabzIt.Enums;
using GrabzIt.COM;
using System.Runtime.InteropServices;

namespace GrabzIt
{
    [ClassInterface(ClassInterfaceType.None)]
    public class GrabzItException : Exception, IGrabzItException
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

        internal GrabzItException(string message, ErrorCode code, Exception e)
            : base(message, e)
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
