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

        protected void btnResend_Click(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            GrabzItScrapeClient client = new GrabzItScrapeClient(ConfigurationManager.AppSettings["ApplicationKey"], ConfigurationManager.AppSettings["ApplicationSecret"]);
            GrabzItScrape scrape = client.GetScrape(button.CommandArgument);
            if (scrape != null)
            {
                client.SendResult(button.CommandArgument, scrape.Results[0].ID);
                lblMessage.Text = "Successfully requested result to be re-sent!";
            }
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

        public bool ShowResentButton(GrabzItScrape obj)
        {
            if (obj != null && obj.Results != null)
            {
                return obj.Results.Length > 0;
            }
            return false;
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