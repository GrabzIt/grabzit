/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.BrowserType;
import it.grabz.grabzit.enums.Country;
import it.grabz.grabzit.enums.ErrorCode;
import it.grabz.grabzit.enums.HorizontalPosition;
import it.grabz.grabzit.enums.TableFormat;
import it.grabz.grabzit.enums.PageOrientation;
import it.grabz.grabzit.enums.VerticalPosition;
import it.grabz.grabzit.enums.ImageFormat;
import it.grabz.grabzit.enums.PageSize;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.net.URLEncoder;
import java.net.UnknownHostException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import javax.xml.bind.JAXBException;

/**
 * This client provides access to the GrabzIt web services
 * This API allows you to take screenshot of websites for free and convert them into images, PDF's and tables.
 *
 * @version 2.0
 * @author GrabzIt
 * @see <a href="http://grabz.it/api/java/">GrabzIt Java API</a>
 */
public class GrabzItClient {
    private String applicationKey;
    private String applicationSecret;

    private String request;
    private String signaturePartOne;
    private String signaturePartTwo;
    
    private final String BASE_URL = "http://grabz.it/services/";

    /**
     * Create a new instance of the Client class in order to access the GrabzIt API.
     *
     * @see <a href="http://grabz.it/register.aspx">You can get an application key and secret by registering for free with GrabzIt</a>
     * @param applicationKey your application key
     * @param applicationSecret your application secret
     */
    public GrabzItClient(String applicationKey, String applicationSecret)
    {
        this.applicationKey = applicationKey;
        this.applicationSecret = applicationSecret;
    }
    
    /**
     * This method sets the parameters required to take a screenshot of a web page.
     * @param url The URL that the screenshot should be made of
     * @param customId A custom identifier that you can pass through to the screenshot web service. This will be returned with the callback URL you have specified
     * @param browserWidth The width of the browser in pixels
     * @param browserHeight The height of the browser in pixels
     * @param outputWidth The height of the resulting screenshot in pixels
     * @param outputHeight The width of the resulting screenshot in pixels
     * @param format The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png
     * @param delay The number of milliseconds to wait before taking the screenshot
     * @param targetElement The id of the only HTML element in the web page to turn into a screenshot
     * @param requestAs Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
     * @param customWaterMarkId Add a custom watermark to the image
     * @param quality The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality for the specified image format
     * @param country Request the screenshot from different countries: Default, UK or US
     * @throws UnsupportedEncodingException
     */
    public void SetImageOptions(String url, String customId, int browserWidth, int browserHeight, int outputWidth, int outputHeight, ImageFormat format, int delay, String targetElement, BrowserType requestAs, String customWaterMarkId, int quality, Country country) throws UnsupportedEncodingException
    {
        this.request = String.format("%stakepicture.ashx?url=%s&key=%s&width=%s&height=%s&bwidth=%s&bheight=%s&format=%s&customid=%s&delay=%s&target=%s&customwatermarkid=%s&requestmobileversion=%s&country=%s&quality=%s&callback=",
                                                              BASE_URL, encode(url), applicationKey, outputWidth, outputHeight,
                                                              browserWidth, browserHeight, format.getValue(), encode(customId), delay, encode(targetElement), encode(customWaterMarkId), requestAs.getValue(), country.getValue(), quality);
        this.signaturePartOne = applicationSecret + "|" + url + "|";
        this.signaturePartTwo = "|" + format.getValue() + "|" + outputHeight + "|" + outputWidth + "|" + browserHeight + "|" + browserWidth + "|" + customId + "|" + delay + "|" + targetElement + "|" + customWaterMarkId + "|" + requestAs.getValue() + "|" + country.getValue() + "|" + quality;
    }

