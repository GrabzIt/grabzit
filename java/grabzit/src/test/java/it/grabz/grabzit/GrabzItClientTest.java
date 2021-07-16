/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.HorizontalPosition;
import it.grabz.grabzit.enums.VerticalPosition;
import it.grabz.grabzit.parameters.ImageOptions;
import it.grabz.grabzit.parameters.PDFOptions;
import java.io.File;
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
    private final String WaterMark_Path = "watermark.png";
    private final String HTML_Path = "test.html";
    private final String Cookie_Name = "test_cookie";
    private final String Cookie_Domain = ".example.com";
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
    }
    
    @After
    public void tearDown() {
    }

    @Test
    public void testApplicationKey()
    {
        Assert.assertNotNull("Please set your application key in the constructor", applicationKey);
    }

    @Test
    public void testApplicationSecret()
    {
        Assert.assertNotNull("Please set your application secret in the constructor", applicationSecret);
    }
    
    @Test
    public void testNullValues()
    {
        try
        {
            ImageOptions options = new ImageOptions();
            options.setBrowserHeight(-1);
            options.setBrowserWidth(1024);
            options.setOutputHeight(-1);
            options.setOutputWidth(-1);
            options.setTargetElement(null);
            options.setCustomWaterMarkId(null);
            client.URLToImage("http://ca.linkedin.com/pub/theresa-bradshaw/9b/742/820", options);
            Assert.assertTrue("Null check passed", true);
        }
        catch(Exception ex)
        {
            Assert.fail("An null exception occured, message: " + ex.getMessage());
        }
    }

    @Test
    public void testTakeDOCX() throws IOException, JAXBException, Exception
    {
        try
        {
            client.URLToDOCX("http://www.google.com");
            Assert.assertNotNull("Failed to take screenshot using URLToDOCX method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a DOCX screenshot: " + ex.getMessage());
        }
    }        
    
    @Test
    public void testTakePDF() throws IOException, JAXBException, Exception
    {
        try
        {
            client.URLToPDF("http://www.google.com");
            Assert.assertNotNull("Failed to take screenshot using URLToPDF method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a PDF screenshot: " + ex.getMessage());
        }
    }    

    @Test
    public void testTakePDFHidePopup() throws IOException, JAXBException, Exception
    {
        try
        {
            PDFOptions options = new PDFOptions();
            options.setHideElement(".ArevicoModal-bg,.ArevicoModal");
            options.setDelay(30000);            
            client.URLToPDF("http://www.itnews24hrs.com/2016/05/turnkey-lender/", options);
            Assert.assertNotNull("Failed to take hide popups using URLToPDF method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a PDF screenshot: " + ex.getMessage());
        }
    }        
    
    @Test
    public void testTakeImage() throws IOException, JAXBException, Exception
    {
        try
        {
            client.URLToImage("http://www.google.com");
            Assert.assertNotNull("Failed to take screenshot using URLToImage method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a image screenshot: " + ex.getMessage());
        }
    }
    
    @Test
    public void testTakeImageHidePopup() throws IOException, JAXBException, Exception
    {
        try
        {
            ImageOptions options = new ImageOptions();
            options.setHideElement(".ArevicoModal-bg,.ArevicoModal");
            options.setDelay(30000);            
            client.URLToImage("http://www.itnews24hrs.com/2016/05/turnkey-lender/", options);
            Assert.assertNotNull("Failed to take hide popups using URLToImage method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a image screenshot: " + ex.getMessage());
        }
    }    
    
    @Test
    public void testHTMLToImage() throws IOException, JAXBException, Exception
    {
        try
        {
            client.HTMLToImage("<h1>Hello World</h1>");
            Assert.assertNotNull("Failed to take screenshot using HTMLToImage method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a create a image: " + ex.getMessage());
        }
    }    
    
    @Test
    public void testTakeAnimation() throws IOException, JAXBException, Exception
    {
        try
        {
            client.URLToAnimation("https://www.youtube.com/watch?v=k85mRPqvMbE");
            Assert.assertNotNull("Failed to take animation using URLToAnimation method", client.Save());
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to create a animated gif: " + ex.getMessage());
        }
    }   

    @Test
    public void testSave() throws IOException, JAXBException, Exception
    {
        String screenshotPath = getResourcePath(WaterMark_Path);
        screenshotPath = screenshotPath.replace(WaterMark_Path, "test.jpg");
        File file = new File(screenshotPath);
        if (file.exists())
        {
            file.delete();
        }
        try
        {
            client.URLToImage("http://www.google.com");
            Assert.assertEquals("Screenshot not saved", client.SaveTo(screenshotPath), true);
            file = new File(screenshotPath);
            Assert.assertEquals("Not saved screenshot file", file.exists(), true);
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a image screenshot: " + ex.getMessage());
        }
    }
    
    @Test
    public void testSaveObject() throws IOException, JAXBException, Exception
    {
        try
        {
            client.URLToImage("http://www.google.com");
            GrabzItFile grabzItFile = client.SaveTo();
            Assert.assertNotNull("Screenshot object not returned", grabzItFile);
            Assert.assertNotSame("Screenshot not saved", grabzItFile.getBytes().length, 0);
        }
        catch(Exception ex)
        {
            Assert.fail("An error occured when trying to take a image screenshot: " + ex.getMessage());
        }
    }
 

    @Test
    public void testStatus() throws IOException, JAXBException, Exception 
    {
        client.URLToImage("http://www.google.com");
        String id = client.Save();
        Status status = client.GetStatus(id);
        Assert.assertEquals("Failed to get correct screenshot status!", true, (status.isProcessing() || status.isCached()));              
    }

    @Test
    public void testAddCookie() throws IOException, JAXBException, Exception {
        if (isSubscribedAccount)
        {
            client.SetCookie(Cookie_Name, Cookie_Domain);
        }
        else
        {
            try
            {
                client.SetCookie(Cookie_Name, Cookie_Domain);
                Assert.fail("User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method");
            }
            catch(Exception ex)
            {
                return;
            }
        }

        Thread.sleep(4000);
        
        Assert.assertEquals("Set cookie has not been found!", findCookie(), true);
    }

    @Test
    public void testDeleteCookie() throws IOException, JAXBException, Exception {
        if (isSubscribedAccount)
        {
            client.SetCookie(Cookie_Name, Cookie_Domain);
        }
        else
        {
            try
            {
                client.SetCookie(Cookie_Name, Cookie_Domain);
                Assert.fail("User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method");
            }
            catch(Exception ex)
            {
                return;
            }
        }
        
        Thread.sleep(4000);
        
        Assert.assertEquals("Test cookie not found!", findCookie(), true);

        client.DeleteCookie(Cookie_Name, Cookie_Domain);
        
        Thread.sleep(4000);

        Assert.assertEquals("Failed to delete cookie!", findCookie(), false);
    }

    @Test
    public void testDeleteWaterMark() throws IOException, JAXBException, Exception {
        try
        {
            client.DeleteWaterMark(WaterMark_Identifier);
        }
        catch(Exception ex)
        {
        }
        if (isSubscribedAccount)
        {
            client.AddWaterMark(WaterMark_Identifier, getResourcePath(WaterMark_Path), HorizontalPosition.CENTER, VerticalPosition.MIDDLE);
        }
        else
        {
            try
            {
                client.AddWaterMark(WaterMark_Identifier, getResourcePath(WaterMark_Path), HorizontalPosition.CENTER, VerticalPosition.MIDDLE);
                Assert.fail("User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method");
            }
            catch(Exception ex)
            {
                return;
            }
        }

        Thread.sleep(4000);
        
        Assert.assertEquals("Test watermark not found!", findWaterMark(), true);

        client.DeleteWaterMark(WaterMark_Identifier);

        Thread.sleep(4000);
        
        Assert.assertEquals("Failed to delete watermark!", findWaterMark(), false);
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
            client.AddWaterMark(WaterMark_Identifier, getResourcePath(WaterMark_Path), HorizontalPosition.CENTER, VerticalPosition.MIDDLE);
        }
        else
        {
            try
            {
                client.AddWaterMark(WaterMark_Identifier, getResourcePath(WaterMark_Path), HorizontalPosition.CENTER, VerticalPosition.MIDDLE);
                Assert.fail("User not subscribed should throw error. If user is subscribed please set @isSubscribedAccount in the setup method");
            }
            catch(Exception ex)
            {                
                return;
            }
        }
        
        Thread.sleep(4000);
        
        Assert.assertEquals("Set watermark has not been found!", findWaterMark(), true);
    }    
    
    //@Test
    public void testDDOS() throws IOException, JAXBException, Exception {
        String message = "";
        try
        {
            for(int i = 0;i < 200;i++)
            {
                client.GetStatus("abcd");
            }
        }
        catch(Exception e)
        {
            message = e.getMessage();            
        }
        Assert.assertTrue("DDOS Detected", message.contains("DDOS"));
    }
            

    private boolean findCookie() throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        Cookie[] cookies = client.GetCookies(Cookie_Domain);

        for(Cookie cookie : cookies)
        {
            if (cookie.getName().equals(Cookie_Name))
            {
                return true;
            }
        }

        return false;
    }

    private boolean findWaterMark() throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        WaterMark waterMark = client.GetWaterMark(WaterMark_Identifier);

        return (waterMark != null);
    }

    private String getResourcePath(String item) {
        URL location = GrabzItClientTest.class.getProtectionDomain().getCodeSource().getLocation();
        String path = location.getFile();
        path = path.substring(1);
        return path + item;       
    }
}
