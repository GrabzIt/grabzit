/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.sample.servlets;

import it.grabz.grabzit.GrabzItClient;
import java.io.File;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URISyntaxException;
import java.net.URL;
import java.net.URLEncoder;
import static java.util.UUID.randomUUID;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 *
 * @author GrabzIt
 */
public class TakeScreenshot extends HttpServlet{
    @Override
    public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        request.setAttribute("useCallbackHandler", useCallbackHandler(request));
        RequestDispatcher view=request.getRequestDispatcher("index.jsp");
        view.forward(request,response);        
    }    
    
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) throws IOException
    {
        String url = request.getParameter("url");
        String html = request.getParameter("html");
        String format = request.getParameter("format");
        boolean isHtml = request.getParameter("convert").equals("html");
        
        try
        {
            GrabzItClient client = new GrabzItClient(Config.getApplicationKey(), Config.getApplicationSecret());
            if (format.equals("pdf"))
            {
                if (isHtml)
                {
                    client.HTMLToPDF(html);
                }
                else
                {                    
                    client.URLToPDF(url);
                }
            }
            else if (format.equals("docx"))
            {
                if (isHtml)
                {
                    client.HTMLToDOCX(html);
                }
                else
                {                    
                    client.URLToDOCX(url);
                }   
            }
            else if (format.equals("csv"))
            {
                if (isHtml)
                {
                    client.HTMLToTable(html);
                }
                else
                {                    
                    client.URLToTable(url);
                }   
            }
            else if (format.equals("gif"))
            {
                client.URLToAnimation(url);
            }
            else
            {
                if (isHtml)
                {
                    client.HTMLToImage(html);
                }
                else
                {                    
                    client.URLToImage(url);
                }
            }
            if (useCallbackHandler(request))
            {
                client.Save(Config.getHandlerUrl());
                response.sendRedirect("/grabzit?message=" + URLEncoder.encode("Processing...", "UTF-8"));
            }
            else
            {
                client.SaveTo(getServletContext().getRealPath("/results") + File.separator + randomUUID() + "." + format);
                response.sendRedirect("/grabzit");
            }
        }
        catch(Exception ex)
        {
            response.sendRedirect("/grabzit?error=" + URLEncoder.encode(ex.getMessage(), "UTF-8"));
        }        
    }
    
    private boolean useCallbackHandler(HttpServletRequest request)
    {
        URL u;

        try {  
            u = new URL(Config.getHandlerUrl());  
        } catch (MalformedURLException e) {  
            return false;  
        }

        try {  
            u.toURI();  
        } catch (URISyntaxException e) {  
            return false;  
        }       
        
        return !"0:0:0:0:0:0:0:1".equals(request.getLocalAddr()) && !"127.0.0.1".equals(request.getLocalAddr());
    }
}