        /**
     * This method sets the parameters required to take a screenshot of a web page.
     * @param url The URL that the screenshot should be made of
     * @param customId A custom identifier that you can pass through to the screenshot web service. This will be returned with the callback URL you have specified
     * @param browserWidth The width of the browser in pixels
     * @param browserHeight The height of the browser in pixels
     * @param outputWidth The height of the resulting screenshot in pixels
     * @param outputHeight The width of the resulting screenshot in pixels
     * @param format The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, tiff, jpg, png
     * @param delay The number of milliseconds to wait before taking the screenshot
     * @param targetElement The id of the only HTML element in the web page to turn into a screenshot
     * @param requestAs Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
     * @param customWaterMarkId Add a custom watermark to the image
     * @throws UnsupportedEncodingException
     */
    public void SetImageOptions(String url, String customId, int browserWidth, int browserHeight, int outputWidth, int outputHeight, ImageFormat format, int delay, String targetElement, BrowserType requestAs, String customWaterMarkId) throws UnsupportedEncodingException
    {
        SetImageOptions(url, customId, browserWidth, browserHeight, outputWidth, outputHeight, format, delay, targetElement, requestAs, customWaterMarkId, -1, Country.DEFAULT);
    }

    /**
     * This method sets the parameters required to take a screenshot of a web page.
     * @param url The URL that the screenshot should be made of
     * @throws UnsupportedEncodingException
     */
    public void SetImageOptions(String url) throws UnsupportedEncodingException
    {
        SetImageOptions(url, "", 0, 0, 0, 0, ImageFormat.JPG, 0, "", BrowserType.STANDARDBROWSER, "");
    }

    /**
     * This method sets the parameters required to take a screenshot of a web page.
     * @param url The URL that the screenshot should be made of
     * @param customId A custom identifier that you can pass through to the screenshot web service. This will be returned with the callback URL you have specified
     * @throws UnsupportedEncodingException
     */
    public void SetImageOptions(String url, String customId) throws UnsupportedEncodingException
    {
        SetImageOptions(url, customId, 0, 0, 0, 0, ImageFormat.JPG, 0, "", BrowserType.STANDARDBROWSER, "");
    }

    /**
     * This method sets the parameters required to extract all tables from a web page.
     * @param url The URL that the should be used to extract tables
     * @param customId A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
     * @param tableNumberToInclude Which table to include, in order from the beginning of the page to the end
     * @param format The format the table should be in: csv, xlsx
     * @param includeHeaderNames If true header names will be included in the table
     * @param includeAllTables If true all table on the web page will be extracted with each table appearing in a separate spreadsheet sheet. Only available with the XLSX format
     * @param targetElement The id of the only HTML element in the web page that should be used to extract tables from
     * @param requestAs Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
     * @param country Request the screenshot from different countries: Default, UK or US
     * @throws UnsupportedEncodingException
     */
    public void SetTableOptions(String url, String customId, int tableNumberToInclude, TableFormat format, boolean includeHeaderNames, boolean includeAllTables, String targetElement, BrowserType requestAs, Country country) throws UnsupportedEncodingException
    {
        this.request = BASE_URL + "taketable.ashx?key=" + encode(applicationKey)+"&url="+encode(url)+"&includeAllTables="+ toInt(includeAllTables)+"&includeHeaderNames="+toInt(includeHeaderNames)+"&format="+format.getValue()+"&tableToInclude="+tableNumberToInclude+"&customid="+encode(customId)+"&target="+encode(targetElement)+"&requestmobileversion="+requestAs.getValue()+"&country="+country.getValue()+"&callback=";
        this.signaturePartOne = applicationSecret+"|"+url+"|";
        this.signaturePartTwo = "|"+customId+"|"+tableNumberToInclude+"|"+toInt(includeAllTables)+"|"+toInt(includeHeaderNames)+"|"+targetElement+"|"+format.getValue()+"|"+requestAs.getValue()+"|"+country.getValue();
    }
    
     /**
     * This method sets the parameters required to extract all tables from a web page.
     * @param url The URL that the should be used to extract tables
     * @param customId A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
     * @param tableNumberToInclude Which table to include, in order from the beginning of the page to the end
     * @param format The format the table should be in: csv, xlsx
     * @param includeHeaderNames If true header names will be included in the table
     * @param includeAllTables If true all table on the web page will be extracted with each table appearing in a separate spreadsheet sheet. Only available with the XLSX format
     * @param targetElement The id of the only HTML element in the web page that should be used to extract tables from
     * @param requestAs Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
     * @throws UnsupportedEncodingException
     */
    public void SetTableOptions(String url, String customId, int tableNumberToInclude, TableFormat format, boolean includeHeaderNames, boolean includeAllTables, String targetElement, BrowserType requestAs) throws UnsupportedEncodingException
    {
        SetTableOptions(url, customId, tableNumberToInclude, format, includeHeaderNames, includeAllTables, targetElement, requestAs, Country.DEFAULT);
    }   

