/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.servlets;

import it.grabz.grabzit.GrabzItClient;
import java.io.IOException;
import java.net.URLEncoder;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 *
 * @author GrabzIt
 */
public class TakeScreenshot extends HttpServlet{
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) throws IOException
    {
        String url = request.getParameter("url");
        String format = request.getParameter("format");
        try
        {
            GrabzItClient client = new GrabzItClient(Config.getApplicationKey(), Config.getApplicationSecret());
            if (format.equals("pdf"))
            {
                client.SetPDFOptions(url);
            }
            else
            {
                client.SetImageOptions(url);
            }
            client.Save(Config.getHandlerUrl());
        }
        catch(Exception ex)
        {
            response.sendRedirect("/grabzit/?error=" + URLEncoder.encode(ex.getMessage(), "UTF-8"));
            return;
        }
        response.sendRedirect("/grabzit/?message=" + URLEncoder.encode("Processing screenshot.", "UTF-8"));
    }
}
