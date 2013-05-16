/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Administrator
 */
@XmlRootElement(name="WebResult")
class TakeScreenShotResult implements IMessageResult {
    @XmlElement(name="Result")
    private String result;
    @XmlElement(name="Message")
    private String message;
    @XmlElement(name="ID")
    private String id;
    
    @Override
    public String getMessage() {
        return message;
    }
    
    public String getResult() {
        return result;
    }

    public String getId() {
        return id;
    }
}
