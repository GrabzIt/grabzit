<?php
namespace GrabzIt;

class GrabzItAnimationOptions extends GrabzItBaseOptions
{	
	private $customWaterMarkId = null;
	private $width = null;
	private $height = null;	
	private $start = null; 
	private $duration = null;
	private $speed = null;
	private	$framesPerSecond = null;
	private $repeat = null;
	private $reverse = false;
	private $quality = -1;	

	/*
	Set to true if the frames of the animated GIF should be reversed.
	*/
	public function setReverse($value)
	{
		$this->reverse = $value;
	}

	/*
	Get if the frames of the animated GIF should be reversed.
	*/
	public function getReverse()
	{
		return $this->reverse;
	}	
	
	/*
	Set the number of times to loop the animated GIF. If 0 it will loop forever.
	*/
	public function setRepeat($value)
	{
		$this->repeat = $value;
	}

	/*
	Get the number of times to loop the animated GIF.
	*/
	public function getRepeat()
	{
		return $this->repeat;
	}		
	
	/*
	Set the number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60.
	*/
	public function setFramesPerSecond($value)
	{
		$this->framesPerSecond = $value;
	}

	/*
	Get the number of frames per second that should be captured from the video.
	*/
	public function getFramesPerSecond()
	{
		return $this->framesPerSecond;
	}	
	
	/*
	Set the speed of the animated GIF from 0.2 to 10 times the original speed.
	*/
	public function setSpeed($value)
	{
		$this->speed = $value;
	}

	/*
	Get the speed of the animated GIF.
	*/
	public function getSpeed()
	{
		return $this->speed;
	}		
	
	/*
	Set the length in seconds of the video that should be converted into a animated GIF.
	*/
	public function setDuration($value)
	{
		$this->duration = $value;
	}

	/*
	Get the length in seconds of the video that should be converted into a animated GIF.
	*/
	public function getDuration()
	{
		return $this->duration;
	}	
	
	/*
	Set the starting position of the video that should be converted into an animated GIF.
	*/
	public function setStart($value)
	{
		$this->start = $value;
	}

	/*
	Get the starting position of the video that should be converted into an animated GIF.
	*/
	public function getStart()
	{
		return $this->start;
	}	
	
	/*
	Set the width of the resulting animated GIF in pixels.
	*/
	public function setWidth($value)
	{
		$this->width = $value;
	}

	/*
	Get the width of the resulting animated GIF in pixels.
	*/
	public function getWidth()
	{
		return $this->width;
	}

	/*
	Set the height of the resulting animated GIF in pixels.
	*/
	public function setHeight($value)
	{
		$this->height = $value;
	}

	/*
	Get the height of the resulting animated GIF in pixels.
	*/
	public function getHeight()
	{
		return $this->height;
	}

	/*
	Set a custom watermark to add to the animated GIF.
	*/
	public function setCustomWaterMarkId($value)
	{
		$this->customWaterMarkId = $value;
	}

	/*
	Get the custom watermark id.
	*/
	public function getCustomWaterMarkId()
	{
		return $this->customWaterMarkId;
	}

	/*
	Set the quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality.
	*/
	public function setQuality($value)
	{
		$this->quality = $value;
	}

	/*
	Get the quality of the animated GIF.
	*/
	public function getQuality()
	{
		return $this->quality;
	}

	public function _getSignatureString($applicationSecret, $callBackURL, $url = null)
	{
		$urlParam = '';
		if ($url != null)
		{
			$urlParam = $this->nullToEmpty($url)."|";
		}		
		
		$callBackURLParam = '';
		if ($callBackURL != null)
		{
			$callBackURLParam = $this->nullToEmpty($callBackURL);
		}				
		
		return $this->nullToEmpty($applicationSecret)."|". $urlParam . $callBackURLParam .
		"|".$this->nullToEmpty($this->height)."|".$this->nullToEmpty($this->width)."|".$this->nullToEmpty($this->getCustomId())."|".
		$this->nullToEmpty($this->framesPerSecond)."|".$this->nullToEmpty($this->speed)."|".$this->nullToEmpty($this->duration)."|".
		$this->nullToEmpty($this->repeat)."|".$this->nullToEmpty(intval($this->reverse))."|".$this->nullToEmpty($this->start)."|".
		$this->nullToEmpty($this->customWaterMarkId)."|".$this->nullToEmpty($this->getCountry())."|".$this->nullToEmpty($this->quality)."|".
		$this->nullToEmpty($this->getExportURL())."|".$this->nullToEmpty($this->getEncryptionKey()."|".$this->nullToEmpty($this->getProxy()));  
	}
	
	public function _getParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = $this->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);		
		$params['width'] = $this->nullToEmpty($this->width);
		$params['height'] = $this->nullToEmpty($this->height);
		$params['duration'] = $this->nullToEmpty($this->duration);
		$params['speed'] = $this->nullToEmpty($this->speed);
		$params['customwatermarkid'] = $this->nullToEmpty($this->customWaterMarkId);
		$params['start'] = $this->nullToEmpty($this->start);
		$params['fps'] = $this->nullToEmpty($this->framesPerSecond);
		$params['repeat'] = $this->nullToEmpty($this->repeat);
		$params['reverse'] = $this->nullToEmpty(intval($this->reverse));		
		$params['quality'] = $this->nullToEmpty($this->quality);			
		
		return $params;
	}
}
?>