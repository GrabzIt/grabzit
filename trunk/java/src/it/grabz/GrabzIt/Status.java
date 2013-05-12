/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.GrabzIt;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;

/**
 *
 * @author GrabzIt
 */
@XmlRootElement(name="WebResult")
public class Status {
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

    /**
     * @return the processing
     */
    public boolean isProcessing() {
        return processing;
    }

    /**
     * @return the cached
     */
    public boolean isCached() {
        return cached;
    }

    /**
     * @return the expired
     */
    public boolean isExpired() {
        return expired;
    }

    /**
     * @return the message
     */
    public String getMessage() {
        return message;
    }
}
