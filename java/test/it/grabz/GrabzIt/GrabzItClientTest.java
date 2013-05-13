/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.HorizontalPosition;
import it.grabz.grabzit.enums.VerticalPosition;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.URL;
import java.security.NoSuchAlgorithmException;
import javax.xml.bind.JAXBException;
import junit.framework.Assert;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

/**
 *
 * @author GrabzIt
 */
public class GrabzItClientTest {
    
    private String applicationKey = null;
    private String applicationSecret = null;
    private boolean isSubscribedAccount;
    private final String WaterMark_Identifier = "test_java_watermark";
    private final String WaterMark_Path = "test.png";
    private GrabzItClient client;
    
    public GrabzItClientTest() {
        applicationKey = "c3VwcG9ydEBncmFiei5pdA==";
        applicationSecret = "AD8/aT8/Pz8/Tz8/PwJ3Pz9sVSs/Pz8/Pz9DOzJodoi=";
        isSubscribedAccount = true;
        client = new GrabzItClient(applicationKey, applicationSecret);
    }
    
    @BeforeClass
    public static void setUpClass() {
    }
    
    @AfterClass
    public static void tearDownClass() {
    }
    
    @Before
    public void setUp() {
        Assert.assertNotNull(applicationKey, "Please set your application key in the constructor");
        Assert.assertNotNull(applicationSecret, "Please set your application secret in the constructor");
    }
    
    @After
    public void tearDown() {
    }

    @Test
    public void testStatus() throws IOException, JAXBException, Exception 
    {
        client.SetImageOptions("http://www.google.com");
        String id = client.Save();
        Status status = client.GetStatus(id);
        Assert.assertEquals("Failed to get correct screenshot status!", true, (status.isProcessing() || status.isCached()));              
    }
    
    @Test
    public void testAddWaterMark() throws IOException, JAXBException, Exception {
        try
        {
            client.DeleteWaterMark(WaterMark_Identifier);
        }
        catch(Exception ex)
        {            
        }
        if (isSubscribedAccount)
        {
            client.AddWaterMark(WaterMark_Identifier, getWaterMarkPath(), HorizontalPosition.CENTER, VerticalPosition.MIDDLE);
        }
        else
        {
            try
            {
                client.AddWaterMark(WaterMark_Identifier, getWaterMarkPath(), HorizontalPosition.CENTER, VerticalPosition.MIDDLE);
                Assert.fail("User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method");
            }
            catch(Exception ex)
            {                
                return;
            }
        }
        
        Assert.assertEquals("Set watermark has not been found!", findWaterMark(), true);
    }    
    
    private boolean findWaterMark() throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        WaterMark[] waterMarks = client.GetWaterMarks(WaterMark_Identifier);
        
        for(WaterMark mark : waterMarks)
        {
            if (mark.getIdentifier().equals(WaterMark_Identifier))
            {
                return true;
            }
        }
        
        return false;
    }

    private String getWaterMarkPath() {
        URL path = GrabzItClientTest.class.getResource(WaterMark_Path);
        return path.toString().replace("file:/", "");       
    }
}