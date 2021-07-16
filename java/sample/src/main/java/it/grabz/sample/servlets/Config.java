/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package it.grabz.sample.servlets;

import java.io.IOException;
import java.util.Properties;

/**
 *
 * @author Administrator
 */
public final class Config {

    private static final Properties properties = new Properties();

    static {
        try {
            ClassLoader loader = Thread.currentThread().getContextClassLoader();
            properties.load(loader.getResourceAsStream("config.properties"));
        } catch (IOException e) {
            throw new ExceptionInInitializerError(e);
        }
    }

    private static String getSetting(String key) {
        return properties.getProperty(key);
    }

    public static String getApplicationKey()
    {
        return getSetting("ApplicationKey");
    }

    public static String getApplicationSecret()
    {
        return getSetting("ApplicationSecret");
    }

    public static String getHandlerUrl()
    {
        return getSetting("HandlerUrl");
    }
}
