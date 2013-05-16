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
class WaterMarks implements IMessageResult {
    @XmlElementWrapper(name="WaterMarks")
    @XmlElement( name="WaterMark" )
    private WaterMark[] watermarks;
    @XmlElement(name="Message")
    private String message;

    public WaterMark[] getWaterMarks() {
        return watermarks;
    }

    @Override
    public String getMessage() {
        return message;
    }
}
