/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit;

import it.grabz.grabzit.enums.ErrorCode;

/**
 *
 * @author Administrator
 */
interface IMessageResult {
    public String getMessage();
    public ErrorCode getCode();
}
