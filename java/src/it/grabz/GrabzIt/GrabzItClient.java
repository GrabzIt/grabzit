/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.GrabzIt;

import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.net.URL;
import java.net.URLConnection;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.List;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;

/**
 *
 * @author GrabzIt
 */
public class GrabzItClient {
    private String applicationKey;
    private String applicationSecret;
    
    private final String BASE_URL = "http://grabz.it/services/";
    
    public GrabzItClient(String applicationKey, String applicationSecret)
    {
        this.applicationKey = applicationKey;
        this.applicationSecret = applicationSecret;
    }
    
    /**
     * Get the current status of a GrabzIt screenshot
     * @param id The id of the screenshot
     * @return A {@link it.grabz.GrabzIt.Screenshots.Status} object representing the screenshot
     * @throws IOException
     * @throws JAXBException 
     */
    public Status GetStatus(String id) throws IOException, JAXBException
    {
        return get(BASE_URL + "getstatus.ashx?id=" + id, Status.class);
    }
    
    public List<Cookie> GetCookies(String domain) throws Exception
    {
        String sig = encrypt(String.format("%s|%s", this.applicationSecret, domain));

        String url = String.format("%sgetcookies.ashx?domain=%s&key=%s&sig=%s",
                                                  BASE_URL, domain, applicationKey, sig);
        Cookies cookies = get(url, Cookies.class);

        if (cookies.getMessage() != null && !cookies.getMessage().isEmpty())
        {
            throw new Exception(cookies.getMessage());
        }

        System.out.println("Hello " + cookies.getCookies().get(0).getName());
        
        return cookies.getCookies();
    }    
    
    private <T> T get(String url, Class<T> clazz) throws IOException, JAXBException
    {
        URL request = new URL(url);
        URLConnection connection = request.openConnection();
        InputStream in = null;
        
        try
        {
            in = connection.getInputStream();
            
            JAXBContext context = JAXBContext.newInstance(clazz);
            Unmarshaller unmarshaller = context.createUnmarshaller();
            return (T) unmarshaller.unmarshal(in);
        }
        finally
        {
            if (in != null)
            {
                in.close();
            }
        }    
    }
    
    private String encrypt(String value) throws UnsupportedEncodingException, NoSuchAlgorithmException
    {
        if (value == null || value.isEmpty())
        {
            return "";
        }
        MessageDigest md = MessageDigest.getInstance("MD5");
        md.update(value.getBytes("ASCII"));

        byte[] hash = md.digest();
        
        StringBuilder sb = new StringBuilder(hash.length); 
        for(byte b : hash)
        { 
            sb.append(String.format("%02x", b&0xff)); 
        }

        return sb.toString();
    }
}
