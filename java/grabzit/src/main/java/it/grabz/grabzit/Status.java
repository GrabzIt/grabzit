/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.ErrorCode;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;

/**
 * This class represents the screenshot status
 *
 * @version 3.0
 * @author GrabzIt
 */
@XmlRootElement(name="WebResult")
public class Status implements IMessageResult {
    @XmlElement(name="Processing")
    @XmlJavaTypeAdapter( BooleanAdapter.class )
    private Boolean processing;
    @XmlElement(name="Cached")
    @XmlJavaTypeAdapter( BooleanAdapter.class )
    private Boolean cached;
    @XmlElement(name="Expired")
    @XmlJavaTypeAdapter( BooleanAdapter.class )
    private Boolean expired;
    @XmlElement(name="Message")
    private String message;
    @XmlElement(name="Code")
    private String code;    

    /**
     * @return if true the screenshot is still being processed
     */
    public boolean isProcessing() {
        return processing;
    }

    /**
     * @return if true the screenshot has been cached
     */
    public boolean isCached() {
        return cached;
    }

    /**
     * @return if true the screenshot has expired
     */
    public boolean isExpired() {
        return expired;
    }

    /**
     * @return returns any error messages associated with the screenshot
     */
    public String getMessage() {
        return message;
    }
    
    @Override
    public ErrorCode getCode() {
        return ErrorCode.valueOf(Integer.parseInt(code));
    }    
}