    /**
     * This method sets the parameters required to extract all tables from a web page.
     * @param url The URL that the should be used to extract tables
     * @param customId A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
     * @throws UnsupportedEncodingException
     */
    public void SetTableOptions(String url, String customId) throws UnsupportedEncodingException
    {
        SetTableOptions(url, customId, 1, TableFormat.CSV, true, false, "", BrowserType.STANDARDBROWSER);
    }

    /**
     * This method sets the parameters required to extract all tables from a web page.
     * @param url The URL that the should be used to extract tables
     * @throws UnsupportedEncodingException
     */
    public void SetTableOptions(String url) throws UnsupportedEncodingException
    {
        SetTableOptions(url, "", 1, TableFormat.CSV, true, false, "", BrowserType.STANDARDBROWSER);
    }

    /**
     * This method sets the parameters required to convert a web page into a PDF.
     * @param url The URL that the should be converted into a PDF
     * @param customId A custom identifier that you can pass through to the webs ervice. This will be returned with the callback URL you have specified
     * @param includeBackground If true the background of the web page should be included in the screenshot
     * @param pagesize The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5', 'Letter'
     * @param orientation The orientation of the PDF to be returned: 'Landscape' or 'Portrait'
     * @param includeLinks True if links should be included in the PDF
     * @param includeOutline True if the PDF outline should be included
     * @param title Provide a title to the PDF document
     * @param coverURL The URL of a web page that should be used as a cover page for the PDF
     * @param marginTop The margin that should appear at the top of the PDF document page
     * @param marginLeft The margin that should appear at the left of the PDF document page
     * @param marginBottom The margin that should appear at the bottom of the PDF document page
     * @param marginRight The margin that should appear at the right of the PDF document
     * @param delay The number of milliseconds to wait before taking the screenshot
     * @param requestAs Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
     * @param customWaterMarkId Add a custom watermark to each page of the PDF document
     * @param quality The quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
     * @param country Request the screenshot from different countries: Default, UK or US
     * @throws UnsupportedEncodingException
     */
    public void SetPDFOptions(String url, String customId, boolean includeBackground, PageSize pagesize, PageOrientation orientation, boolean includeLinks, boolean includeOutline, String title, String coverURL, int marginTop, int marginLeft, int marginBottom, int marginRight, int delay, BrowserType requestAs, String customWaterMarkId, int quality, Country country) throws UnsupportedEncodingException
    {
        this.request = BASE_URL + "takepdf.ashx?key=" + encode(applicationKey) + "&url=" + encode(url) + "&background=" + toInt(includeBackground) + "&pagesize=" + pagesize.getValue() + "&orientation=" + orientation.getValue() + "&customid=" + encode(customId) + "&customwatermarkid=" + encode(customWaterMarkId) + "&includelinks=" + toInt(includeLinks) + "&includeoutline=" + toInt(includeOutline) + "&title=" + encode(title) + "&coverurl=" + encode(coverURL) + "&mleft=" + marginLeft + "&mright=" + marginRight + "&mtop=" + marginTop + "&mbottom=" + marginBottom + "&delay=" + delay + "&requestmobileversion=" + requestAs.getValue() + "&country=" + country.getValue() + "&quality=" + quality + "&callback=";
        this.signaturePartOne = applicationSecret + "|" + url + "|";
        this.signaturePartTwo = "|" + customId + "|" + toInt(includeBackground) + "|" + pagesize.getValue() + "|" + orientation.getValue() + "|" + customWaterMarkId + "|" + toInt(includeLinks) + "|" + toInt(includeOutline) + "|" + title + "|" + coverURL + "|" + marginTop + "|" + marginLeft + "|" + marginBottom + "|" + marginRight + "|" + delay + "|" + requestAs.getValue() + "|" + country.getValue() + "|" + quality;
    }

