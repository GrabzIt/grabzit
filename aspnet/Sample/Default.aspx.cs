using System;
using System.Configuration;
using System.Drawing;
using System.Web;
using GrabzIt;

namespace Sample
{
    public partial class _Default : System.Web.UI.Page
    {
        private GrabzItClient grabzItClient = GrabzItClient.Create(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {            
            grabzItClient.ScreenShotComplete += grabzIt_ScreenShotComplete;
            grabzItClient.TakePicture(txtURL.Text, HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "GrabzIt.ashx");
        }

        protected void grabzIt_ScreenShotComplete(object sender, ScreenShotEventArgs result)
        {
            Image image = grabzItClient.GetPicture(result.ID);
            //Ensure that the application has the correct rights for this directory.
            image.Save(Server.MapPath("~/screenshots/"+result.Filename));
        }
    }
}
