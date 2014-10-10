/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum PageOrientation {
    LANDSCAPE("Landscape"),
    PORTRAIT("Portrait");

    private String value;

    PageOrientation(String value)
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
