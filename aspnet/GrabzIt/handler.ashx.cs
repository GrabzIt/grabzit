using System;
using System.Web;

namespace GrabzIt
{
    public class Handler : IHttpAsyncHandler
    {
        private AsyncProcessorDelegate WorkDelegate;
        protected delegate void AsyncProcessorDelegate(HttpContext context);

        private const string ID = "id";
        private const string FILENAME = "filename";
        private const string MESSAGE = "message";
        private const string CUSTOMID = "customid";
        private const string FORMAT = "format";

        /// <summary>
        /// Process the parameters returned to the callback handler
        /// </summary>
        /// <param name="context">The request context</param>
        public void ProcessRequest(HttpContext context)
        {
            string id = string.Empty;
            string filename = string.Empty;
            string message = string.Empty;
            string customId = string.Empty;
            string format = string.Empty;
            if (context.Request.QueryString[ID] != null)
            {
                id = HttpUtility.UrlDecode(context.Request.QueryString[ID]);
            }
            if (context.Request.QueryString[FILENAME] != null)
            {
                filename = HttpUtility.UrlDecode(context.Request.QueryString[FILENAME]);
            }
            if (context.Request.QueryString[MESSAGE] != null)
            {
                message = HttpUtility.UrlDecode(context.Request.QueryString[MESSAGE]);
            }
            if (context.Request.QueryString[CUSTOMID] != null)
            {
                customId = HttpUtility.UrlDecode(context.Request.QueryString[CUSTOMID]);
            }
            if (context.Request.QueryString[FORMAT] != null)
            {
                format = HttpUtility.UrlDecode(context.Request.QueryString[FORMAT]);
            }

            Process(context, filename, id, message, customId, format);
        }

        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            WorkDelegate = new AsyncProcessorDelegate(ProcessRequest);
            return WorkDelegate.BeginInvoke(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            WorkDelegate.EndInvoke(result);
        }

        #endregion

        /// <summary>
        /// This method allows this handler to be overriden and the result processed in a different way
        /// </summary>
        /// <param name="context">The current context of the request</param>
        /// <param name="filename">The filename of the screenshot</param>
        /// <param name="id">The unique identifier of the screenshot</param>
        /// <param name="message">Any message due to the processing</param>
        /// <param name="customId">Any custom id that was passed to the GrabzIt web service.</param>
        /// <param name="format">The format of the returned screenshot (e.g. PDF, JPG).</param>
        protected virtual void Process(HttpContext context, string filename, string id, string message, string customId, string format)
        {
            if (GrabzItClient.WebClient != null)
            {
                GrabzItClient.WebClient.OnScreenShotComplete(this, new ScreenShotEventArgs(filename, id, message, customId, format));
            }
        }

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}