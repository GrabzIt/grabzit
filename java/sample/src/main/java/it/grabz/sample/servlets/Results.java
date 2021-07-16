/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.servlets;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 *
 * @author GrabzIt
 */
public class Results extends HttpServlet{
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException
    {
        String path = getServletContext().getRealPath("/results");
        File folder = new File(path);
        File[] listOfFiles = folder.listFiles();
        List<String> results = new ArrayList<String>();
        response.setStatus(HttpServletResponse.SC_OK);
        response.setContentType("application/json");
        response.getWriter().write("[");
        boolean multipleFiles = false;
        for (int i = 0; i < listOfFiles.length; i++)
        {
            if (listOfFiles[i].isFile() && !listOfFiles[i].getName().contains(".txt"))
            {
                if (multipleFiles)
                {
                    response.getWriter().write(",");
                }
                response.getWriter().write("\"grabzit/results/");
                response.getWriter().write(listOfFiles[i].getName());
                response.getWriter().write("\"");
                multipleFiles = true;
            }
        }        
        response.getWriter().write("]");
        response.getWriter().flush();
        response.getWriter().close();
    }
}
