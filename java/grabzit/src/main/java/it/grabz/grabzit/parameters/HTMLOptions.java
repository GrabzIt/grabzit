/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.parameters;

import it.grabz.grabzit.enums.BrowserType;
import java.io.UnsupportedEncodingException;
import java.util.HashMap;

/**
 * Contains all of the available options when creating a image capture.
 * 
 * @version 3.0
 * @author GrabzIt
 */
public class HTMLOptions extends BaseOptions {
    private int browserWidth;
    private int browserHeight;
    private String waitForElement;    
    private BrowserType requestAs;
    private boolean noAds;
    private boolean noCookieNotifications;
    private String address;

    public HTMLOptions()
    {
        this.browserHeight = 0;
        this.browserWidth = 0;
        this.waitForElement = "";
        this.requestAs = BrowserType.STANDARDBROWSER;
        this.address = "";
    }
    
    /**
     * @return the width of the browser in pixels.
     */
    public int getBrowserWidth() {
        return browserWidth;
    }

    /**
     * @param browserWidth set the width of the browser in pixels.
     */
    public void setBrowserWidth(int browserWidth) {
        this.browserWidth = browserWidth;
    }

    /**
     * @return the height of the browser in pixels.
     */
    public int getBrowserHeight() {
        return browserHeight;
    }

    /**
     * @param browserHeight set the height of the browser in pixels.
     */
    public void setBrowserHeight(int browserHeight) {
        this.browserHeight = browserHeight;
    }

    /**
     * @return the number of milliseconds to wait before creating the capture.
     */
    public int getDelay() {
        return delay;
    }

    /**
     * @param delay set the number of milliseconds to wait before creating the capture.
     */
    public void setDelay(int delay) {
        this.delay = delay;
    }
    
    /**
     * @return the waitForElement the CSS selector of the HTML element in the web page that must be visible before the capture is performed.
     */
    public String getWaitForElement() {
        return waitForElement;
    }

    /**
     * @param waitForElement the CSS selector of the HTML element in the web page that must be visible before the capture is performed.
     */
    public void setWaitForElement(String waitForElement) {
        this.waitForElement = waitForElement;
    }        
        
    /**
     * @return get which user agent type should be used.
     */
    public BrowserType getRequestAs() {
        return requestAs;
    }

    /**
     * @param requestAs set which user agent type should be used.
     */
    public void setRequestAs(BrowserType requestAs) {
        this.requestAs = requestAs;
    }
    
    /**
     * @return if true adverts should be automatically hidden.
     */
    public boolean isNoAds() {
        return noAds;
    }

    /**
     * @param noAds set to true if adverts should be automatically hidden.
     */
    public void setNoAds(boolean noAds) {
        this.noAds = noAds;
    }    
    
    /**
     * @return if true cookie notifications should be automatically hidden.
     */
    public boolean isNoCookieNotifications() {
        return noCookieNotifications;
    }

    /**
     * @param noCookieNotifications set to true if cookie notification should be automatically hidden.
     */
    public void setNoCookieNotifications(boolean noCookieNotifications) {
        this.noCookieNotifications = noCookieNotifications;
    }    
    
    /**
     * @return the URL to execute the HTML code in.
     */
    public String getAddress() {
        return address;
    }

    /**
     * @param address the URL to execute the HTML code in.
     */
    public void setAddress(String address) {
        this.address = address;
    }        
    
    /**
     * Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
     * GrabzIt to perform a HTTP post.        
     * 
     * @param name - The name of the HTTP Post parameter
     * @param value - The value of the HTTP Post parameter
     * @throws UnsupportedEncodingException 
     */
    public void AddPostParameter(String name, String value) throws UnsupportedEncodingException
    {
        this.post = appendParameter(this.post, name, value);
    }    
    
    @Override
    public String _getSignatureString(String applicationSecret, String callBackURL, String url)
    {
        String urlParam = "";
        if (url != null && !"".equals(url))
        {
            urlParam = ParameterUtility.nullCheck(url)+"|";
        }		

        String callBackURLParam = "";
        if (callBackURL != null && !"".equals(callBackURL))
        {
            callBackURLParam = ParameterUtility.nullCheck(callBackURL);
        }				

        return ParameterUtility.nullCheck(applicationSecret) + "|" + urlParam + callBackURLParam
        + "|" + browserHeight + "|" + browserWidth + "|" + getCustomId() + "|" + delay 
        + "|" + requestAs.getValue() + "|" + getCountry().getValue()
        + "|" + getExportURL() + "|" + waitForElement + "|" + getEncryptionKey()
        + "|" + ParameterUtility.toInt(noAds) + "|" + post + "|" + getProxy() + "|" + address + "|" + ParameterUtility.toInt(noCookieNotifications);
    }    
    
    @Override
    public HashMap<String, String>  _getParameters(String applicationKey, String sig, String callBackURL, String dataName, String dataValue) throws UnsupportedEncodingException
    {
        HashMap<String, String> params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue);		
        params.put("waitfor", ParameterUtility.encode(ParameterUtility.nullCheck(waitForElement)));
        params.put("requestmobileversion", String.valueOf(requestAs.getValue()));
	params.put("bwidth", String.valueOf(browserWidth));
	params.put("bheight", String.valueOf(browserHeight));
	params.put("delay", String.valueOf(delay));
        params.put("noads", String.valueOf(ParameterUtility.toInt(noAds)));
        params.put("post", ParameterUtility.encode(ParameterUtility.nullCheck(post)));        
        params.put("nonotify", String.valueOf(ParameterUtility.toInt(noCookieNotifications)));
        params.put("address", ParameterUtility.encode(ParameterUtility.nullCheck(address)));
        
        return params;
    }    
}