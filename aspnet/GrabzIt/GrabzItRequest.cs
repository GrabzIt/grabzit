using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt
{
    internal class GrabzItRequest
    {
        [ThreadStatic]
        private static string static_request;
        [ThreadStatic]
        private static string static_signaturePartOne;
        [ThreadStatic]
        private static string static_signaturePartTwo;

        private string instance_request;
        private string instance_signaturePartOne;
        private string instance_signaturePartTwo;

        private bool isStatic;

        internal GrabzItRequest(bool isStatic)
        {
            this.isStatic = isStatic;
        }

        internal string Request
        {
            get
            {
                if (isStatic)
                {
                    return static_request;
                }
                return instance_request;
            }
            set
            {
                if (isStatic)
                {
                    static_request = value;
                }
                else
                {
                    instance_request = value;
                }
            }
        }

        internal string SignaturePartOne
        {
            get
            {
                if (isStatic)
                {
                    return static_signaturePartOne;
                }
                return instance_signaturePartOne;
            }
            set
            {
                if (isStatic)
                {
                    static_signaturePartOne = value;
                }
                else
                {
                    instance_signaturePartOne = value;
                }
            }
        }

        internal string SignaturePartTwo
        {
            get
            {
                if (isStatic)
                {
                    return static_signaturePartTwo;
                }
                return instance_signaturePartTwo;
            }
            set
            {
                if (isStatic)
                {
                    static_signaturePartTwo = value;
                }
                else
                {
                    instance_signaturePartTwo = value;
                }
            }
        }
    }
}
