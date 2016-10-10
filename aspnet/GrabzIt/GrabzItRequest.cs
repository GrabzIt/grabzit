using GrabzIt.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt
{
    internal class GrabzItRequest
    {
        [ThreadStatic]
        private static BaseOptions static_options;
        private BaseOptions instance_options;

        [ThreadStatic]
        private static bool static_post;
        private bool instance_post;

        [ThreadStatic]
        private static string static_ws_url;
        private string instance_ws_url;

        [ThreadStatic]
        private static string static_data;
        private string instance_data;

        private bool isStatic;

        internal GrabzItRequest(bool isStatic)
        {
            this.isStatic = isStatic;
        }

        internal void Store(string wsUrl, bool post, BaseOptions options)
        {
            Store(wsUrl, post, options, null);
        }

        internal void Store(string wsUrl, bool post, BaseOptions options, string data)
        {
            if (isStatic)
            {
                static_ws_url = wsUrl;
                static_post = post;
                static_options = options;
                static_data = data;
                return;
            }
            instance_ws_url = wsUrl;
            instance_post = post;
            instance_options = options;
            instance_data = data;
        }

        internal BaseOptions Options
        {
            get
            {
                if (isStatic)
                {
                    return static_options;
                }
                return instance_options;
            }
            set
            {
                if (isStatic)
                {
                    static_options = value;
                }
                else
                {
                    instance_options = value;
                }
            }
        }

        internal bool IsPost
        {
            get
            {
                if (isStatic)
                {
                    return static_post;
                }
                return instance_post;
            }
        }

        internal string Data
        {
            get
            {
                if (isStatic)
                {
                    return static_data;
                }
                return instance_data;
            }
        }

        internal string WebServiceURL
        {
            get
            {
                if (isStatic)
                {
                    return static_ws_url;
                }
                return instance_ws_url;
            }
        }

	    internal string TargetUrl
	    {
            get
            {
                if (IsPost)
                {
                    return string.Empty;
                }
                return Data;
            }
	    }
    }
}
