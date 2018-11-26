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
            pnlCallbackWarning.Visible = !UseCallbackHandler;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {                
                if (ddlFormat.SelectedValue == "jpg")
                {
                    if (ddlConvert.SelectedValue == "html")
                    {
                        grabzItClient.HTMLToImage(txtHTML.Text);
                    }
                    else
                    {
                        grabzItClient.URLToImage(txtURL.Text);
                    }
                }
                else if (ddlFormat.SelectedValue == "docx")
                {
                    if (ddlConvert.SelectedValue == "html")
                    {
                        grabzItClient.HTMLToDOCX(txtHTML.Text);
                    }
                    else
                    {
                        grabzItClient.URLToDOCX(txtURL.Text);
                    }
                }
                else if (ddlFormat.SelectedValue == "csv")
                {
                    if (ddlConvert.SelectedValue == "html")
                    {
                        grabzItClient.HTMLToTable(txtHTML.Text);
                    }
                    else
                    {
                        grabzItClient.URLToTable(txtURL.Text);
                    }
                }
                else if (ddlFormat.SelectedValue == "gif")
                {
                    grabzItClient.URLToAnimation(txtURL.Text);
                }
                else
                {
                    if (ddlConvert.SelectedValue == "html")
                    {
                        grabzItClient.HTMLToPDF(txtHTML.Text);
                    }
                    else
                    {
                        grabzItClient.URLToPDF(txtURL.Text);
                    }
                }
                if (UseCallbackHandler)
                {
                    grabzItClient.Save(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "handler.ashx");
                    lblMessage.Text = "Processing...";
                    lblMessage.CssClass = string.Empty;
                    lblMessage.Style.Add("color", "green");
                    lblMessage.Style.Add("font-weight", "bold");
                }
                else
                {
                    grabzItClient.SaveTo(Server.MapPath("~/results/" + Guid.NewGuid().ToString() + "." + ddlFormat.SelectedValue));
                }
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

        public bool UseCallbackHandler
        {
            get
            {
                return !HttpContext.Current.Request.IsLocal;
            }
        }
    }
}
