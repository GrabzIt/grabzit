/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.ErrorCode;
import it.grabz.grabzit.enums.HorizontalPosition;
import it.grabz.grabzit.enums.VerticalPosition;
import it.grabz.grabzit.parameters.AnimationOptions;
import it.grabz.grabzit.parameters.DOCXOptions;
import it.grabz.grabzit.parameters.HTMLOptions;
import it.grabz.grabzit.parameters.ImageOptions;
import it.grabz.grabzit.parameters.PDFOptions;
import it.grabz.grabzit.parameters.ParameterUtility;
import it.grabz.grabzit.parameters.TableOptions;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import java.net.Authenticator;
import java.net.InetSocketAddress;
import java.net.MalformedURLException;
import java.net.Proxy;
import java.net.URL;
import java.net.URLConnection;
import java.net.UnknownHostException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Arrays;
import java.util.Date;
import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import javax.xml.bind.DatatypeConverter;
import javax.xml.bind.JAXBException;

/**
 * This client provides access to the GrabzIt web services
 * This API allows you to take capture websites for free and convert them into images, PDF's and tables.
 *
 * @version 3.0
 * @author GrabzIt
 * @see <a href="https://grabz.it/api/java/">GrabzIt Java API</a>
 */
public class GrabzItClient {
    private final String applicationKey;
    private final String applicationSecret;

    private Request request;
    private Proxy proxy = null;
    private String protocol = "http";
    
    private final String BASE_URL = "://api.grabz.it/services/";
    private final String TAKE_DOCX = "takedocx";
    private final String TAKE_PDF = "takepdf";
    private final String TAKE_TABLE = "taketable";
    private final String TAKE_PICTURE = "takepicture";
    private final String TAKE_HTML = "takehtml";
    
    /**
     * Create a new instance of the Client class in order to access the GrabzIt API.
     *
     * @see <a href="https://grabz.it/register.aspx">You can get an application key and secret by registering for free with GrabzIt</a>
     * @param applicationKey your application key
     * @param applicationSecret your application secret
     */
    public GrabzItClient(String applicationKey, String applicationSecret)
    {
        this.applicationKey = applicationKey;
        this.applicationSecret = applicationSecret;
    }
    
    /**
     * This method enables a local proxy server to be used for all requests
     * @param proxyUrl the URL, which can include a port if required, of the proxy. Providing a null will remove any previously set proxy
     * 
     * @throws MalformedURLException 
     */
    public void SetLocalProxy(String proxyUrl) throws MalformedURLException{
        Authenticator.setDefault(null);
        
        if (proxyUrl == null || "".equals(proxyUrl))
        {
            this.proxy = null;
            return;
        }
        
        URL url = new URL(proxyUrl);            
        this.proxy = new Proxy(Proxy.Type.HTTP, new InetSocketAddress(url.getHost(), url.getPort()));        

        if (url.getUserInfo() != null)
        {
            String[] userInfoParts = url.getUserInfo().split(":");
            if (userInfoParts.length == 2)
            {
                Authenticator.setDefault(new BasicAuthenticator(userInfoParts[0], userInfoParts[1]));
            }
        }
    }
    
    /**
     * This method sets if requests to GrabzIt's API should use SSL or not
     * @param value true if should use SSL
     */
    public void UseSSL(boolean value)
    {
        if (value)
        {
            this.protocol = "https";
            return;
        }
        this.protocol = "http";
    }
    
    /**
     * This method creates an encryption key to pass to the setEncryptionKey method.
     * @return The encryption key
     */
    public String CreateEncryptionKey()
    {
        SecureRandom random = new SecureRandom();
        byte[] bytes = new byte[32];
        random.nextBytes(bytes);
        return DatatypeConverter.printBase64Binary(bytes);
    }
    
    /**
     * This method will decrypt a encrypted capture, using the key you passed to the setEncryptionKey method.
     * @param file The encrypted GrabzItFile
     * @param key The encryption key
     * @return The decrypted GrabzItFile
     * @throws NoSuchAlgorithmException
     * @throws NoSuchPaddingException
     * @throws InvalidKeyException
     * @throws IllegalBlockSizeException
     * @throws BadPaddingException
     * @throws InvalidAlgorithmParameterException 
     */
    public GrabzItFile Decrypt(GrabzItFile file, String key) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException, InvalidAlgorithmParameterException
    {
        if (file == null)
        {
            return null;
        }
        return new GrabzItFile(Decrypt(file.getBytes(), key));
    }

