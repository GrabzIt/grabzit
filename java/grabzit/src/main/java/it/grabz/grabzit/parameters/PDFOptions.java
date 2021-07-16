/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.parameters;

import it.grabz.grabzit.enums.BrowserType;
import it.grabz.grabzit.enums.CSSMediaType;
import it.grabz.grabzit.enums.PageOrientation;
import it.grabz.grabzit.enums.PageSize;
import java.io.UnsupportedEncodingException;
import java.util.HashMap;

/**
 * Contains all of the available options when creating a PDF.
 * 
 * @version 3.0
 * @author GrabzIt
 */
public class PDFOptions extends BaseOptions {
    private boolean includeBackground;
    private PageSize pagesize;
    private PageOrientation orientation;
    private boolean includeLinks;
    private boolean includeOutline;
    private String title;
    private String coverURL;
    private int marginTop;
    private int marginLeft;
    private int marginBottom;
    private int marginRight;
    private BrowserType requestAs;
    private String templateId;
    private String customWaterMarkId;
    private int quality;
    private String targetElement;
    private String hideElement;
    private String waitForElement;
    private boolean noAds;    
    private int browserWidth;
    private int width;
    private int height; 
    private String templateVariables;
    private String mergeId;
    private boolean noCookieNotifications;
    private String address;
    private CSSMediaType cssMediaType;
    private String password;
    private String clickElement;
    
    public PDFOptions()
    {
        this.includeBackground = true;
        this.pagesize = PageSize.A4;
        this.orientation = PageOrientation.PORTRAIT;
        this.includeLinks = true;
        this.includeOutline = false;
        this.title = "";
        this.coverURL = "";
        this.marginBottom = 10;
        this.marginLeft = 10;
        this.marginRight = 10;
        this.marginTop = 10;
        this.requestAs = BrowserType.STANDARDBROWSER;
        this.customWaterMarkId = "";
        this.templateId = "";
        this.targetElement = "";
        this.hideElement = "";
        this.waitForElement = "";
        this.quality = -1;
        this.browserWidth = 0;
        this.width = 0;
        this.height = 0;
        this.templateVariables = "";
        this.mergeId = "";
        this.address = "";
        this.cssMediaType = CSSMediaType.SCREEN;
        this.password = "";
        this.clickElement = "";
    }
    
    /**
     * @return true if the background of the web page should be included in the PDF.
     */
    public boolean isIncludeBackground() {
        return includeBackground;
    }

    /**
     * @param includeBackground set to true if the background of the web page should be included in the PDF.
     */
    public void setIncludeBackground(boolean includeBackground) {
        this.includeBackground = includeBackground;
    }

    /**
     * @return the page size of the PDF.
     */
    public PageSize getPagesize() {
        return pagesize;
    }

    /**
     * @param pagesize the page size of the PDF.
     */
    public void setPagesize(PageSize pagesize) {
        this.pagesize = pagesize;
    }

    /**
     * @return the orientation of the PDF either portrait or landscape.
     */
    public PageOrientation getOrientation() {
        return orientation;
    }

    /**
     * @param orientation the orientation of the PDF either portrait or landscape.
     */
    public void setOrientation(PageOrientation orientation) {
        this.orientation = orientation;
    }
    
    /**
     * @return the CSS media type of the PDF either Screen or Print.
     */
    public CSSMediaType getCSSMediaType() {
        return cssMediaType;
    }

    /**
     * @param cssMediaType the orientation of the PDF either portrait or landscape.
     */
    public void setCSSMediaType(CSSMediaType cssMediaType) {
        this.cssMediaType = cssMediaType;
    } 

    /**
     * @return if the links should be included in the PDF.
     */
    public boolean isIncludeLinks() {
        return includeLinks;
    }

    /**
     * @param includeLinks set to true if links should be included in the PDF.
     */
    public void setIncludeLinks(boolean includeLinks) {
        this.includeLinks = includeLinks;
    }

    /**
     * @return if the PDF outline should be included.
     */
    public boolean isIncludeOutline() {
        return includeOutline;
    }

    /**
     * @param includeOutline set to true if the PDF outline should be included.
     */
    public void setIncludeOutline(boolean includeOutline) {
        this.includeOutline = includeOutline;
    }

    /**
     * @return the title for the PDF document.
     */
    public String getTitle() {
        return title;
    }

