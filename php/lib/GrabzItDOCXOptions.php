<?php
namespace GrabzIt;

class GrabzItDOCXOptions extends GrabzItBaseOptions
{	
	private $includeBackground = true;
	private $pagesize = 'A4';
	private $orientation = 'Portrait';
	private $includeImages = true;	
	private $includeLinks = true;
	private $title = null;
	private $marginTop = 10;
	private $marginLeft = 10;
	private $marginBottom = 10;
	private $marginRight = 10;
	private $requestAs = 0;
	private $quality = -1;
	private $hideElement = null;
	private $waitForElement = null;
	private $noAds = false;
	private $templateId = null;
	private $targetElement = null;
	private $width = null;
	private $height = null;	
	private $browserWidth = null;
	private $templateVariables = null;
	private $mergeId = null;
	private $noCookieNotifications = false;
	private $address = null;
	private $password = null;
	
	/*
	Set the width of the resulting DOCX in mm.
	*/
	public function setPageWidth($value)
	{
		$this->width = $value;
	}

	/*
	Get the width of the resulting DOCX in mm.
	*/
	public function getPageWidth()
	{
		return $this->width;
	}

	/*
	Set the height of the resulting DOCX in mm.
	*/
	public function setPageHeight($value)
	{
		$this->height = $value;
	}

