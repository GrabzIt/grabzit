/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.ErrorCode;
import java.io.IOException;
import java.net.HttpURLConnection;
import java.net.URLConnection;

/**
 *
 * @author GrabzIt
 */
class HttpUtility {
       
    public static void CheckResponse(URLConnection urlConnection) throws IOException, Exception
    {        
        HttpURLConnection httpConnection = (HttpURLConnection) urlConnection;
        
        int statusCode = httpConnection.getResponseCode();
        if (statusCode == 403)
        {
            throw new GrabzItException("Rate limit reached. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.", ErrorCode.NETWORKDDOSATTACK);
        }
        else if (statusCode >= 400)
        {
            throw new GrabzItException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NETWORKGENERALERROR);
        }
    }

}
