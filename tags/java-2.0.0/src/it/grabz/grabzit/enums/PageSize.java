/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum PageSize {
    A3("A3"),
    A4("A4"),
    A5("A5"),
    B3("B3"),
    B4("B4"),
    B5("B5");

    private String value;

    PageSize(String value)
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
