/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;

/**
 *
 * @author GrabzIt
 */
class HttpUtility {
       
    public static URLConnection OpenConnection(URL url) throws IOException, Exception
    {
        URLConnection urlConnection = (URLConnection) url.openConnection();
        HttpURLConnection httpConnection = (HttpURLConnection) urlConnection;
        
        if (httpConnection.getResponseCode() == 403)
        {
            throw new Exception("Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.");
        }
        
        return urlConnection;
    }

}
