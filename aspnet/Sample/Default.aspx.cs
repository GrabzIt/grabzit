using System;
using System.Configuration;
using System.Web;
using System.IO;
using GrabzIt;
using GrabzIt.Screenshots;

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
            try
            {
                grabzItClient.ScreenShotComplete += grabzIt_ScreenShotComplete;
                if (ddlFormat.SelectedValue == "jpg")
                {
                    grabzItClient.SetImageOptions(txtURL.Text);
                }
                else if (ddlFormat.SelectedValue == "gif")
                {
                    grabzItClient.SetAnimationOptions(txtURL.Text);
                }
                else
                {
                    grabzItClient.SetPDFOptions(txtURL.Text);
                }
                grabzItClient.Save(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "GrabzIt.ashx");
                lblMessage.Text = "Processing...";
                lblMessage.CssClass = string.Empty;
                lblMessage.Style.Add("color", "green");
                lblMessage.Style.Add("font-weight", "bold");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.Style.Clear();
                lblMessage.CssClass = "error";
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Server.MapPath("~/results"));
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        protected void grabzIt_ScreenShotComplete(object sender, ScreenShotEventArgs result)
        {
            if (!string.IsNullOrEmpty(result.Message))
            {
                lblMessage.Text = result.Message;
                lblMessage.Style.Clear();
                lblMessage.CssClass = "error";
            }
            GrabzItFile file = grabzItClient.GetResult(result.ID);
            //Ensure that the application has the correct rights for this directory.
            file.Save(Server.MapPath("~/results/" + result.Filename));
        }
    }
}
