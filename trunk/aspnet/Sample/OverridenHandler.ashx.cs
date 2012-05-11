using System.Configuration;
using System.Drawing;
using System.Web;
using GrabzIt;

namespace Sample
{
    public class OverridenHandler : Handler
    {
        protected override void Process(HttpContext context, string filename, string id, string message, string customId)
        {
            GrabzItClient grabzItClient = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);
            Image image = grabzItClient.GetPicture(id);
            //Ensure that the application has the correct rights for this directory.
            image.Save(context.Server.MapPath("~/screenshots/" + filename));
        }
    }
}
