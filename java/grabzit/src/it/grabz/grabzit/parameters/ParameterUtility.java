/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.parameters;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;

/**
 * @version 3.0
 * @author GrabzIt
 */
public class ParameterUtility {
    public static String encode(String value) throws UnsupportedEncodingException
    {
        return URLEncoder.encode(value, "UTF-8");
    }

    public static String nullCheck(String value)
    {
        if (value == null)
        {
            return "";
        }
        return value;
    }    
    
    public static String toString(float value)
    {
        if ((value % 1) == 0)
        {
            return String.valueOf(((int)value));
        }
        return String.valueOf(value);
    }
    
    public static int toInt(boolean value)
    {
        return (value ? 1 : 0);
    }    
}