    /**
     * This method will decrypt a encrypted capture, using the key you passed to the setEncryptionKey method.
     * @param path The path of the encrypted capture
     * @param key The encryption key
     * @throws NoSuchAlgorithmException
     * @throws NoSuchPaddingException
     * @throws InvalidKeyException
     * @throws IllegalBlockSizeException
     * @throws BadPaddingException
     * @throws InvalidAlgorithmParameterException
     * @throws GrabzItException
     * @throws IOException 
     */
    public void Decrypt(String path, String key) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException, InvalidAlgorithmParameterException, GrabzItException, IOException
    {
        byte[] data = fileToBytes(path);
        FileUtility.Save(path, Decrypt(data, key));
    }
    
    /**
     * This method will decrypt a encrypted capture, using the key you passed to the setEncryptionKey method.
     * @param data The encrypted bytes
     * @param key The encryption key
     * @return The decrypted bytes
     * @throws NoSuchAlgorithmException
     * @throws NoSuchPaddingException
     * @throws InvalidKeyException
     * @throws IllegalBlockSizeException
     * @throws BadPaddingException
     * @throws InvalidAlgorithmParameterException 
     */
    public byte[] Decrypt(byte[] data, String key) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException, InvalidAlgorithmParameterException
    {
        if (data == null)
        {
            return null;
        }
        byte[] iv = Arrays.copyOfRange(data, 0, 16);
        byte[] payload = Arrays.copyOfRange(data, 16, data.length);
                
        IvParameterSpec ivSpec = new IvParameterSpec(iv);
        SecretKeySpec keySpec = new SecretKeySpec(DatatypeConverter.parseBase64Binary(key), "AES");
        
        Cipher cipher = Cipher.getInstance("AES/CBC/NoPadding");
        cipher.init(Cipher.DECRYPT_MODE, keySpec, ivSpec);

        return cipher.doFinal(payload);
    }
    
