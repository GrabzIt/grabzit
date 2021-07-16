/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit;

import it.grabz.grabzit.parameters.BaseOptions;

class Request {
    private String url;
    private boolean isPost;
    private BaseOptions options;
    private String data; 
    
    public Request(String url, boolean isPost, BaseOptions options, String data)
    {
        this.url = url;
        this.isPost = isPost;
        this.options = options;
        this.data = data;
    }

    /**
     * @return the url
     */
    public String getUrl() {
        return url;
    }
    
    public String getTargetUrl() {
        if (this.isPost)
        {
            return "";
        }
        return this.data;
    }    

    /**
     * @return the isPost
     */
    public boolean isIsPost() {
        return isPost;
    }

    /**
     * @return the options
     */
    public BaseOptions getOptions() {
        return options;
    }

    /**
     * @return the data
     */
    public String getData() {
        return data;
    }
}
