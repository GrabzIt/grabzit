/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Skinners
 */
@XmlRootElement(name="WebResult")
class Cookies implements IMessageResult {
    @XmlElementWrapper(name="Cookies")
    @XmlElement( name="Cookie" )
    private Cookie[] cookies;
    @XmlElement(name="Message")
    private String message;

    public Cookie[] getCookies() {
        return cookies;
    }

    @Override
    public String getMessage() {
        return message;
    }
}