    /**
     * This method specifies the URL of the online video that should be converted into a animated GIF.
     * @param url The URL to convert into a animated GIF.
     * @param options A instance of the AnimationOptions class that defines any special options to use when creating the animated GIF.
     * @throws UnsupportedEncodingException
     */
    public void URLToAnimation(String url, AnimationOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new AnimationOptions();
        }
        this.request = new Request(getRootURL() + "takeanimation", false, options, url);        
    }   
    
    /**
     * This method specifies the URL of the online video that should be converted into a animated GIF.
     * @param url The URL to convert into a animated GIF.
     * @throws UnsupportedEncodingException
     */
    public void URLToAnimation(String url) throws UnsupportedEncodingException
    {
        URLToAnimation(url, null);
    }    
    
    /**
     * This method specifies the URL that should be converted into a image screenshot.
     * @param url The URL to capture as a screenshot.
     * @param options A instance of the ImageOptions class that defines any special options to use when creating the screenshot.
     * @throws UnsupportedEncodingException
     */
    public void URLToImage(String url, ImageOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new ImageOptions();
        }
        this.request = new Request(getRootURL() + TAKE_PICTURE, false, options, url);        
    }
    
    /**
     * This method specifies the URL that should be converted into a image screenshot.
     * @param url The URL to capture as a screenshot.
     * @throws UnsupportedEncodingException
     */
    public void URLToImage(String url) throws UnsupportedEncodingException
    {
        URLToImage(url, null);
    }    
    
    /**
     * This method specifies the HTML that should be converted into a image.
     * @param html The HTML to convert into a image.
     * @param options A instance of the ImageOptions class that defines any special options to use when creating the image.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToImage(String html, ImageOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new ImageOptions();
        }
        this.request = new Request(getRootURL() + TAKE_PICTURE, true, options, html);
    }   
    
    /**
     * This method specifies the HTML that should be converted into a image.
     * @param html The HTML to convert into a image.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToImage(String html) throws UnsupportedEncodingException
    {
        HTMLToImage(html, null);
    }    
    
    /**
     * This method specifies a HTML file that should be converted into a image.
     * @param path The file path of a HTML file to convert into a image.
     * @param options A instance of the ImageOptions class that defines any special options to use when creating the image.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToImage(String path, ImageOptions options) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        this.HTMLToImage(fileToHTML(path), options);
    }        

    /**
     * This method specifies a HTML file that should be converted into a image.
     * @param path The file path of a HTML file to convert into a image.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToImage(String path) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        FileToImage(path, null);
    }

    /**
     * This method specifies the URL that should be converted into rendered HTML.
     * @param url The URL to capture as rendered HTML.
     * @param options A instance of the HTMLOptions class that defines any special options to use when creating rendered HTML.
     * @throws UnsupportedEncodingException
     */
    public void URLToRenderedHTML(String url, HTMLOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new HTMLOptions();
        }
        this.request = new Request(getRootURL() + TAKE_HTML, false, options, url);        
    }
    
    /**
     * This method specifies the URL that should be converted into rendered HTML.
     * @param url The URL to capture as rendered HTML.
     * @throws UnsupportedEncodingException
     */
    public void URLToRenderedHTML(String url) throws UnsupportedEncodingException
    {
        URLToRenderedHTML(url, null);
    }    
    
    /**
     * This method specifies the HTML that should be converted into rendered HTML.
     * @param html The HTML to convert into rendered HTML.
     * @param options A instance of the HTMLOptions class that defines any special options to use when creating the rendered HTML.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToRenderedHTML(String html, HTMLOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new HTMLOptions();
        }
        this.request = new Request(getRootURL() + TAKE_HTML, true, options, html);
    }   
    
    /**
     * This method specifies the HTML that should be converted into rendered HTML.
     * @param html The HTML to convert into rendered HTML.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToRenderedHTML(String html) throws UnsupportedEncodingException
    {
        HTMLToRenderedHTML(html, null);
    }    
    
    /**
     * This method specifies a HTML file that should be converted into rendered HTML.
     * @param path The file path of a HTML file to convert into rendered HTML.
     * @param options A instance of the ImageOptions class that defines any special options to use when creating the rendered HTML.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToRenderedHTML(String path, HTMLOptions options) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        this.HTMLToRenderedHTML(fileToHTML(path), options);
    }        

    /**
     * This method specifies a HTML file that should be converted into rendered HTML.
     * @param path The file path of a HTML file to convert into rendered HTML.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToRenderedHTML(String path) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        FileToRenderedHTML(path, null);
    }    
    
    /**
     * This method specifies the URL that the HTML tables should be extracted from.
     * @param url The URL that the should be used to extract tables
     * @param options A instance of the TableOptions class that defines any special options to use when converting the HTML table.
     * @throws UnsupportedEncodingException
     */
    public void URLToTable(String url, TableOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new TableOptions();
        }
        this.request = new Request(getRootURL() + TAKE_TABLE, false, options, url);        
    }    
    
    /**
     * This method specifies the URL that the HTML tables should be extracted from.
     * @param url The URL that the should be used to extract tables
     * @throws UnsupportedEncodingException
     */
    public void URLToTable(String url) throws UnsupportedEncodingException
    {
        URLToTable(url, null);
    }    
    
    /**
     * This method specifies the HTML that the HTML tables should be extracted from.
     * @param html The HTML to extract HTML tables from.
     * @param options A instance of the TableOptions class that defines any special options to use when converting the HTML table.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToTable(String html, TableOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new TableOptions();
        }
        this.request = new Request(getRootURL() + TAKE_TABLE, true, options, html);
    }    
    
    /**
     * This method specifies the HTML that the HTML tables should be extracted from.
     * @param html The HTML to extract HTML tables from.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToTable(String html) throws UnsupportedEncodingException
    {
        HTMLToTable(html, null);
    }    
    
    /**
     * This method specifies a HTML file that the HTML tables should be extracted from.
     * @param path The file path of a HTML file to extract HTML tables from.
     * @param options A instance of the TableOptions class that defines any special options to use when converting the HTML table.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToTable(String path, TableOptions options) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        this.HTMLToTable(fileToHTML(path), options);
    }    

    /**
     * This method specifies a HTML file that the HTML tables should be extracted from.
     * @param path The file path of a HTML file to extract HTML tables from.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToTable(String path) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        FileToTable(path, null);
    }    
    
    /**
     * This method specifies the URL that should be converted into a PDF.
     * @param url The URL that the should be converted into a PDF
     * @param options A instance of the PDFOptions class that defines any special options to use when creating the PDF.
     * @throws UnsupportedEncodingException
     */
    public void URLToPDF(String url, PDFOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new PDFOptions();
        }
        this.request = new Request(getRootURL() + TAKE_PDF, false, options, url);
    }
    
    /**
     * This method specifies the URL that should be converted into a PDF.
     * @param url The URL that the should be converted into a PDF
     * @throws UnsupportedEncodingException
     */
    public void URLToPDF(String url) throws UnsupportedEncodingException
    {
        URLToPDF(url, null);
    }    
    
    /**
     * This method specifies the HTML that should be converted into a PDF.
     * @param html The HTML to convert into a PDF.
     * @param options A instance of the PDFOptions class that defines any special options to use when creating the PDF.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToPDF(String html, PDFOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new PDFOptions();
        }
        this.request = new Request(getRootURL() + TAKE_PDF, true, options, html);
    }    

    /**
     * This method specifies the HTML that should be converted into a PDF.
     * @param html The HTML to convert into a PDF.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToPDF(String html) throws UnsupportedEncodingException
    {
        HTMLToPDF(html, null);
    }    
    
    /**
     * This method specifies a HTML file that should be converted into a PDF.
     * @param path The file path of a HTML file to convert into a PDF.
     * @param options A instance of the PDFOptions class that defines any special options to use when creating the PDF.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToPDF(String path, PDFOptions options) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        this.HTMLToPDF(fileToHTML(path), options);
    }       
    
    /**
     * This method specifies a HTML file that should be converted into a PDF.
     * @param path The file path of a HTML file to convert into a PDF.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToPDF(String path) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        this.FileToPDF(path, null);
    }    
    
    /**
     * This method specifies the URL that should be converted into a DOCX.
     * @param url The URL that the should be converted into a DOCX
     * @param options A instance of the DOCXOptions class that defines any special options to use when creating the DOCX.
     * @throws UnsupportedEncodingException
     */
    public void URLToDOCX(String url, DOCXOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new DOCXOptions();
        }
        this.request = new Request(getRootURL() + TAKE_DOCX, false, options, url);
    }
    
    /**
     * This method specifies the URL that should be converted into a DOCX.
     * @param url The URL that the should be converted into a DOCX
     * @throws UnsupportedEncodingException
     */
    public void URLToDOCX(String url) throws UnsupportedEncodingException
    {
        URLToDOCX(url, null);
    }    
    
    /**
     * This method specifies the HTML that should be converted into a DOCX.
     * @param html The HTML to convert into a DOCX.
     * @param options A instance of the DOCXOptions class that defines any special options to use when creating the DOCX.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToDOCX(String html, DOCXOptions options) throws UnsupportedEncodingException
    {
        if (options == null)
        {
            options = new DOCXOptions();
        }
        this.request = new Request(getRootURL() + TAKE_DOCX, true, options, html);
    }    

    /**
     * This method specifies the HTML that should be converted into a DOCX.
     * @param html The HTML to convert into a DOCX.
     * @throws UnsupportedEncodingException
     */
    public void HTMLToDOCX(String html) throws UnsupportedEncodingException
    {
        HTMLToDOCX(html, null);
    }    
    
    /**
     * This method specifies a HTML file that should be converted into a DOCX.
     * @param path The file path of a HTML file to convert into a DOCX.
     * @param options A instance of the DOCXOptions class that defines any special options to use when creating the DOCX.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToDOCX(String path, DOCXOptions options) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        this.HTMLToDOCX(fileToHTML(path), options);
    }       
    
    /**
     * This method specifies a HTML file that should be converted into a DOCX.
     * @param path The file path of a HTML file to convert into a DOCX.
     * @throws UnsupportedEncodingException
     * @throws GrabzItException
     * @throws FileNotFoundException
     * @throws IOException
     */
    public void FileToDOCX(String path) throws UnsupportedEncodingException, GrabzItException, FileNotFoundException, IOException
    {
        FileToDOCX(path, null);
    }            

    private byte[] fileToBytes(String path) throws GrabzItException, FileNotFoundException, IOException
    {
        File fileToConvert = new File(path);
        if(!fileToConvert.exists())
        {
            throw new GrabzItException("File: " + path + " does not exist", ErrorCode.FILENONEXISTANTPATH);
        }
        FileInputStream fis = null;

        byte[] data;
        
        try
        {
            fis = new FileInputStream(fileToConvert);
            data = new byte[(int) fileToConvert.length()];
            fis.read(data);
        }
        finally
        {
            if (fis != null)
            {
                fis.close();
            }
        }   
        
        return data;
    }
    
    private String fileToHTML(String path) throws IOException, GrabzItException {
        return new String(fileToBytes(path), "UTF-8");
    }       

    /**
     * Calls the GrabzIt web service to take the screenshot.
     * @param callBackURL The handler the GrabzIt web service should call after it has completed its work
     * @return The unique identifier of the screenshot. This can be used to get the screenshot with the GetResult method
     * @throws Exception
     */
    public String Save(String callBackURL) throws Exception
    {
        callBackURL = ParameterUtility.nullCheck(callBackURL);
        
        if (this.request == null)
        {
            throw new GrabzItException("No parameters have been set.", ErrorCode.PARAMETERMISSINGPARAMETERS);
        }
        
        String sig = encrypt(this.request.getOptions()._getSignatureString(applicationSecret, callBackURL, this.request.getTargetUrl()));
        
        TakeScreenShotResult result = take(sig, callBackURL);    
        
        if (result == null)
        {
            result = take(sig, callBackURL);
        }
        
        if (result == null)
        {
            throw new GrabzItException("An unknown network error occurred, please try calling this method again.", ErrorCode.NETWORKGENERALERROR);
        }        
        
        return result.getId();
    }

    private TakeScreenShotResult take(String sig, String callBackURL) throws Exception {
        TakeScreenShotResult result;
        if (this.request.isIsPost())
        {
            result = post(this.request.getUrl(), this.request.getOptions()._getQueryString(applicationKey, sig, callBackURL, "html", ParameterUtility.encode(this.request.getData())), TakeScreenShotResult.class);
        }
        else
        {
            result = get(this.request.getUrl() + "?" + this.request.getOptions()._getQueryString(applicationKey, sig, callBackURL, "url", this.request.getData()), TakeScreenShotResult.class);
        }
        checkForError(result);
        return result;
    }

    /**
     * Calls the GrabzIt web service to take the screenshot.
     * @return The unique identifier of the screenshot. This can be used to get the screenshot with the GetResult method
     * @throws Exception
     */
    public String Save() throws Exception
    {
        return Save("");
    }

    /**
     * Calls the GrabzIt web service to take the screenshot and returns a GrabzItFile object
     * 
     * Warning, this is a SYNCHONOUS method and can take up to 5 minutes before a response
     * @return Returns a GrabzItFile object containing the screenshot data.
     * @throws Exception 
     */
    public GrabzItFile SaveTo() throws Exception
    {
        String id = Save();
        
        if (isNullOrEmpty(id))
        {
            return null;
        }
        
        Thread.sleep(3000 + request.getOptions()._startDelay());
        
        //Wait for it to be ready.
        while (true)
        {
            Status status = GetStatus(id);

            if (!status.isCached() && !status.isProcessing())
            {
                throw new GrabzItException("The capture did not complete with the error: " + status.getMessage(), ErrorCode.RENDERINGERROR);
            }

            if (status.isCached())
            {
                GrabzItFile result = GetResult(id);

                if (result == null)
                {
                    throw new GrabzItException("The capture could not be found on GrabzIt.", ErrorCode.RENDERINGMISSINGSCREENSHOT);
                }

                return result;
            }

            Thread.sleep(3000);
        }
    }    
    
    /**
     * Calls the GrabzIt web service to take the screenshot and saves it to the target path provided
     *
     * Warning, this is a SYNCHONOUS method and can take up to 5 minutes before a response
     * @param saveToFile The file path that the screenshot should saved to
     * @return Returns the true if it is successful otherwise it throws an exception
     * @throws Exception
     */
    public boolean SaveTo(String saveToFile) throws Exception
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                GrabzItFile result = SaveTo();

                if (result == null)
                {
                    return false;
                }

                result.Save(saveToFile);
                break;
            }
            catch(GrabzItException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                if (attempt < 3)
                {
                    attempt++;
                    continue;
                }
                throw new GrabzItException("An error occurred trying to save the capture to: " +
                                    saveToFile, ErrorCode.FILESAVEERROR, ex);
            }
        }
        return true;
    }

    /**
     * Get the current status of a GrabzIt screenshot
     * @param id The id of the screenshot
     * @return A {@link it.grabz.grabzit.Status} object representing the screenshot status
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public Status GetStatus(String id) throws IOException, JAXBException, Exception
    {
        if (isNullOrEmpty(id))
        {
            return null;
        }
        
        return get(getRootURL() + "getstatus?id=" + id, Status.class);
    }

    /**
     * Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
     * @param domain The domain to return cookies for
     * @return An array of {@link it.grabz.grabzit.Cookie} objects
     * @throws Exception
     */
    public Cookie[] GetCookies(String domain) throws Exception
    {
        domain = ParameterUtility.nullCheck(domain);
        
        String sig = encrypt(String.format("%s|%s", this.applicationSecret, domain));

        String url = String.format("%sgetcookies?domain=%s&key=%s&sig=%s",
                                                  getRootURL(), domain, applicationKey, sig);
        Cookies cookies = get(url, Cookies.class);
        
        checkForError(cookies);
        
        return cookies.getCookies();
    }

    /**
     * Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
     * cookie is overridden.
     *
     * This can be useful if a websites functionality is controlled by cookies.
     * @param name The name of the cookie to set
     * @param domain The domain of the website to set the cookie for
     * @return Returns true if the cookie was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean SetCookie(String name, String domain) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        return SetCookie(name, domain, "", "", false, null);
    }

    /**
     * Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
     * cookie is overridden.
     *
     * This can be useful if a websites functionality is controlled by cookies.
     * @param name The name of the cookie to set
     * @param domain The domain of the website to set the cookie for
     * @param value The value of the cookie
     * @return Returns true if the cookie was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean SetCookie(String name, String domain, String value) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        return SetCookie(name, domain, value, "", false, null);
    }

    /**
     * Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
     * cookie is overridden.
     *
     * This can be useful if a websites functionality is controlled by cookies.
     * @param name The name of the cookie to set
     * @param domain The domain of the website to set the cookie for
     * @param value The value of the cookie
     * @param path The website path the cookie relates to
     * @return Returns true if the cookie was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean SetCookie(String name, String domain, String value, String path) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        return SetCookie(name, domain, value, path, false, null);
    }

    /**
     * Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
     * cookie is overridden.
     *
     * This can be useful if a websites functionality is controlled by cookies.
     * @param name The name of the cookie to set
     * @param domain The domain of the website to set the cookie for
     * @param value The value of the cookie
     * @param path The website path the cookie relates to
     * @param httponly Is the cookie only used on HTTP
     * @return Returns true if the cookie was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean SetCookie(String name, String domain, String value, String path, boolean httponly) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        return SetCookie(name, domain, value, path, httponly, null);
    }

    /**
     * Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
     * cookie is overridden.
     *
     * This can be useful if a websites functionality is controlled by cookies.
     * @param name The name of the cookie to set
     * @param domain The domain of the website to set the cookie for
     * @param value The value of the cookie
     * @param path The website path the cookie relates to
     * @param httponly Is the cookie only used on HTTP
     * @param expires When the cookie expires. Pass a null value if it does not expire
     * @return Returns true if the cookie was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean SetCookie(String name, String domain, String value, String path, boolean httponly, Date expires) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        String expiresStr = "";
        if (expires != null)
        {
            DateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
            expiresStr = df.format(expires);
        }
        
        name = ParameterUtility.nullCheck(name);
        domain = ParameterUtility.nullCheck(domain);
        value = ParameterUtility.nullCheck(value);
        path = ParameterUtility.nullCheck(path);        
        
        String sig = encrypt(String.format("%s|%s|%s|%s|%s|%s|%s|%s", applicationSecret, name, domain,
                                      value, path, (httponly ? 1 : 0), expiresStr, 0));
        
        String url = String.format("%ssetcookie?name=%s&domain=%s&value=%s&path=%s&httponly=%s&expires=%s&key=%s&sig=%s",
                                                  getRootURL(), ParameterUtility.encode(name), ParameterUtility.encode(domain), ParameterUtility.encode(value), ParameterUtility.encode(path), (httponly ? 1 : 0), ParameterUtility.encode(expiresStr), applicationKey, sig);

        GenericResult webResult = get(url, GenericResult.class);
        checkForError(webResult);

        return Boolean.valueOf(webResult.getResult());
    }

    /**
     * Delete a custom cookie or block a global cookie from being used.
     * @param name The name of the cookie to delete
     * @param domain The website the cookie belongs to
     * @return Returns true if the cookie was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean DeleteCookie(String name, String domain) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        name = ParameterUtility.nullCheck(name);
        domain = ParameterUtility.nullCheck(domain);
        
        String sig = encrypt(String.format("%s|%s|%s|%s", applicationSecret, name, domain, 1));

        String url = String.format("%ssetcookie?name=%s&domain=%s&delete=1&key=%s&sig=%s",
                                                  getRootURL(), ParameterUtility.encode(name), ParameterUtility.encode(domain), applicationKey, sig);

        GenericResult webResult = get(url, GenericResult.class);
        checkForError(webResult);

        return Boolean.valueOf(webResult.getResult());
    }

    /**
     * Add a new custom watermark.
     * @param identifier The identifier you want to give the custom watermark. It is important that this identifier is unique
     * @param path The absolute path of the watermark on your server. For instance C:/watermark/1.png
     * @param xpos The horizontal position you want the screenshot to appear at
     * @param ypos The vertical position you want the screenshot to appear at
     * @return Returns true if the watermark was successfully set
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws MalformedURLException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public boolean AddWaterMark(String identifier, String path, HorizontalPosition xpos, VerticalPosition ypos) throws UnsupportedEncodingException, NoSuchAlgorithmException, MalformedURLException, IOException, JAXBException, Exception
    {
        identifier = ParameterUtility.nullCheck(identifier);
        
        File fileToUpload = new File(path);
        if(!fileToUpload.exists())
        {
            throw new GrabzItException("File: " + path + " does not exist", ErrorCode.FILENONEXISTANTPATH);
        }
        
        String sig = encrypt(String.format("%s|%s|%s|%s", applicationSecret, identifier, String.valueOf(xpos.getValue()), String.valueOf(ypos.getValue())));

        String url = this.protocol + BASE_URL + "addwatermark";
        
        Post post = new Post(url, "UTF-8");
 
        post.addFormField("key", applicationKey);
        post.addFormField("identifier", identifier);
        post.addFormField("xpos", String.valueOf(xpos.getValue()));
        post.addFormField("ypos", String.valueOf(ypos.getValue()));
        post.addFormField("sig", sig);
        post.addFilePart("watermark", fileToUpload);

        GenericResult webResult = post.finish(GenericResult.class);
        checkForError(webResult);

        return Boolean.valueOf(webResult.getResult());
    }

    /**
     * Delete a custom watermark.
     * @param identifier The identifier of the custom watermark you want to delete
     * @return Returns true if the watermark was successfully deleted
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws Exception
     */
    public boolean DeleteWaterMark(String identifier) throws UnsupportedEncodingException, NoSuchAlgorithmException, Exception
    {
        identifier = ParameterUtility.nullCheck(identifier);
        
        String sig = encrypt(String.format("%s|%s", applicationSecret, identifier));

        String url = String.format("%sdeletewatermark?key=%s&identifier=%s&sig=%s",
                                                              getRootURL(), ParameterUtility.encode(applicationKey), ParameterUtility.encode(identifier), sig);

        GenericResult webResult = get(url, GenericResult.class);
        checkForError(webResult);

        return Boolean.valueOf(webResult.getResult());
    }

    /**
     * Get all your custom watermarks.
     * @return An array of {@link it.grabz.grabzit.WaterMark} objects
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public WaterMark[] GetWaterMarks() throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        return getWaterMarks("");
    }

    /**
     * Get a particular custom watermark.
     * @param identifier The identifier of a particular custom watermark you want to view
     * @return A {@link it.grabz.grabzit.WaterMark} object
     * @throws UnsupportedEncodingException
     * @throws NoSuchAlgorithmException
     * @throws IOException
     * @throws JAXBException
     * @throws Exception
     */
    public WaterMark GetWaterMark(String identifier) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        WaterMark[] waterMarks = getWaterMarks(identifier);

        if (waterMarks == null || waterMarks.length != 1)
        {
            return null;
        }

        return waterMarks[0];
    }

    private WaterMark[] getWaterMarks(String identifier) throws UnsupportedEncodingException, NoSuchAlgorithmException, IOException, JAXBException, Exception
    {
        identifier = ParameterUtility.nullCheck(identifier);
        
        String sig =  encrypt(String.format("%s|%s", applicationSecret, identifier));

        String url = String.format("%sgetwatermarks?key=%s&identifier=%s&sig=%s",
                                                          getRootURL(), ParameterUtility.encode(applicationKey), ParameterUtility.encode(identifier), sig);

        WaterMarks watermarks = get(url, WaterMarks.class);

        if (watermarks == null)
        {
            return new WaterMark[0];
        }
        
        checkForError(watermarks);

        return watermarks.getWaterMarks();
    }

    /**
     * This method returns the screenshot.
     * @param id The unique identifier of the screenshot, returned by the callback handler or the Save method
     * @return A {@link it.grabz.grabzit.GrabzItFile} object representing the screenshot
     * @throws IOException
     * @throws Exception
     */
    public GrabzItFile GetResult(String id) throws IOException, Exception
    {
        if (isNullOrEmpty(id))
        {
            return null;
        }
        
        String url = String.format("%sgetfile?id=%s", getRootURL(), id);

        URL requestUrl = new URL(url);
        URLConnection connection = (URLConnection) requestUrl.openConnection();
        HttpUtility.CheckResponse(connection);

        InputStream in = null;
        ByteArrayOutputStream buffer = null;
        try
        {
            in = connection.getInputStream();

            buffer = new ByteArrayOutputStream();

            int nRead;
            byte[] data = new byte[16384];

            while ((nRead = in.read(data, 0, data.length)) != -1)
            {
                buffer.write(data, 0, nRead);
            }

            buffer.flush();

            return new GrabzItFile(buffer.toByteArray());
        } 
        finally
        {
            if (in != null)
            {
                in.close();
            }
            if (buffer != null)
            {
                buffer.close();
            }
        }
    }

    private void checkForError(IMessageResult result) throws Exception
    {
        if (result != null && !isNullOrEmpty(result.getMessage()))
        {
            throw new GrabzItException(result.getMessage(), result.getCode());
        }
    }
    
    private <T> T get(String url, Class<T> clazz) throws IOException, JAXBException, Exception
    {
        try
        {
            URLConnection connection = getURLConnection(url);
            HttpUtility.CheckResponse(connection);        
            Response response = new Response();        
            return response.Parse(connection, clazz);
        }
        catch(UnknownHostException e)
        {
            throw new GrabzItException("A network error occurred when connecting to GrabzIt.", ErrorCode.NETWORKGENERALERROR);
        }
    }
    
    private <T> T post(String targetUrl, String parameters, Class<T> clazz) throws IOException, JAXBException, Exception
    {
        URLConnection conn = getURLConnection(targetUrl);                
        conn.setDoOutput(true);
        conn.setRequestProperty("Content-Type","application/x-www-form-urlencoded"); 
        OutputStreamWriter writer = null;
        
        try 
        {
            writer = new OutputStreamWriter(conn.getOutputStream());
            writer.write(parameters);
            writer.flush();
            
            HttpUtility.CheckResponse(conn);
            
            Response response = new Response();        
            return response.Parse(conn, clazz);
        }
        catch(UnknownHostException e)
        {
            throw new GrabzItException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NETWORKGENERALERROR);
        }        
        finally
        {
            if (writer != null)
            {
                writer.close();
            }
        }
    }

    private URLConnection getURLConnection(String targetUrl) throws IOException, MalformedURLException {
        URL url = new URL(targetUrl);
        if (this.proxy == null)
        {
            return url.openConnection();
        }
        return url.openConnection(this.proxy);
    }
    
    private String getRootURL()
    {
        return this.protocol + BASE_URL;
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
    
    private boolean isNullOrEmpty(String toTest)
    {
        return (toTest == null || toTest.isEmpty());
    }
}