   /**
     * This method sets the parameters required to convert a web page into a PDF.
     * @param url The URL that the should be converted into a PDF
     * @param customId A custom identifier that you can pass through to the webs ervice. This will be returned with the callback URL you have specified
     * @param includeBackground If true the background of the web page should be included in the screenshot
     * @param pagesize The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5', 'Letter'
     * @param orientation The orientation of the PDF to be returned: 'Landscape' or 'Portrait'
     * @param includeLinks True if links should be included in the PDF
     * @param includeOutline True if the PDF outline should be included
     * @param title Provide a title to the PDF document
     * @param coverURL The URL of a web page that should be used as a cover page for the PDF
     * @param marginTop The margin that should appear at the top of the PDF document page
     * @param marginLeft The margin that should appear at the left of the PDF document page
     * @param marginBottom The margin that should appear at the bottom of the PDF document page
     * @param marginRight The margin that should appear at the right of the PDF document
     * @param delay The number of milliseconds to wait before taking the screenshot
     * @param requestAs Request screenshot in different forms: Standard Browser, Mobile Browser and Search Engine
     * @param customWaterMarkId Add a custom watermark to each page of the PDF document
     * @throws UnsupportedEncodingException
     */
    public void SetPDFOptions(String url, String customId, boolean includeBackground, PageSize pagesize, PageOrientation orientation, boolean includeLinks, boolean includeOutline, String title, String coverURL, int marginTop, int marginLeft, int marginBottom, int marginRight, int delay, BrowserType requestAs, String customWaterMarkId) throws UnsupportedEncodingException
    {
        SetPDFOptions(url, customId, includeBackground, pagesize, orientation, includeLinks, includeOutline, title, coverURL, marginTop, marginLeft, marginBottom, marginRight, delay, requestAs, customWaterMarkId, -1, Country.DEFAULT);
    }
    /**
     * This method sets the parameters required to convert a web page into a PDF.
     * @param url The URL that the should be converted into a PDF
     * @param customId A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified
     * @throws UnsupportedEncodingException
     */
    public void SetPDFOptions(String url, String customId)  throws UnsupportedEncodingException
    {
        SetPDFOptions(url, customId, true, PageSize.A4, PageOrientation.PORTRAIT, true, false, "", "", 10, 10, 10, 10, 0, BrowserType.STANDARDBROWSER, "");
    }

    /**
     * This method sets the parameters required to convert a web page into a PDF.
     * @param url The URL that the should be converted into a PDF
     * @throws UnsupportedEncodingException
     */
    public void SetPDFOptions(String url)  throws UnsupportedEncodingException
    {
        SetPDFOptions(url, "", true, PageSize.A4, PageOrientation.PORTRAIT, true, false, "", "", 10, 10, 10, 10, 0, BrowserType.STANDARDBROWSER, "");
    }

