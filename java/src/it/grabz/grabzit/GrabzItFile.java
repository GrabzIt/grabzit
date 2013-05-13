/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.grabzit;

import java.io.FileOutputStream;
import java.io.IOException;

/**
 *
 * @author Administrator
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
        FileOutputStream fileOuputStream = null;
        try
        {
            fileOuputStream = new FileOutputStream(path);
            fileOuputStream.write(bytes);
            fileOuputStream.close();
        }
        catch (Exception ex)
        {
            if (fileOuputStream != null)
            {
                fileOuputStream.close();
            }
            throw ex;
        }
    }
}
