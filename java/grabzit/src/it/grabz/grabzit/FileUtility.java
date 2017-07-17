/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

/**
 *
 * @author Dominic
 */
class FileUtility {
    public static void Save(String path, byte[] data) throws FileNotFoundException, IOException
    {
        FileOutputStream fileOuputStream = null;
        try
        {
            fileOuputStream = new FileOutputStream(path);
            fileOuputStream.write(data);
        }
        finally
        {
            if (fileOuputStream != null)
            {
                fileOuputStream.close();
            }
        }
    }
}
