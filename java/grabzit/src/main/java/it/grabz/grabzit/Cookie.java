/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.adapters.XmlJavaTypeAdapter;

/**
 * This class represents the cookies stored in GrabzIt
 *
 * @version 3.0
 * @author GrabzIt
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
     * @return the name of the cookie
     */
    public String getName() {
        return name;
    }

    /**
     * @return the value of the cookie
     */
    public String getValue() {
        return value;
    }

    /**
     * @return the domain of the cookie
     */
    public String getDomain() {
        return domain;
    }

    /**
     * @return the path of the cookie
     */
    public String getPath() {
        return path;
    }

    /**
     * @return is the cookie httponly
     */
    public Boolean getHttpOnly() {
        return httpOnly;
    }

    /**
     * @return the date and time the cookie expires
     */
    public String getExpires() {
        return expires;
    }

    /**
     * @return the type of cookie
     */
    public String getType() {
        return type;
    }
}
