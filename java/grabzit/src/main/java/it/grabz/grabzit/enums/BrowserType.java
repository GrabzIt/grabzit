/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum BrowserType {
    STANDARDBROWSER(0),
    MOBILEBROWSER(1),
    SEARCHENGINE(2);

    private int value;

    BrowserType(int value)
    {
        this.value = value;
    }

    /**
     * @return the value
     */
    public int getValue() {
        return value;
    }
}
