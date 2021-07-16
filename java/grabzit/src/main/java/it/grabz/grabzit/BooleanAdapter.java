/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import javax.xml.bind.annotation.adapters.XmlAdapter;

/**
 *
 * @author Skinners
 */
class BooleanAdapter extends XmlAdapter<String, Boolean> 
{
    @Override
    public Boolean unmarshal(String s)
    {
        return s == null ? false : "True".equals(s);
    }

    @Override
    public String marshal(Boolean c )
    {
        return c == null ? "False" : c ? "True" : "False";
    }
}