    /**
     * Calls the GrabzIt web service to take the screenshot.
     * @param callBackURL The handler the GrabzIt web service should call after it has completed its work
     * @return The unique identifier of the screenshot. This can be used to get the screenshot with the GetResult method
     * @throws Exception
     */
    public String Save(String callBackURL) throws Exception
    {
        if (isNullOrEmpty(this.signaturePartOne) && isNullOrEmpty(this.signaturePartTwo) && isNullOrEmpty(this.request))
        {
            throw new GrabzItException("No screenshot parameters have been set.", ErrorCode.PARAMETERMISSINGPARAMETERS);
        }

        String sig = encrypt(this.signaturePartOne + callBackURL + this.signaturePartTwo);
        this.request += encode(callBackURL) + "&sig=" + encode(sig);

        TakeScreenShotResult result = get(this.request, TakeScreenShotResult.class);
        checkForError(result);

        return result.getId();
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
        
        //Wait for it to be ready.
        while (true)
        {
            Status status = GetStatus(id);

            if (!status.isCached() && !status.isProcessing())
            {
                throw new GrabzItException("The screenshot did not complete with the error: " + status.getMessage(), ErrorCode.RENDERINGERROR);
            }

            if (status.isCached())
            {
                GrabzItFile result = GetResult(id);

                if (result == null)
                {
                    throw new GrabzItException("The screenshot could not be found on GrabzIt.", ErrorCode.RENDERINGMISSINGSCREENSHOT);
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
            catch (Exception ex)
            {
                if (attempt < 3)
                {
                    attempt++;
                    continue;
                }
                throw new GrabzItException("An error occurred trying to save the screenshot to: " +
                                    saveToFile, ErrorCode.FILESAVEERROR);
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
        return get(BASE_URL + "getstatus.ashx?id=" + id, Status.class);
    }

    /**
     * Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
     * @param domain The domain to return cookies for
     * @return An array of {@link it.grabz.grabzit.Cookie} objects
     * @throws Exception
     */
    public Cookie[] GetCookies(String domain) throws Exception
    {
        String sig = encrypt(String.format("%s|%s", this.applicationSecret, domain));

        String url = String.format("%sgetcookies.ashx?domain=%s&key=%s&sig=%s",
                                                  BASE_URL, domain, applicationKey, sig);
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
        
        String sig = encrypt(String.format("%s|%s|%s|%s|%s|%s|%s|%s", applicationSecret, name, domain,
                                      value, path, (httponly ? 1 : 0), expiresStr, 0));
        
        String url = String.format("%ssetcookie.ashx?name=%s&domain=%s&value=%s&path=%s&httponly=%s&expires=%s&key=%s&sig=%s",
                                                  BASE_URL, encode(name), encode(domain), encode(value), encode(path), (httponly ? 1 : 0), encode(expiresStr), applicationKey, sig);

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
        String sig = encrypt(String.format("%s|%s|%s|%s", applicationSecret, name, domain, 1));

        String url = String.format("%ssetcookie.ashx?name=%s&domain=%s&delete=1&key=%s&sig=%s",
                                                  BASE_URL, encode(name), encode(domain), applicationKey, sig);

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
        File fileToUpload = new File(path);
        if(!fileToUpload.exists())
        {
            throw new GrabzItException("File: " + path + " does not exist", ErrorCode.FILENONEXISTANTPATH);
        }
        
        String sig = encrypt(String.format("%s|%s|%s|%s", applicationSecret, identifier, String.valueOf(xpos.getValue()), String.valueOf(ypos.getValue())));

        String url = String.format("%saddwatermark.ashx", BASE_URL);
        
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
        String sig = encrypt(String.format("%s|%s", applicationSecret, identifier));

        String url = String.format("%sdeletewatermark.ashx?key=%s&identifier=%s&sig=%s",
                                                              BASE_URL, encode(applicationKey), encode(identifier), sig);

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
        String sig =  encrypt(String.format("%s|%s", applicationSecret, identifier));

        String url = String.format("%sgetwatermarks.ashx?key=%s&identifier=%s&sig=%s",
                                                          BASE_URL, encode(applicationKey), encode(identifier), sig);

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
        String url = String.format("%sgetfile.ashx?id=%s", BASE_URL, id);

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
        if (!isNullOrEmpty(result.getMessage()))
        {
            throw new GrabzItException(result.getMessage(), result.getCode());
        }
    }
    
    private <T> T get(String url, Class<T> clazz) throws IOException, JAXBException, Exception
    {
        try
        {
            URL request = new URL(url);
            URLConnection connection = (URLConnection) request.openConnection();
            HttpUtility.CheckResponse(connection);        
            Response response = new Response();        
            return response.Parse(connection, clazz);
        }
        catch(UnknownHostException e)
        {
            throw new GrabzItException("A network error occured when connecting to the GrabzIt servers.", ErrorCode.NETWORKGENERALERROR);
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

    private String encode(String value) throws UnsupportedEncodingException
    {
        return URLEncoder.encode(value, "UTF-8");
    }

    private boolean isNullOrEmpty(String toTest)
    {
        return (toTest == null || toTest.isEmpty());
    }

    private int toInt(boolean value)
    {
        return (value ? 1 : 0);
    }
}
