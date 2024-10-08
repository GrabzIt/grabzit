/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit;

import java.io.IOException;
import java.nio.charset.Charset;

/**
 * Represents the capture file generated by GrabzIt
 * 
 * @version 3.0
 * @author GrabzIt
 */
public class GrabzItFile {
    private byte[] bytes;

    public GrabzItFile(byte[] bytes)
    {
        this.bytes = bytes;
    }

    /**
     * Save GrabzItFile to physical file.
     * @param path The path to save the file to
     * @throws IOException
     * @throws Exception
     */
    public void Save(String path) throws IOException, Exception
    {
        FileUtility.Save(path, getBytes());
    }

    /**
     * @return the raw bytes of the screenshot file
     */
    public byte[] getBytes() {
        return bytes;
    }
    
    /**
     * Only use this with text based formats such as CSV or JSON otherwise it will return a string that represents a byte array.
     * @return a string representing the file
     */
    @Override
    public String toString()
    {
        if (this.bytes != null)
        {
            return new String(this.bytes, Charset.forName("UTF-8"));
        }
        return "";
    }
}
