/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.parameters;

import it.grabz.grabzit.enums.BrowserType;
import it.grabz.grabzit.enums.TableFormat;
import java.io.UnsupportedEncodingException;
import java.util.HashMap;

/**
 * Contains all of the available options when creating a table capture.
 * 
 * @version 3.0
 * @author GrabzIt
 */
public class TableOptions extends BaseOptions {
    private int tableNumberToInclude;
    private TableFormat format;
    private boolean includeHeaderNames;
    private boolean includeAllTables;
    private String targetElement;
    private BrowserType requestAs;
    private String address;
    
    public TableOptions()
    {
        this.tableNumberToInclude = 1;
        this.format = TableFormat.CSV;
        this.includeHeaderNames = true;
        this.includeAllTables = false;
        this.targetElement = "";
        this.requestAs = BrowserType.STANDARDBROWSER;
        this.address = "";
    }

    /**
     * @return the index of the table to be converted.
     */
    public int getTableNumberToInclude() {
        return tableNumberToInclude;
    }

    /**
     * @param tableNumberToInclude set the index of the table to be converted, were all tables in a web page are ordered from the top of the web page to bottom.
     */
    public void setTableNumberToInclude(int tableNumberToInclude) {
        this.tableNumberToInclude = tableNumberToInclude;
    }

    /**
     * @return the format of the table should be.
     */
    public TableFormat getFormat() {
        return format;
    }

    /**
     * @param format the format the table should be in: csv, xlsx or json.
     */
    public void setFormat(TableFormat format) {
        this.format = format;
    }

    /**
     * @return true if the header names are included in the table.
     */
    public boolean isIncludeHeaderNames() {
        return includeHeaderNames;
    }

    /**
     * @param includeHeaderNames set to true to include header names into the table.
     */
    public void setIncludeHeaderNames(boolean includeHeaderNames) {
        this.includeHeaderNames = includeHeaderNames;
    }

    /**
     * @return if all the tables on the web page will be extracted with each table appearing in a separate spreadsheet sheet.
     */
    public boolean isIncludeAllTables() {
        return includeAllTables;
    }

    /**
     * @param includeAllTables set to true to extract every table on the web page into a separate spreadsheet sheet. Only available with the XLSX and JSON formats.
     */
    public void setIncludeAllTables(boolean includeAllTables) {
        this.includeAllTables = includeAllTables;
    }

    /**
     * @return the id of the only HTML element in the web page that should be used to extract tables from.
     */
    public String getTargetElement() {
        return targetElement;
    }

    /**
     * @param targetElement the id of the only HTML element in the web page that should be used to extract tables from.
     */
    public void setTargetElement(String targetElement) {
        this.targetElement = targetElement;
    }

    /**
     * @return which user agent type should be used
     */
    public BrowserType getRequestAs() {
        return requestAs;
    }

    /**
     * @param requestAs set which user agent type should be used
     */
    public void setRequestAs(BrowserType requestAs) {
        this.requestAs = requestAs;
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
        +"|"+getCustomId()+"|"+tableNumberToInclude+"|"+ParameterUtility.toInt(includeAllTables)
        +"|"+ParameterUtility.toInt(includeHeaderNames)+"|"+targetElement+"|"+format.getValue()+"|"+requestAs.getValue()
        +"|"+getCountry().getValue() + "|" + getExportURL() + "|" + getEncryptionKey()
        +"|"+post + "|" + getProxy() + "|" + address;
    }    

    @Override
    public HashMap<String, String>  _getParameters(String applicationKey, String sig, String callBackURL, String dataName, String dataValue) throws UnsupportedEncodingException
    {
        HashMap<String, String> params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue);		
        params.put("includeAllTables", String.valueOf(ParameterUtility.toInt(includeAllTables)));
        params.put("includeHeaderNames", String.valueOf(ParameterUtility.toInt(includeHeaderNames)));
        params.put("format", format.getValue());
        params.put("tableToInclude", String.valueOf(tableNumberToInclude));
        params.put("target", ParameterUtility.encode(ParameterUtility.nullCheck(targetElement)));
        params.put("requestmobileversion", String.valueOf(requestAs.getValue()));			
        params.put("post", ParameterUtility.encode(ParameterUtility.nullCheck(post)));
        params.put("address", ParameterUtility.encode(ParameterUtility.nullCheck(address)));
        
        return params;
    }    
}
