/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit.enums;

/**
 *
 * @author Administrator
 */
public enum VerticalPosition {
    TOP(0),
    MIDDLE(1),
    BOTTOM(2);

    private int value;

    VerticalPosition(int value)
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
