using GrabzIt.Scraper;
using GrabzIt.Scraper.Results;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sample
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowScrapes();
        }

        private void ShowScrapes()
        {
            GrabzItScrapeClient client = new GrabzItScrapeClient(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);
            grdScrapes.DataSource = client.GetScrapes();
            grdScrapes.DataBind();
        }

        protected void btnStart_Click(object sender, System.EventArgs e)
        {
            SetScrapeStatus(sender, GrabzIt.Scraper.Enums.ScrapeStatus.Start);
        }

        protected void btnStop_Click(object sender, System.EventArgs e)
        {
            SetScrapeStatus(sender, GrabzIt.Scraper.Enums.ScrapeStatus.Stop);
        }

        protected void btnEnable_Click(object sender, System.EventArgs e)
        {
            SetScrapeStatus(sender, GrabzIt.Scraper.Enums.ScrapeStatus.Enable);
        }

        protected void btnDisable_Click(object sender, System.EventArgs e)
        {
            SetScrapeStatus(sender, GrabzIt.Scraper.Enums.ScrapeStatus.Disable);
        }

        private void SetScrapeStatus(object sender, GrabzIt.Scraper.Enums.ScrapeStatus status)
        {
            lblMessage.Text = string.Empty;
            lblError.Text = string.Empty;
            try
            {
                Button button = (Button)sender;
                GrabzItScrapeClient client = new GrabzItScrapeClient(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);
                if (client.SetScrapeStatus(button.CommandArgument, status))
                {
                    lblMessage.Text = "Successfully updated scrape!";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            ShowScrapes();
        }
    }
}