    /**
     * @param title set the title for the PDF document.
     */
    public void setTitle(String title) {
        this.title = title;
    }

    /**
     * @return the URL of a web page that should be used as a cover page for the PDF.
     */
    public String getCoverURL() {
        return coverURL;
    }

    /**
     * @param coverURL the URL of a web page that should be used as a cover page for the PDF.
     */
    public void setCoverURL(String coverURL) {
        this.coverURL = coverURL;
    }

    /**
     * @return the margin that should appear at the top of the PDF document page.
     */
    public int getMarginTop() {
        return marginTop;
    }

    /**
     * @param marginTop the margin that should appear at the top of the PDF document page.
     */
    public void setMarginTop(int marginTop) {
        this.marginTop = marginTop;
    }

    /**
     * @return the margin that should appear at the left of the PDF document page.
     */
    public int getMarginLeft() {
        return marginLeft;
    }

    /**
     * @param marginLeft set the margin that should appear at the left of the PDF document page.
     */
    public void setMarginLeft(int marginLeft) {
        this.marginLeft = marginLeft;
    }

    /**
     * @return the margin that should appear at the bottom of the PDF document page.
     */
    public int getMarginBottom() {
        return marginBottom;
    }

    /**
     * @param marginBottom the margin that should appear at the bottom of the PDF document page.
     */
    public void setMarginBottom(int marginBottom) {
        this.marginBottom = marginBottom;
    }

    /**
     * @return the margin that should appear at the right of the PDF document.
     */
    public int getMarginRight() {
        return marginRight;
    }

    /**
     * @param marginRight set the margin that should appear at the right of the PDF document.
     */
    public void setMarginRight(int marginRight) {
        this.marginRight = marginRight;
    }
    
    /**
     * @return the width of the resulting PDF in mm.
     */
    public int getPageWidth() {
        return width;
    }

    /**
     * @param pageWidth set the width of the resulting PDF in mm.
     */
    public void setPageWidth(int pageWidth) {
        this.width = pageWidth;
    }

    /**
     * @return get the height of the resulting PDF in mm.
     */
    public int getPageHeight() {
        return height;
    }

