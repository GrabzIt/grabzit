/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import java.io.IOException;
import java.io.InputStream;
import java.net.URLConnection;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;

/**
 *
 * @author GrabzIt
 */
class Response {
    @SuppressWarnings("unchecked")
    public <T> T Parse(URLConnection connection, Class<T> clazz) throws IOException, JAXBException, Exception {
        InputStream in = null;
        try {
            in = connection.getInputStream();
            JAXBContext context = JAXBContext.newInstance(clazz);
            Unmarshaller unmarshaller = context.createUnmarshaller();            
            return (T) unmarshaller.unmarshal(in);
        } finally {
            if (in != null) {
                in.close();
            }
            HttpUtility.CheckResponse(connection);
        }
    }    
}
