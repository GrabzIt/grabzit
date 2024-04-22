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
 * Contains all of the available options when creating an animated GIF conversion.
 * 
 * @version 3.0
 * @author GrabzIt
 */
public class VideoOptions extends BaseOptions{
    private int browserWidth;
    private int browserHeight;
    private String address;
    private int start;
    private int duration;
    private float framesPerSecond;
    private String customWaterMarkId;    
    private String clickElement;    
    private String waitForElement;
    private BrowserType requestAs;
    private boolean noAds;    
    private boolean noCookieNotifications;
    private int outputWidth;
    private int outputHeight;    

    public VideoOptions()
    {
        this.browserHeight = 0;
        this.browserWidth = 0;
        this.outputHeight = 0;
        this.outputWidth = 0;        
        this.duration = 10;
        this.requestAs = BrowserType.STANDARDBROWSER;
        this.customWaterMarkId = "";
        this.clickElement = "";
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
     * @return the user agent type should be used.
     */
    public BrowserType getRequestAs() {
        return requestAs;
    }

    /**
     * @param requestAs the user agent type should be used.
     */
    public void setRequestAs(BrowserType requestAs) {
        this.requestAs = requestAs;
    }    

    /**
     * @return the starting time of the web page that should be converted into a video.
     */
    public int getStart() {
        return start;
    }

    /**
     * @param start the starting time of the web page that should be converted into a video.
     */
    public void setStart(int start) {
        this.delay = start;
        this.start = start;
    }

    /**
     * @return the length in seconds of the web page that should be converted into a video.
     */
    public int getDuration() {
        return duration;
    }

    /**
     * @param duration the length in seconds of the web page that should be converted into a video.
     */
    public void setDuration(int duration) {
        this.duration = duration;
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
     * @return the number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10.
     */
    public float getFramesPerSecond() {
        return framesPerSecond;
    }

    /**
     * @param framesPerSecond the number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10.
     */
    public void setFramesPerSecond(float framesPerSecond) {
        this.framesPerSecond = framesPerSecond;
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
     * @return the custom watermark id.
     */
    public String getCustomWaterMarkId() {
        return customWaterMarkId;
    }

    /**
     * @param customWaterMarkId a custom watermark to add to the video.
     */
    public void setCustomWaterMarkId(String customWaterMarkId) {
        this.customWaterMarkId = customWaterMarkId;
    }
    
    /**
     * @return get the CSS selector of the HTML element to click.
     */
    public String getClickElement() {
        return clickElement;
    }

    /**
     * @param clickElement set the CSS selector of the HTML element to click.
     */
    public void setClickElement(String clickElement) {
        this.clickElement = clickElement;
    }    
    
    /**
     * @return the width of the resulting video in pixels.
     */
    public int getOutputWidth() {
        return outputWidth;
    }

    /**
     * @param outputWidth set the width of the resulting video in pixels.
     */
    public void setOutputWidth(int outputWidth) {
        this.outputWidth = outputWidth;
    }

    /**
     * @return get the height of the resulting video in pixels.
     */
    public int getOutputHeight() {
        return outputHeight;
    }

    /**
     * @param outputHeight set the height of the resulting video in pixels.
     */
    public void setOutputHeight(int outputHeight) {
        this.outputHeight = outputHeight;
    }
    

    public String _getSignatureString(String applicationSecret, String callBackURL)
    {
        return _getSignatureString(applicationSecret, callBackURL, null);
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
        + "|" + this.browserHeight + "|" + this.browserWidth + "|" + getCustomId() + "|" + this.customWaterMarkId 
        + "|" + start + "|" + requestAs.getValue() + "|" + getCountry().getValue()
        + "|" + getExportURL() + "|" + waitForElement + "|" + getEncryptionKey()
        + "|" + ParameterUtility.toInt(noAds) + "|" + post + "|" + getProxy() + "|" + address  + "|" + ParameterUtility.toInt(noCookieNotifications)
         + "|" + clickElement + "|" + ParameterUtility.toString(framesPerSecond)  + "|" + duration + "|" + outputWidth + "|" + outputHeight;
    }    

    @Override
    public HashMap<String, String> _getParameters(String applicationKey, String sig, String callBackURL, String dataName, String dataValue) throws UnsupportedEncodingException
    {
        HashMap<String, String> params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue);		
        params.put("bwidth", String.valueOf(this.browserWidth));
        params.put("bheight", String.valueOf(this.browserHeight));
        params.put("duration", String.valueOf(duration));
        params.put("waitfor", ParameterUtility.encode(ParameterUtility.nullCheck(waitForElement)));
        params.put("customwatermarkid", ParameterUtility.nullCheck(customWaterMarkId));
        params.put("start", String.valueOf(start));
        params.put("fps", ParameterUtility.toString(framesPerSecond));
        params.put("click", ParameterUtility.encode(ParameterUtility.nullCheck(clickElement)));
        params.put("address", ParameterUtility.encode(ParameterUtility.nullCheck(address)));		
        params.put("nonotify", String.valueOf(ParameterUtility.toInt(noCookieNotifications)));	
        params.put("noads", String.valueOf(ParameterUtility.toInt(noAds)));
        params.put("click", ParameterUtility.encode(ParameterUtility.nullCheck(clickElement)));
        params.put("post", ParameterUtility.encode(ParameterUtility.nullCheck(post)));
        params.put("requestmobileversion", String.valueOf(requestAs.getValue()));
 	params.put("width", String.valueOf(outputWidth));
	params.put("height", String.valueOf(outputHeight));       

        return params;
    }    
}
