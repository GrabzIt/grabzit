/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum HorizontalPosition {
    LEFT(0),
    CENTER(1),
    RIGHT(2);

    private int value;

    HorizontalPosition(int value)
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
