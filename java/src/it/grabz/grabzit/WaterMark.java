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
 *
 * @author Administrator
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
     * @return the Identifier
     */
    public String getIdentifier() {
        return Identifier;
    }

    /**
     * @return the Format
     */
    public String getFormat() {
        return Format;
    }

    /**
     * @return the XPosition
     */
    public HorizontalPosition getXPosition() {
        return HorizontalPosition.values()[XPosition];
    }

    /**
     * @return the YPosition
     */
    public VerticalPosition getYPosition() {
        return VerticalPosition.values()[YPosition];
    }
}
