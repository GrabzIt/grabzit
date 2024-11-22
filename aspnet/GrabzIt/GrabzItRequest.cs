using GrabzIt.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrabzIt
{
    internal class GrabzItRequest
    {
        private BaseOptions instance_options;
        private bool instance_post;
        private string instance_ws_url;
        private string instance_data;

        internal GrabzItRequest()
        {
        }

        internal void Store(string wsUrl, bool post, BaseOptions options)
        {
            Store(wsUrl, post, options, null);
        }

        internal void Store(string wsUrl, bool post, BaseOptions options, string data)
        {
            instance_ws_url = wsUrl;
            instance_post = post;
            instance_options = options;
            instance_data = data;
        }

        internal BaseOptions Options
        {
            get
            {
                return instance_options;
            }
            set
            {
                instance_options = value;
            }
        }

        internal bool IsPost
        {
            get
            {
                return instance_post;
            }
        }

        internal string Data
        {
            get
            {
                return instance_data;
            }
        }

        internal string WebServiceURL
        {
            get
            {
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
