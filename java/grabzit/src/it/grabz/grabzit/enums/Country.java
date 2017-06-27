/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum Country {
    SINGAPORE("SG"),
    UNITEDSTATES("US"),
    UNITEDKINGDOM("UK"),
    DEFAULT("");

    private String value;

    Country(String value)
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
