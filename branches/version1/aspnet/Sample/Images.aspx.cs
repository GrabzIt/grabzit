using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Sample
{
    public partial class Images : System.Web.UI.Page
    {
        private const string SCREENSHOT_PATH = "~/screenshots";

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Server.MapPath(SCREENSHOT_PATH));
            foreach(string file in files)
            {
                if (!file.Contains(".txt"))
                {
                    Image image = new Image();
                    FileInfo info = new FileInfo(file);
                    image.ImageUrl = string.Concat(SCREENSHOT_PATH, "/", info.Name);
                    image.Style.Add("margin-right", "1em");
                    plImages.Controls.Add(image);
                }
            }
        }
    }
}
