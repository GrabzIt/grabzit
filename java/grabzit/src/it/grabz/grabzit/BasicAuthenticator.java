/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import java.io.UnsupportedEncodingException;
import java.net.Authenticator;
import java.net.PasswordAuthentication;
import java.net.URLDecoder;

/**
 *
 * @version 3.2
 * @author GrabzIt
 */
public class BasicAuthenticator extends Authenticator  {
    private String username;
    private String password;
    
    public BasicAuthenticator(String username, String password){
        try{
            this.username = URLDecoder.decode(username, "UTF-8");
            this.password = URLDecoder.decode(password, "UTF-8");
        }
        catch(UnsupportedEncodingException e){}
    }
    
    @Override
    protected PasswordAuthentication getPasswordAuthentication() 
    {
        return new PasswordAuthentication(username, password.toCharArray());
    }
}
