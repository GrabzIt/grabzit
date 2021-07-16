/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum ImageFormat {
    BMP8("bmp8"),
    BMP16("bmp16"),
    BMP24("bmp24"),
    BMP("bmp"),
    JPG("jpg"),
    TIFF("tiff"),
    PNG("png"),
    WEBP("webp");

    private String value;

    ImageFormat(String value)
    {
        this.value = value;
    }

    /**
     * @return the value
     */
    public String getValue() {
        return value;
    }
}
