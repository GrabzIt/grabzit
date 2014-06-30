/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.servlets;

import it.grabz.grabzit.GrabzItClient;
import it.grabz.grabzit.GrabzItFile;
import java.io.File;
import java.io.IOException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 *
 * @author GrabzIt
 */
public class Handler extends HttpServlet{
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException
    {
        String message = request.getParameter("message");
        String customId = request.getParameter("customid");
        String id = request.getParameter("id");
        String filename = request.getParameter("filename");
        String format = request.getParameter("format");

        GrabzItClient client = new GrabzItClient(Config.getApplicationKey(), Config.getApplicationSecret());

        GrabzItFile file = client.GetResult(id);

        if (file == null)
        {
            return;
        }

        String path = getServletContext().getRealPath("/results") + File.separator + filename;

        try
        {
            file.Save(path);
        }
        catch(Exception ex)
        {
            //You should log any errors
        }
    }
}
