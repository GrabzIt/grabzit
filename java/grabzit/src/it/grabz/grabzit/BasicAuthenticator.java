/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import java.net.Authenticator;
import java.net.PasswordAuthentication;

/**
 *
 * @version 3.0
 * @author GrabzIt
 */
public class BasicAuthenticator extends Authenticator  {
    private String username;
    private String password;
    
    public BasicAuthenticator(String username, String password){
        this.username = username;
        this.password = password;
    }
    
    @Override
    protected PasswordAuthentication getPasswordAuthentication() 
    {
        return new PasswordAuthentication(username, password.toCharArray());
    }
}
