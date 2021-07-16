/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.enums.ErrorCode;

/**
 *
 * @author GrabzIt
 */
public class GrabzItException extends Exception {
    private ErrorCode code;

    public GrabzItException(String message, ErrorCode code, Exception innerException)
    {        
        super(message);
        this.initCause(innerException);
        this.code = code;
    }
    
    public GrabzItException(String message, ErrorCode code)
    {
        this(message, code, null);
    }
    
    public GrabzItException(String message, String code)
    {
        this(message, ErrorCode.values()[Integer.parseInt(code)]);
    }
    /**
     * @return the code
     */
    public ErrorCode getCode() {
        return code;
    }
    
    
}