	/*
	Get the height of the resulting DOCX in mm.
	*/
	public function getPageHeight()
	{
		return $this->height;
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
	Set to true if background images should be included in the DOCX.
	*/
	public function setIncludeBackground($value)
	{
		$this->includeBackground = $value;
	}

	/*
	Get if the background images should be included in the DOCX.
	*/
	public function getIncludeBackground()
	{
		return $this->includeBackground;
	}

	/*
	Set the page size of the DOCX to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'.
	*/
	public function setPageSize($value)
	{
		$value = strtoupper($value);
		$this->pagesize = $value;
	}

	/*
	Get the page size of the DOCX to be returned.
	*/
	public function getPageSize()
	{
		return $this->pagesize;
	}

	/*
	Set the orientation of the DOCX to be returned: 'Landscape' or 'Portrait'.
	*/
	public function setOrientation($value)
	{
		$value = ucfirst($value);
		$this->orientation = $value;
	}

	/*
	Get the orientation of the DOCX to be returned.
	*/
	public function getOrientation()
	{
		return $this->orientation;
	}

	/*
	Set to true if links should be included in the DOCX.
	*/
	public function setIncludeLinks($value)
	{
		$this->includeLinks = $value;
	}

	/*
	Get if the links should be included in the DOCX.
	*/
	public function getIncludeLinks()
	{
		return $this->includeLinks;
	}

	/*
	Set to true if the DOCX should include non-background images.
	*/
	public function setIncludeImages($value)
	{
		$this->includeImages = $value;
	}

	/*
	Get if the DOCX should include non-background images.
	*/
	public function getIncludeImages()
	{
		return $this->includeImages;
	}

	/*
	Set a title for the DOCX document.
	*/
	public function setTitle($value)
	{
		$this->title = $value;
	}

	/*
	Get a title for the DOCX document.
	*/
	public function getTitle()
	{
		return $this->title;
	}
	
	/*
	Set the CSS selector of the only HTML element in the web page to capture.
	*/
	public function setTargetElement($value)
	{
		$this->targetElement = $value;
	}

	/*
	Get the CSS selector of the only HTML element in the web page to capture.
	*/
	public function getTargetElement()
	{
		return $this->targetElement;
	}
	
	/*
	Set the CSS selector(s) of the one or more HTML elements in the web page to hide.
	*/
	public function setHideElement($value)
	{
		$this->hideElement = $value;
	}

	/*
	Get the CSS selector(s) of the one or more HTML elements in the web page to hide.
	*/
	public function getHideElement()
	{
		return $this->hideElement;
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
	Set the margin that should appear at the top of the DOCX document page.
	*/
	public function setMarginTop($value)
	{
		$this->marginTop = $value;
	}

	/*
	Get the margin that should appear at the top of the DOCX document page.
	*/
	public function getMarginTop()
	{
		return $this->marginTop;
	}

	/*
	Set the margin that should appear at the left of the DOCX document page.
	*/
	public function setMarginLeft($value)
	{
		$this->marginLeft = $value;
	}

	/*
	Get the margin that should appear at the left of the DOCX document page.
	*/
	public function getMarginLeft()
	{
		return $this->marginLeft;
	}

	/*
	Set the margin that should appear at the bottom of the DOCX document page.
	*/
	public function setMarginBottom($value)
	{
		$this->marginBottom = $value;
	}

	/*
	Get the margin that should appear at the bottom of the DOCX document page.
	*/
	public function getMarginBottom()
	{
		return $this->marginBottom;
	}

	/*
	Set the margin that should appear at the right of the DOCX document.
	*/
	public function setMarginRight($value)
	{
		$this->marginRight = $value;
	}

	/*
	Get the margin that should appear at the right of the DOCX document.
	*/
	public function getMarginRight()
	{
		return $this->marginRight;
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
	Set the quality of the DOCX where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality.
	*/
	public function setQuality($value)
	{
		$this->quality = $value;
	}

	/*
	Get the quality of the DOCX.
	*/
	public function getQuality()
	{
		return $this->quality;
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
	Set a template ID that specifies the header and footer of the DOCX document.
	*/
	public function setTemplateId($value)
	{
		$this->templateId = $value;
	}

	/*
	Get the template ID that specifies the header and footer of the DOCX document.
	*/
	public function getTemplateId()
	{
		return $this->templateId;
	}

	/*
	Set the ID of a capture that should be merged at the beginning of the new DOCX document.
	*/
	public function setMergeId($value)
	{
		$this->mergeId = $value;
	}

	/*
	Get the ID of a capture that should be merged at the beginning of the new DOCX document.
	*/
	public function getMergeId()
	{
		return $this->mergeId;
	}
	
	
	/*
	Set password to protect the DOCX document.
	*/
	public function setPassword($value)
	{
		$this->password = $value;
	}

	/*
	Get password to protect the DOCX document with.
	*/
	public function getPassword()
	{
		return $this->password;
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
	
	/*
	Define a custom template parameter and value, this method can be called multiple times to add multiple parameters.

    name - The name of the template parameter.
	value - The value of the template parameter.
    */		
	public function AddTemplateParameter($name, $value)
	{
		$this->templateVariables = $this->appendParameter($this->templateVariables, $name, $value);
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
	  
		return $this->nullToEmpty($applicationSecret)."|". $urlParam . $callBackURLParam . "|".$this->nullToEmpty($this->getCustomId()) ."|".
		$this->nullToEmpty(intval($this->includeBackground)) ."|".
		$this->nullToEmpty($this->pagesize) ."|".$this->nullToEmpty($this->orientation)."|".
		$this->nullToEmpty(intval($this->includeImages))."|".$this->nullToEmpty(intval($this->includeLinks))."|".$this->nullToEmpty($this->title)."|".$this->nullToEmpty($this->marginTop)."|".$this->nullToEmpty($this->marginLeft)."|".$this->nullToEmpty($this->marginBottom)."|".
		$this->nullToEmpty($this->marginRight)."|".$this->nullToEmpty($this->delay)."|".$this->nullToEmpty(intval($this->requestAs))."|".
		$this->nullToEmpty($this->getCountry())."|".$this->nullToEmpty($this->quality)."|".
		$this->nullToEmpty($this->hideElement)."|".$this->nullToEmpty($this->getExportURL())."|".$this->nullToEmpty($this->waitForElement)."|".
		$this->nullToEmpty($this->getEncryptionKey())."|".$this->nullToEmpty(intval($this->noAds))."|".$this->nullToEmpty($this->post)."|".
		$this->nullToEmpty($this->targetElement)."|".$this->nullToEmpty($this->templateId)."|".$this->nullToEmpty($this->templateVariables)."|".
		$this->nullToEmpty($this->height)."|".$this->nullToEmpty($this->width)."|".$this->nullToEmpty($this->browserWidth)."|".$this->nullToEmpty($this->getProxy())."|".$this->nullToEmpty($this->mergeId)."|".$this->nullToEmpty($this->address)."|".$this->nullToEmpty(intval($this->noCookieNotifications))."|".$this->nullToEmpty($this->password);
	}

	public function _getParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = $this->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);		
		$params['background'] = $this->nullToEmpty(intval($this->includeBackground));
		$params['pagesize'] = $this->nullToEmpty($this->pagesize);
		$params['orientation'] = $this->nullToEmpty($this->orientation);
		$params['includelinks'] = $this->nullToEmpty(intval($this->includeLinks));
		$params['includeimages'] = $this->nullToEmpty(intval($this->includeImages));
		$params['title'] = $this->nullToEmpty($this->title);
		$params['mleft'] = $this->nullToEmpty($this->marginLeft);
		$params['mright'] = $this->nullToEmpty($this->marginRight);
		$params['mtop'] = $this->nullToEmpty($this->marginTop);
		$params['mbottom'] = $this->nullToEmpty($this->marginBottom);
		$params['delay'] = $this->nullToEmpty($this->delay);
		$params['requestmobileversion'] = $this->nullToEmpty(intval($this->requestAs));
		$params['quality'] = $this->nullToEmpty($this->quality);
		$params['hide'] = $this->nullToEmpty($this->hideElement);
		$params['waitfor'] = $this->nullToEmpty($this->waitForElement);
		$params['noads'] = $this->nullToEmpty(intval($this->noAds));
		$params['post'] = $this->nullToEmpty($this->post);
		$params['bwidth'] = $this->nullToEmpty($this->browserWidth);
		$params['width'] = $this->nullToEmpty($this->width);
		$params['height'] = $this->nullToEmpty($this->height);
		$params['tvars'] = $this->nullToEmpty($this->templateVariables);
		$params['target'] = $this->nullToEmpty($this->targetElement);
		$params['templateid'] = $this->nullToEmpty($this->templateId);
		$params['mergeid'] = $this->nullToEmpty($this->mergeId);
		$params['address'] = $this->nullToEmpty($this->address);
		$params['nonotify'] = $this->nullToEmpty(intval($this->noCookieNotifications));
		$params['password'] = $this->nullToEmpty($this->password);
		
		return $params;
	}
}
?>