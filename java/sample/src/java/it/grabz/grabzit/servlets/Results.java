/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.servlets;

import java.io.IOException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 *
 * @author Skinners
 */
public class Results extends HttpServlet{
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException
    {
        response.setStatus(HttpServletResponse.SC_OK);
        response.getWriter().write("Hello");
        response.getWriter().flush();
        response.getWriter().close();
    }
}
