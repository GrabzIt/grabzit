/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;

/**
 *
 * @author Skinners
 */
@XmlRootElement(name = "Cookie")
public class Cookie {
    @XmlElement(name="Name")
    private String name;
    @XmlElement(name="Value")
    private String value;
    @XmlElement(name="Domain")
    private String domain;
    @XmlElement(name="Path")
    private String path;
    @XmlJavaTypeAdapter( BooleanAdapter.class )
    @XmlElement(name="HttpOnly")
    private Boolean httpOnly;
    @XmlElement(name="Expires")
    private String expires;
    @XmlElement(name="Type")
    private String type;   

    /**
     * @return the name
     */
    public String getName() {
        return name;
    }

    /**
     * @return the value
     */
    public String getValue() {
        return value;
    }

    /**
     * @return the domain
     */
    public String getDomain() {
        return domain;
    }

    /**
     * @return the path
     */
    public String getPath() {
        return path;
    }

    /**
     * @return the httpOnly
     */
    public Boolean getHttpOnly() {
        return httpOnly;
    }

    /**
     * @return the expires
     */
    public String getExpires() {
        return expires;
    }

    /**
     * @return the type
     */
    public String getType() {
        return type;
    }
}
