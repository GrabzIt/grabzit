using System.Configuration;
using System.Drawing;
using System.Web;
using GrabzIt;
using GrabzIt.Screenshots;

namespace Sample
{
    public class OverridenHandler : Handler
    {
        protected override void Process(HttpContext context, string filename, string id, string message, string customId, string format)
        {
            GrabzItClient grabzItClient = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);
            GrabzItFile file = grabzItClient.GetResult(id);
            //Ensure that the application has the correct rights for this directory.
            file.Save(context.Server.MapPath("~/results/" + filename));
        }
    }
}
