/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.GrabzIt;

import java.io.IOException;
import java.util.List;
import javax.xml.bind.JAXBException;
import junit.framework.Assert;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author GrabzIt
 */
public class GrabzItClientTest {
    
    private String applicationKey = null;
    private String applicationSecret = null;
    private GrabzItClient client = new GrabzItClient("", "");
    
    public GrabzItClientTest() {
        applicationKey = "c3VwcG9ydEBncmFiei5pdA==";
        applicationSecret = "AD8/aT8/Pz8/Tz8/PwJ3Pz9sVSs/Pz8/Pz9DOzJodoi=";
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
    public void testStatus() throws IOException, JAXBException {
        Status status = client.GetStatus("test");        
        Assert.assertNotNull(status);
    }
    
    @Test
    public void testGetCookies() throws IOException, JAXBException, Exception {
        List<Cookie> cookies = client.GetCookies("space.com");        
        //Assert.assertNotNull(status);
    }    
}