    /**
     * @param pageHeight set the height of the resulting PDF in mm.
     */
    public void setPageHeight(int pageHeight) {
        this.height = pageHeight;
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
     * @return the number of milliseconds to wait before creating the capture.
     */
    public int getDelay() {
        return delay;
    }

    /**
     * @param delay the number of milliseconds to wait before creating the capture.
     */
    public void setDelay(int delay) {
        this.delay = delay;
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
     * @return a template ID that specifies the header and footer of the PDF document.
     */
    public String getTemplateId() {
        return templateId;
    }

    /**
     * @param templateId set a template ID that specifies the header and footer of the PDF document.
     */
    public void setTemplateId(String templateId) {
        this.templateId = templateId;
    }

    /**
     * @return a custom watermark to add to the PDF.
     */
    public String getCustomWaterMarkId() {
        return customWaterMarkId;
    }

    /**
     * @param customWaterMarkId the custom watermark id.
     */
    public void setCustomWaterMarkId(String customWaterMarkId) {
        this.customWaterMarkId = customWaterMarkId;
    }

    /**
     * @return the quality of the PDF.
     */
    public int getQuality() {
        return quality;
    }

    /**
     * @param quality set the quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality.
     */
    public void setQuality(int quality) {
        this.quality = quality;
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
     * @return get the CSS selector of the only HTML element in the web page to capture.
     */
    public String getTargetElement() {
        return targetElement;
    }

    /**
     * @param targetElement set the CSS selector of the only HTML element in the web page to capture.
     */
    public void setTargetElement(String targetElement) {
        this.targetElement = targetElement;
    }    
    
    /**
     * @return get the CSS selector(s) of the one or more HTML elements in the web page to hide.
     */
    public String getHideElement() {
        return hideElement;
    }

    /**
     * @param hideElement set the CSS selector(s) of the one or more HTML elements in the web page to hide.
     */
    public void setHideElement(String hideElement) {
        this.hideElement = hideElement;
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
     * @return the ID of a capture that should be merged at the beginning of the new PDF document.
     */
    public String getMergeId() {
        return mergeId;
    }

    /**
     * @param mergeId set a ID of a capture that should be merged at the beginning of the new PDF document.
     */
    public void setMergeId(String mergeId) {
        this.mergeId = mergeId;
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
     * @return password that protects the PDF document.
     */
    public String getPassword() {
        return password;
    }

    /**
     * @param password the password to use to protect the PDF document with.
     */
    public void setPassword(String password) {
        this.password = password;
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
    
    /**
     * Define a custom Template parameter and value, this method can be called multiple times to add multiple parameters. 
     * 
     * @param name - The name of the template parameter
     * @param value - The value of the template parameter
     * @throws UnsupportedEncodingException 
     */
    public void AddTemplateParameter(String name, String value) throws UnsupportedEncodingException
    {
        this.templateVariables = appendParameter(this.templateVariables, name, value);
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
        + "|" + getCustomId() + "|" + ParameterUtility.toInt(includeBackground) + "|" + pagesize.getValue() + "|" + orientation.getValue() 
        + "|" + customWaterMarkId + "|" + ParameterUtility.toInt(includeLinks) + "|" + ParameterUtility.toInt(includeOutline)
        + "|" + title + "|" + coverURL + "|" + marginTop + "|" + marginLeft + "|" + marginBottom + "|" + marginRight
        + "|" + delay + "|" + requestAs.getValue() + "|" + getCountry().getValue() + "|" + quality + "|" + templateId + "|" + hideElement
        + "|" + targetElement + "|" + getExportURL() + "|" + waitForElement + "|" + getEncryptionKey()
        + "|" + ParameterUtility.toInt(noAds) + "|" + post + "|" + browserWidth + "|" + height + "|" + width + "|" + templateVariables
        + "|" + getProxy() + "|" + getMergeId() + "|" + address + "|" + ParameterUtility.toInt(noCookieNotifications) + "|" + cssMediaType.getValue()
        + "|" + password + "|" + clickElement;
    }    
    
    @Override
    public HashMap<String, String> _getParameters(String applicationKey, String sig, String callBackURL, String dataName, String dataValue) throws UnsupportedEncodingException
    {
        HashMap<String, String> params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue);		
        params.put("background", String.valueOf(ParameterUtility.toInt(includeBackground)));
        params.put("pagesize", pagesize.getValue());
        params.put("orientation", orientation.getValue());
        params.put("templateid", ParameterUtility.encode(ParameterUtility.nullCheck(templateId)));
        params.put("customwatermarkid", ParameterUtility.encode(ParameterUtility.nullCheck(customWaterMarkId)));
        params.put("includelinks", String.valueOf(ParameterUtility.toInt(includeLinks)));
        params.put("includeoutline", String.valueOf(ParameterUtility.toInt(includeOutline)));
        params.put("title", ParameterUtility.encode(ParameterUtility.nullCheck(title)));
        params.put("coverurl", ParameterUtility.encode(ParameterUtility.nullCheck(coverURL)));
        params.put("mleft", String.valueOf(marginLeft));
        params.put("mright", String.valueOf(marginRight));
        params.put("mtop", String.valueOf(marginTop));
        params.put("mbottom", String.valueOf(marginBottom));
        params.put("delay", String.valueOf(delay));
        params.put("requestmobileversion", String.valueOf(requestAs.getValue()));		
        params.put("quality", String.valueOf(quality));
        params.put("target", ParameterUtility.encode(ParameterUtility.nullCheck(targetElement)));
        params.put("hide", ParameterUtility.encode(ParameterUtility.nullCheck(hideElement)));
        params.put("waitfor", ParameterUtility.encode(ParameterUtility.nullCheck(waitForElement)));        
        params.put("noads", String.valueOf(ParameterUtility.toInt(noAds)));
        params.put("post", ParameterUtility.encode(ParameterUtility.nullCheck(post)));
        params.put("bwidth", String.valueOf(browserWidth));
        params.put("width", String.valueOf(width));
        params.put("height", String.valueOf(height));
        params.put("tvars", String.valueOf(templateVariables));
        params.put("mergeid", ParameterUtility.encode(ParameterUtility.nullCheck(mergeId)));
        params.put("nonotify", String.valueOf(ParameterUtility.toInt(noCookieNotifications)));
        params.put("address", ParameterUtility.encode(ParameterUtility.nullCheck(address)));
        params.put("media", String.valueOf(cssMediaType.getValue()));
        params.put("password", password);
        params.put("click", ParameterUtility.encode(ParameterUtility.nullCheck(clickElement)));
        
        return params;
    }    
}