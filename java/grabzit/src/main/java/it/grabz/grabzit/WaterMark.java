/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit;

import it.grabz.grabzit.enums.HorizontalPosition;
import it.grabz.grabzit.enums.VerticalPosition;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

/**
 * This class represents the custom watermarks stored in GrabzIt
 *
 * @version 3.0
 * @author GrabzIt
 */
@XmlRootElement(name="WaterMark")
public class WaterMark {
    @XmlElement(name="Identifier")
    private String Identifier;
    @XmlElement(name="Format")
    private String Format;
    @XmlElement(name="XPosition")
    private int XPosition;
    @XmlElement(name="YPosition")
    private int YPosition;

    /**
     * @return the identifier of the watermark
     */
    public String getIdentifier() {
        return Identifier;
    }

    /**
     * @return the format of the watermark  
     */
    public String getFormat() {
        return Format;
    }

    /**
     * @return the horizontal position of the watermark
     */
    public HorizontalPosition getXPosition() {
        return HorizontalPosition.values()[XPosition];
    }

    /**
     * @return the vertical position of the watermark
     */
    public VerticalPosition getYPosition() {
        return VerticalPosition.values()[YPosition];
    }
}
