using System.Web;

namespace GrabzIt
{
    public class Handler : IHttpHandler
    {
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
            if (context.Request.QueryString["id"] != null)
            {
                id = HttpUtility.UrlDecode(context.Request.QueryString["id"]);
            }
            if (context.Request.QueryString["filename"] != null)
            {
                filename = HttpUtility.UrlDecode(context.Request.QueryString["filename"]);
            }
            if (context.Request.QueryString["message"] != null)
            {
                message = HttpUtility.UrlDecode(context.Request.QueryString["message"]);
            }
            if (context.Request.QueryString["customid"] != null)
            {
                customId = HttpUtility.UrlDecode(context.Request.QueryString["customid"]);
            }

            Process(context, filename, id, message, customId);
        }

        /// <summary>
        /// This method allows this handler to be overriden and the result processed in a different way
        /// </summary>
        /// <param name="context">The current context of the request</param>
        /// <param name="filename">The filename of the screenshot</param>
        /// <param name="id">The unique identifier of the screenshot</param>
        /// <param name="message">Any message due to the processing</param>
        /// <param name="customId">Any custom id that was passed to the GrabzIt web service.</param>
        protected virtual void Process(HttpContext context, string filename, string id, string message, string customId)
        {
            if (GrabzItClient.WebClient != null)
            {
                GrabzItClient.WebClient.OnScreenShotComplete(this, new ScreenShotEventArgs(filename, id, message, customId));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}