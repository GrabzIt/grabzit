<?php
namespace GrabzIt;

class GrabzItVideoOptions extends GrabzItBaseOptions
{	
	private $customWaterMarkId = null;
	private $browserWidth = null;
	private $browserHeight = null;
	private $width = null;
	private $height = null;	
	private $start = null; 
	private $duration = null;
	private	$framesPerSecond = null;
	private $waitForElement = null;
	private $requestAs = 0;
	private $clickElement = null;
	private $noAds = false;
	private $noCookieNotifications = false;
	private $address = null;		
	
	/*
	Set the number of frames per second that should be used to create the video. From a minimum of 0.2 to a maximum of 10.
	*/
	public function setFramesPerSecond($value)
	{
		$this->framesPerSecond = $value;
	}

	/*
	Get the number of frames per second that should be used to create the video.
	*/
	public function getFramesPerSecond()
	{
		return $this->framesPerSecond;
	}	
	
	/*
	Set the length in seconds of the web page that should be converted into a video.
	*/
	public function setDuration($value)
	{
		$this->duration = $value;
	}

	/*
	Get the length in seconds of the web page that should be converted into a video.
	*/
	public function getDuration()
	{
		return $this->duration;
	}	
	
	/*
	Set the starting time of the web page that should be converted into a video.
	*/
	public function setStart($value)
	{
		$this->start = $value;
	}

	/*
	Get starting time of the web page that should be converted into a video.
	*/
	public function getStart()
	{
		return $this->start;
	}	
	
	/*
	Set the width of the browser in pixels.
	*/
	public function setBrowserWidth($value)
	{
		$this->browserWidth = $value;
	}

	/*
	Get the width of the browser in pixels.
	*/
	public function getBrowserWidth()
	{
		return $this->browserWidth;
	}	

	/*
	Set the height of the browser in pixels.
	*/
	public function setBrowserHeight($value)
	{
		$this->browserHeight = $value;
	}

	/*
	Get the height of the browser in pixels.
	*/
	public function getBrowserHeight()
	{
		return $this->browserHeight;
	}	

	/*
	Set the width of the resulting video in pixels
	*/
	public function setWidth($value)
	{
		$this->width = $value;
	}

	/*
	Get the width of the resulting video in pixels
	*/
	public function getWidth()
	{
		return $this->width;
	}

	/*
	Set the height of the resulting video in pixels
	*/
	public function setHeight($value)
	{
		$this->height = $value;
	}

	/*
	Get the height of the resulting video in pixels
	*/
	public function getHeight()
	{
		return $this->height;
	}	

	/*
	Set a custom watermark to add to the video.
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
	Set the CSS selector of the HTML element in the web page that must be visible before the capture is performed.
	*/
	public function setWaitForElement($value)
	{
		$this->waitForElement = $value;
	}

	/*
	Get the CSS selector of the HTML element in the web page that must be visible before the capture is performed.
	*/
	public function getWaitForElement()
	{
		return $this->waitForElement;
	}

	/*
	Set which user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3.
	*/
	public function setRequestAs($value)
	{
		$this->requestAs = $value;
	}

	/*
	Get which user agent type should be used.
	*/
	public function getRequestAs()
	{
		return $this->requestAs;
	}
	
	/*
	Set to true if adverts should be automatically hidden.
	*/
	public function setNoAds($value)
	{
		$this->noAds = $value;
	}

	/*
	Get if adverts should be automatically hidden.
	*/
	public function getNoAds()
	{
		return $this->noAds;
	}		
	
	/*
	Set to true if cookie notification should be automatically hidden.
	*/
	public function setNoCookieNotifications($value)
	{
		$this->noCookieNotifications = $value;
	}

	/*
	Get if cookie notification should be automatically hidden.
	*/
	public function getNoCookieNotifications()
	{
		return $this->noCookieNotifications;
	}	

	/*
	Set the URL to execute the HTML code in.
	*/
	public function setAddress($value)
	{
		$this->address = $value;
	}

	/*
	Get the URL to execute the HTML code in.
	*/
	public function getAddress()
	{
		return $this->address;
	}	

	/*
	Set the CSS selector of the HTML element in the web page to click.
	*/
	public function setClickElement($value)
	{
		$this->clickElement = $value;
	}

	/*
	Get the CSS selector of the HTML element in the web page to click.
	*/
	public function getClickElement()
	{
		return $this->clickElement;
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
		"|".$this->nullToEmpty($this->browserHeight)."|".$this->nullToEmpty($this->browserWidth)."|".$this->nullToEmpty($this->getCustomId())."|".
		$this->nullToEmpty($this->customWaterMarkId)."|".$this->nullToEmpty($this->start)."|".
		$this->nullToEmpty(intval($this->requestAs))."|".$this->nullToEmpty($this->getCountry())."|".$this->nullToEmpty($this->getExportURL())."|".
		$this->nullToEmpty($this->waitForElement)."|".$this->nullToEmpty($this->getEncryptionKey())."|".
		$this->nullToEmpty(intval($this->noAds))."|".$this->nullToEmpty($this->post)."|".$this->nullToEmpty($this->getProxy())."|".$this->nullToEmpty($this->address)."|".$this->nullToEmpty(intval($this->noCookieNotifications))."|".
		$this->nullToEmpty($this->clickElement)."|".$this->nullToEmpty($this->framesPerSecond)."|".$this->nullToEmpty($this->duration)."|".$this->nullToEmpty($this->width)."|".$this->nullToEmpty($this->height);  
	}
	
	public function _getParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = $this->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);		
		$params['bwidth'] = $this->nullToEmpty($this->browserWidth);
		$params['bheight'] = $this->nullToEmpty($this->browserHeight);
		$params['width'] = $this->nullToEmpty($this->width);
		$params['height'] = $this->nullToEmpty($this->height);		
		$params['duration'] = $this->nullToEmpty($this->duration);
		$params['waitfor'] = $this->nullToEmpty($this->waitForElement);
		$params['customwatermarkid'] = $this->nullToEmpty($this->customWaterMarkId);
		$params['start'] = $this->nullToEmpty($this->start);
		$params['fps'] = $this->nullToEmpty($this->framesPerSecond);
		$params['requestmobileversion'] = $this->nullToEmpty($this->requestAs);
		$params['noads'] = $this->nullToEmpty(intval($this->noAds));		
		$params['post'] = $this->nullToEmpty($this->post);	
		$params['address'] = $this->nullToEmpty($this->address);
		$params['nonotify'] = $this->nullToEmpty(intval($this->noCookieNotifications));
		$params['click'] = $this->nullToEmpty($this->clickElement);
		
		return $params;
	}
}
?>