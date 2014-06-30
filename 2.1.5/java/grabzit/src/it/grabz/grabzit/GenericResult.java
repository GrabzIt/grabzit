/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit;

import it.grabz.grabzit.enums.ErrorCode;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author Administrator
 */
@XmlRootElement(name="WebResult")
class GenericResult implements IMessageResult {
    @XmlElement(name="Result")
    private String result;
    @XmlElement(name="Message")
    private String message;
    @XmlElement(name="Code")
    private String code;    

    @Override
    public String getMessage() {
        return message;
    }
    
    public String getResult() {
        return result;
    }
    
    @Override
    public ErrorCode getCode() {
        return ErrorCode.valueOf(Integer.parseInt(code));
    }    
}
