/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.GrabzIt;

import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Skinners
 */
@XmlRootElement(name="WebResult")
@XmlAccessorType(XmlAccessType.FIELD)
class Cookies {
    @XmlElement(name="Cookies", type=Cookie.class)
    private List<Cookie> cookies;
    @XmlElement(name="Message")
    private String message;

    /**
     * @return the cookies
     */
    public List<Cookie> getCookies() {
        return cookies;
    }

    /**
     * @return the message
     */
    public String getMessage() {
        return message;
    }
}
