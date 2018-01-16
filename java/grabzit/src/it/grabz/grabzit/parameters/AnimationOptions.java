/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package it.grabz.grabzit.parameters;

import java.io.UnsupportedEncodingException;
import java.util.HashMap;

/**
 * Contains all of the available options when creating an animated GIF conversion.
 * 
 * @version 3.0
 * @author GrabzIt
 */
public class AnimationOptions extends BaseOptions{
    private int width;
    private int height;
    private int start;
    private int duration;
    private float speed;
    private float framesPerSecond;
    private int repeat;
    private boolean reverse;
    private String customWaterMarkId;
    private int quality; 

    public AnimationOptions()
    {
        this.width = 0;
        this.height = 0;
        this.start = 0;
        this.duration = 1;
        this.speed = 0;
        this.framesPerSecond = 0;
        this.repeat = 0;
        this.reverse = false;
        this.customWaterMarkId = "";
        this.quality = -1;
    }
    
    /**
     * @return the width of the resulting animated GIF in pixels.
     */
    public int getWidth() {
        return width;
    }

    /**
     * @param width the width of the resulting animated GIF in pixels.
     */
    public void setWidth(int width) {
        this.width = width;
    }

    /**
     * @return the height of the resulting animated GIF in pixels.
     */
    public int getHeight() {
        return height;
    }

    /**
     * @param height the height of the resulting animated GIF in pixels.
     */
    public void setHeight(int height) {
        this.height = height;
    }

    /**
     * @return the starting position of the video that should be converted into an animated GIF.
     */
    public int getStart() {
        return start;
    }

    /**
     * @param start the starting position of the video that should be converted into an animated GIF.
     */
    public void setStart(int start) {
        this.start = start;
    }

    /**
     * @return the length in seconds of the video that should be converted into a animated GIF.
     */
    public int getDuration() {
        return duration;
    }

    /**
     * @param duration the length in seconds of the video that should be converted into a animated GIF.
     */
    public void setDuration(int duration) {
        this.duration = duration;
    }

    /**
     * @return the the speed of the animated GIF.
     */
    public float getSpeed() {
        return speed;
    }

    /**
     * @param speed the speed of the animated GIF from 0.2 to 10 times the original speed.
     */
    public void setSpeed(float speed) {
        this.speed = speed;
    }

    /**
     * @return the number of frames per second that should be captured from the video.
     */
    public float getFramesPerSecond() {
        return framesPerSecond;
    }

    /**
     * @param framesPerSecond the number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60.
     */
    public void setFramesPerSecond(float framesPerSecond) {
        this.framesPerSecond = framesPerSecond;
    }

    /**
     * @return the number of times to loop the animated GIF.
     */
    public int getRepeat() {
        return repeat;
    }

    /**
     * @param repeat the number of times to loop the animated GIF. If 0 it will loop forever.
     */
    public void setRepeat(int repeat) {
        this.repeat = repeat;
    }

    /**
     * @return if the frames of the animated GIF should be reversed.
     */
    public boolean isReverse() {
        return reverse;
    }

    /**
     * @param reverse set to true if the frames of the animated GIF should be reversed.
     */
    public void setReverse(boolean reverse) {
        this.reverse = reverse;
    }

    /**
     * @return the custom watermark id.
     */
    public String getCustomWaterMarkId() {
        return customWaterMarkId;
    }

    /**
     * @param customWaterMarkId a custom watermark to add to the animated GIF.
     */
    public void setCustomWaterMarkId(String customWaterMarkId) {
        this.customWaterMarkId = customWaterMarkId;
    }

    /**
     * @return the quality of the animated GIF.
     */
    public int getQuality() {
        return quality;
    }

    /**
     * @param quality the quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality.
     */
    public void setQuality(int quality) {
        this.quality = quality;
    }

    public String _getSignatureString(String applicationSecret, String callBackURL)
    {
        return _getSignatureString(applicationSecret, callBackURL, null);
    }    
    
    @Override    
    public String _getSignatureString(String applicationSecret, String callBackURL, String url)
    {
        String urlParam = "";
        if (url != null && !"".equals(url))
        {
            urlParam = ParameterUtility.nullCheck(url)+"|";
        }		

        String callBackURLParam = "";
        if (callBackURL != null && !"".equals(callBackURL))
        {
            callBackURLParam = ParameterUtility.nullCheck(callBackURL);
        }				

        return ParameterUtility.nullCheck(applicationSecret) + "|" + urlParam + callBackURLParam
        + "|" + height + "|" + width + "|" + getCustomId() + "|" + ParameterUtility.toString(framesPerSecond) 
        + "|" + ParameterUtility.toString(speed) + "|" + duration + "|" + repeat + "|" + ParameterUtility.toInt(reverse)
        + "|" + start + "|" + customWaterMarkId + "|" + getCountry().getValue() + "|" + quality + "|" + getExportURL()
        + "|" + getEncryptionKey() + "|" + getProxy();
    }    

    @Override
    public HashMap<String, String> _getParameters(String applicationKey, String sig, String callBackURL, String dataName, String dataValue) throws UnsupportedEncodingException
    {
        HashMap<String, String> params = createParameters(applicationKey, sig, callBackURL, dataName, dataValue);		
        params.put("width", String.valueOf(width));
        params.put("height", String.valueOf(height));
        params.put("duration", String.valueOf(duration));
        params.put("speed", ParameterUtility.toString(speed));
        params.put("customwatermarkid", ParameterUtility.nullCheck(customWaterMarkId));
        params.put("start", String.valueOf(start));
        params.put("fps", ParameterUtility.toString(framesPerSecond));
        params.put("repeat", String.valueOf(repeat));
        params.put("reverse", String.valueOf(ParameterUtility.toInt(reverse)));		
        params.put("quality", String.valueOf(quality));			

        return params;
    }    
}
