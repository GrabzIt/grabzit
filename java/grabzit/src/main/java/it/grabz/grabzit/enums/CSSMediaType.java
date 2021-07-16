/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum CSSMediaType {
    PRINT("Print"),
    SCREEN("Screen");

    private String value;

    CSSMediaType(String value)
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
