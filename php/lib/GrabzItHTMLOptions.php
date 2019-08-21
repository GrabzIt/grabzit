<?php
namespace GrabzIt;

class GrabzItHTMLOptions extends GrabzItBaseOptions
{	
	private $requestAs = 0;
	private $browserWidth = null;
	private $browserHeight = null;
	private $waitForElement = null;
	private $noAds = false;
	private $noCookieNotifications = false;
	private $address = null;
	
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
	Set the height of the browser in pixels. Use -1 to screenshot the whole web page.
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
	Set the number of milliseconds to wait before creating the capture.
	*/
	public function setDelay($value)
	{
		$this->delay = $value;
	}

	/*
	Get the number of milliseconds to wait before creating the capture.
	*/
	public function getDelay()
	{
		return $this->delay;
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
	Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
	GrabzIt to perform a HTTP post.

    name - The name of the HTTP Post parameter.
	value - The value of the HTTP Post parameter.
    */		
	public function AddPostParameter($name, $value)
	{
		$this->post = $this->appendParameter($this->post, $name, $value);
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
		"|".$this->nullToEmpty($this->browserHeight)
		."|".$this->nullToEmpty($this->browserWidth)."|".$this->nullToEmpty($this->getCustomId())."|".$this->nullToEmpty($this->delay)."|".$this->nullToEmpty(intval($this->requestAs))."|".$this->nullToEmpty($this->getCountry())."|".
		$this->nullToEmpty($this->getExportURL())."|".
		$this->nullToEmpty($this->waitForElement)."|".$this->nullToEmpty($this->getEncryptionKey())."|".$this->nullToEmpty(intval($this->noAds))."|".$this->nullToEmpty($this->post)."|".$this->nullToEmpty($this->getProxy())."|".$this->nullToEmpty($this->address)."|".$this->nullToEmpty(intval($this->noCookieNotifications));  
	}
	
	public function _getParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = $this->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);		
		$params['bwidth'] = $this->nullToEmpty($this->browserWidth);
		$params['bheight'] = $this->nullToEmpty($this->browserHeight);
		$params['delay'] = $this->nullToEmpty($this->delay);
		$params['requestmobileversion'] = $this->nullToEmpty(intval($this->requestAs));		
		$params['waitfor'] = $this->nullToEmpty($this->waitForElement);
		$params['noads'] = $this->nullToEmpty(intval($this->noAds));
		$params['post'] = $this->nullToEmpty($this->post);	
		$params['address'] = $this->nullToEmpty($this->address);
		$params['nonotify'] = $this->nullToEmpty(intval($this->noCookieNotifications));

		return $params;
	}
}
?>