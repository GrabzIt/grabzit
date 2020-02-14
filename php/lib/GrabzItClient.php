<?php
namespace GrabzIt;

spl_autoload_register(function ($class_name) {
	$file_name = str_replace("GrabzIt\\", '', $class_name) . '.php';
	if (strpos($file_name, "GrabzIt") === 0) {
		include($file_name);
	}
});

class GrabzItClient
{
	const WebServicesBaseURL = "://api.grabz.it/services/";
	const TakePicture = "takepicture.ashx";
	const TakeTable = "taketable.ashx";
	const TakePDF = "takepdf.ashx";
	const TakeDOCX = "takedocx.ashx";
	const TakeHTML = "takehtml.ashx";
	const TrueString = "True";

	private $applicationKey;
	private $applicationSecret;
	private $request;
	private $connectionTimeout = 600;
	private $protocol = "http";
	private $proxy = null;

	public function __construct($applicationKey, $applicationSecret)
	{
		$this->applicationKey = $applicationKey;
		$this->applicationSecret = $applicationSecret;
	}

	public function SetTimeout($timeout)
	{
		$this->connectionTimeout = $timeout;
	}

	public function SetApplicationKey($applicationKey)
	{
		$this->applicationKey = $applicationKey;
	}

	public function GetApplicationKey()
	{
		return $this->applicationKey;
	}

	public function SetApplicationSecret($applicationSecret)
	{
		$this->applicationSecret = $applicationSecret;
	}

	public function GetApplicationSecret()
	{
		return $this->applicationSecret;
	}
	
	/*
	This method sets if requests to GrabzIt's API should use SSL or not

	value - true if should use SSL
	*/
	public function UseSSL($value)
	{
		if ($value)
		{
			$this->protocol = "https";
			return;
		}
		$this->protocol = "http";
	}
	
	
	/*
	This method enables a local proxy server to be used for all requests
	
	proxyUrl - the URL, which can include a port if required, of the proxy. Providing a null will remove any previously set proxy
	*/
	public function SetLocalProxy($proxyUrl)
	{
		if ($proxyUrl == null || empty($proxy))
		{
			$this->proxy = null;
			return;
		}
		
		$url_parts = parse_url($proxyUrl);
		
		$this->proxy = new stdClass();
		$this->proxy->host = $url_parts['host'];
		$this->proxy->port = $url_parts['port'];
		$this->proxy->username = null;
		$this->proxy->password = null;
		
		if (isset($url_parts['user']))
		{
			$this->proxy->username = urldecode($url_parts['user']);
		}
		if (isset($url_parts['pass']))
		{
			$this->proxy->password = urldecode($url_parts['pass']);
		}
	}
	
	/*
	This method creates a cryptographically secure encryption key to pass to the encryption key parameter.
	*/
	public function CreateEncryptionKey()
	{
		if (function_exists('random_bytes'))
		{
			return base64_encode(random_bytes(32));
		}
		if (function_exists('openssl_random_pseudo_bytes'))
		{
			return base64_encode(openssl_random_pseudo_bytes(32));
		}
		throw new GrabzItException("Your installation of PHP does not support a method for making a cryptographically secure encryption key. Please upgrade to at least PHP 5.3", GrabzItException::GENERIC_ERROR);		
	}
	
	/*
	This method will decrypt a encrypted capture file, using the key you passed to the encryption key parameter.
	
	path - the path of the encrypted capture
	key - the encryption key
	*/
	public function DecryptFile($path, $key)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		$data = file_get_contents($path);
		file_put_contents($path, $this->Decrypt($data, $key));
		return true;
	}

	/*
	This method will decrypt a encrypted capture, using the key you passed to the encryption key parameter.
	
	data - the encrypted bytes
	key - the encryption key
	*/	
	public function Decrypt($data, $key)
	{
		if ($data == null)
		{
			return null;
		}
		$iv = substr($data, 0, 16);
		$payload = substr($data, 16);
		return openssl_decrypt($payload, 'AES-256-CBC', base64_decode($key), OPENSSL_RAW_DATA|OPENSSL_ZERO_PADDING, $iv);
	}

	/*
	This method specifies the URL of the online video that should be converted into a animated GIF.

	url - The URL to convert into a animated GIF.
	options - A instance of the GrabzItAnimationOptions class that defines any special options to use when creating the animated GIF.
	*/
	public function URLToAnimation($url, GrabzItAnimationOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItAnimationOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . "takeanimation.ashx", false, $options, $url);
	}

	/*
	This method specifies the URL that should be converted into a image screenshot.

	url - The URL to capture as a screenshot.
	options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the screenshot.
	*/
	public function URLToImage($url, GrabzItImageOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItImageOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakePicture, false, $options, $url);
	}

	/*
	This method specifies the HTML that should be converted into a image.

	html - The HTML to convert into a image.
	options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the image.
	*/	
	public function HTMLToImage($html, GrabzItImageOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItImageOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakePicture, true, $options, $html);
	}

	/*
	This method specifies a HTML file that should be converted into a image.

	path - The file path of a HTML file to convert into a image.
	options - A instance of the GrabzItImageOptions class that defines any special options to use when creating the image.
	*/		
	public function FileToImage($path, GrabzItImageOptions $options = null)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		
		$this->HTMLToImage(file_get_contents($path), $options);
	}	

	/*
	This method specifies the URL that should be converted into rendered HTML.

	url - The URL to capture as rendered HTML.
	options - A instance of the GrabzItHTMLOptions class that defines any special options to use when creating the rendered HTML.
	*/
	public function URLToRenderedHTML($url, GrabzItHTMLOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItHTMLOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakeHTML, false, $options, $url);
	}

	/*
	This method specifies the HTML that should be converted into rendered HTML.

	html - The HTML to convert into rendered HTML.
	options - A instance of the GrabzItHTMLOptions class that defines any special options to use when creating the rendered HTML.
	*/	
	public function HTMLToRenderedHTML($html, GrabzItHTMLOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItHTMLOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakeHTML, true, $options, $html);
	}

	/*
	This method specifies a HTML file that should be converted into rendered HTML.

	path - The file path of a HTML file to convert into rendered HTML.
	options - A instance of the GrabzItHTMLOptions class that defines any special options to use when creating the rendered HTML.
	*/		
	public function FileToRenderedHTML($path, GrabzItHTMLOptions $options = null)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		
		$this->HTMLToRenderedHTML(file_get_contents($path), $options);
	}

	/*
	This method specifies the URL that the HTML tables should be extracted from.

	url - The URL to extract HTML tables from.
	options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table.	
	*/
	public function URLToTable($url, GrabzItTableOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItTableOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakeTable, false, $options, $url);
	}
	
	/*
	This method specifies the HTML that the HTML tables should be extracted from.

	html - The HTML to extract HTML tables from.
	options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table.	
	*/
	public function HTMLToTable($html, GrabzItTableOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItTableOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakeTable, true, $options, $html);
	}	
	
	/*
	This method specifies a HTML file that the HTML tables should be extracted from.

	path - The file path of a HTML file to extract HTML tables from.
	options - A instance of the GrabzItTableOptions class that defines any special options to use when converting the HTML table.
	*/	
	public function FileToTable($path, GrabzItTableOptions $options = null)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		
		$this->HTMLToTable(file_get_contents($path), $options);
	}	

	/*
	This method specifies the URL that should be converted into a PDF.

	url - The URL to capture as a PDF.
	options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF.
	*/
	public function URLToPDF($url, GrabzItPDFOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItPDFOptions();			
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakePDF, false, $options, $url);
	}

	/*
	This method specifies the HTML that should be converted into a PDF.

	html - The HTML to convert into a PDF.
	options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF.
	*/	
	public function HTMLToPDF($html, GrabzItPDFOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItPDFOptions();			
		}

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakePDF, true, $options, $html);
	}

	/*
	This method specifies a HTML file that should be converted into a PDF.

	path - The file path of a HTML file to convert into a PDF.
	options - A instance of the GrabzItPDFOptions class that defines any special options to use when creating the PDF.
	*/	
	public function FileToPDF($path, GrabzItPDFOptions $options = null)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		
		$this->HTMLToPDF(file_get_contents($path), $options);
	}	

	/*
	This method specifies the URL that should be converted into a DOCX.

	url - The URL to capture as a DOCX.
	options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX.
	*/
	public function URLToDOCX($url, GrabzItDOCXOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItDOCXOptions();
		}		

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakeDOCX, false, $options, $url);
	}

	/*
	This method specifies the HTML that should be converted into a DOCX.

	html - The HTML to convert into a DOCX.
	options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX.
	*/	
	public function HTMLToDOCX($html, GrabzItDOCXOptions $options = null)
	{
		if ($options == null)
		{
			$options = new GrabzItDOCXOptions();
		}

		$this->request = new GrabzItRequest($this->getRootUrl() . GrabzItClient::TakeDOCX, true, $options, $html);
	}

	/*
	This method specifies a HTML file that should be converted into a DOCX.

	path - The file path of a HTML file to convert into a DOCX.
	options - A instance of the GrabzItDOCXOptions class that defines any special options to use when creating the DOCX.
	*/	
	public function FileToDOCX($path, GrabzItDOCXOptions $options = null)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		
		$this->HTMLToDOCX(file_get_contents($path), $options);
	}	
	
	/*
	This function attempts to Save the result asynchronously and returns a unique identifier, which can be used to get the screenshot with the GetResult method.

	This is the recommended method of saving a file.
	*/
	public function Save($callBackURL = null)
	{
		if ($this->request == null)
		{
			throw new GrabzItException("No parameters have been set.", GrabzItException::PARAMETER_MISSING_PARAMETERS);
		}

		$sig = $this->encode($this->request->getOptions()->_getSignatureString($this->applicationSecret, $callBackURL, $this->request->getTargetUrl()));
		
		$obj = $this->_take($sig, $callBackURL);
		
		if ($obj == null)
		{
			$obj = $this->_take($sig, $callBackURL);
		}
		
		if ($obj == null)
		{
			throw new GrabzItException("An unknown network error occurred, please try calling this method again.", GrabzItException::NETWORK_GENERAL_ERROR);
		}

		if ($obj->ID != null)
		{
			return $obj->ID->__toString();
		}
		
		return $obj->ID;
	}
	
	private function _take($sig, $callBackURL)
	{
		if (!$this->request->isPost())
		{
			return $this->getResultObject($this->Get($this->request->getUrl().'?'.http_build_query($this->request->getOptions()->_getParameters($this->applicationKey, $sig, $callBackURL, 'url', $this->request->getData()), '', '&')));
		}
		return $this->getResultObject($this->Post($this->request->getUrl(), $this->request->getOptions()->_getParameters($this->applicationKey, $sig, $callBackURL, 'html', urlencode($this->request->getData()))));
	}

	/*
	Calls the GrabzIt web service to take the screenshot and saves it to the target path provided. if no target path is provided
	it returns the screenshot byte data.

	WARNING this method is synchronous so will cause a application to pause while the result is processed.

	This function returns the true if it is successful saved to a file, or if it is not saving to a file byte data is returned,
	otherwise the method throws an exception.
	*/
	public function SaveTo($saveToFile = '')
	{
		$id = $this->Save();

		if (empty($id))
		{
			return false;
		}

		//Wait for screenshot to be possibly ready
		usleep((3000 + $this->request->getOptions()->_getStartDelay()) * 1000);

		//Wait for it to be ready.
		while(true)
		{
			$status = $this->GetStatus($id);

			if (!$status->Cached && !$status->Processing)
			{
				throw new GrabzItException("The capture did not complete with the error: " . $status->Message, GrabzItException::RENDERING_ERROR);
				break;
			}
			else if ($status->Cached)
			{
				$result = $this->GetResult($id);
				if (!$result)
				{
					throw new GrabzItException("The capture could not be found on GrabzIt.", GrabzItException::RENDERING_MISSING_SCREENSHOT);
					break;
				}

				if (empty($saveToFile))
				{
					return $result;
				}

				file_put_contents($saveToFile, $result);
				break;
			}

			sleep(3);
		}

		return true;
	}

	/*
	Get the current status of a GrabzIt screenshot

	id - The id of the screenshot

	This function returns a Status object representing the screenshot
	*/
	public function GetStatus($id)
	{
		if (empty($id))
		{
			return null;
		}

		$result = $this->Get($this->getRootUrl() . "getstatus.ashx?id=" . $id);

		$obj = simplexml_load_string($result);

		$status = new GrabzItStatus();
		$status->Processing = ((string)$obj->Processing == GrabzItClient::TrueString);
		$status->Cached = ((string)$obj->Cached == GrabzItClient::TrueString);
		$status->Expired = ((string)$obj->Expired == GrabzItClient::TrueString);
		$status->Message = (string)$obj->Message;

		return $status;
	}

	/*
	This method returns the screenshot itself. If nothing is returned then something has gone wrong or the screenshot is not ready yet.

	id - The unique identifier of the screenshot, returned by the callback handler or the Save method

	This function returns the screenshot
	*/
	public function GetResult($id)
	{
		if (empty($id))
		{
			return null;
		}

		$result = $this->Get($this->getRootUrl() . "getfile.ashx?id=" . $id);

		if (empty($result))
		{
			return null;
		}

		return $result;
	}

	/*
	Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.

	domain - The domain to return cookies for.

	This function returns an array of cookies
	*/
	public function GetCookies($domain)
	{
		$sig = $this->encode($this->applicationSecret."|".$domain);

		$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&sig=".$sig;

		$obj = $this->getResultObject($this->Get($this->getRootUrl() . "getcookies.ashx?" . $qs));

		$result = array();

		foreach ($obj->Cookies->Cookie as $cookie)
		{
			$grabzItCookie = new GrabzItCookie();
			$grabzItCookie->Name = (string)$cookie->Name;
			$grabzItCookie->Value = (string)$cookie->Value;
			$grabzItCookie->Domain = (string)$cookie->Domain;
			$grabzItCookie->Path = (string)$cookie->Path;
			$grabzItCookie->HttpOnly = ((string)$cookie->HttpOnly == GrabzItClient::TrueString);
			$grabzItCookie->Expires = (string)$cookie->Expires;
			$grabzItCookie->Type = (string)$cookie->Type;

			$result[] = $grabzItCookie;
		}

		return $result;
	}

	/*
	Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global
	cookie is overridden.

	This can be useful if a websites functionality is controlled by cookies.

	name - The name of the cookie to set.
	domain - The domain of the website to set the cookie for.
	value - The value of the cookie.
	path - The website path the cookie relates to.
	httponly - Is the cookie only used on HTTP
	expires - When the cookie expires. Pass a null value if it does not expire.

	This function returns true if the cookie was successfully set.
	*/
	public function SetCookie($name, $domain, $value = "", $path = "/", $httponly = false, $expires = "")
	{
		$sig = $this->encode($this->applicationSecret."|".$name."|".$domain."|".$value."|".$path."|".((int)$httponly)."|".$expires."|0");

		$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&name=".urlencode($name)."&value=".urlencode($value)."&path=".urlencode($path)."&httponly=".intval($httponly)."&expires=".urlencode($expires)."&sig=".$sig;

		return $this->isSuccessful($this->Get($this->getRootUrl() . "setcookie.ashx?" . $qs));
	}

	/*
	Delete a custom cookie or block a global cookie from being used.

	name - The name of the cookie to delete
	domain - The website the cookie belongs to

	This function returns true if the cookie was successfully set.
	*/
	public function DeleteCookie($name, $domain)
	{
		$sig =	$this->encode($this->applicationSecret."|".$name."|".$domain."|1");

		$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&name=".urlencode($name)."&delete=1&sig=".$sig;

		return $this->isSuccessful($this->Get($this->getRootUrl() . "setcookie.ashx?" . $qs));
	}

	/*
	Add a new custom watermark.

	identifier - The identifier you want to give the custom watermark. It is important that this identifier is unique.
	path - The absolute path of the watermark on your server. For instance C:/watermark/1.png
	xpos - The horizontal position you want the screenshot to appear at: Left = 0, Center = 1, Right = 2
	ypos - The vertical position you want the screenshot to appear at: Top = 0, Middle = 1, Bottom = 2

	This function returns true if the watermark was successfully set.
	*/
	public function AddWaterMark($identifier, $path, $xpos, $ypos)
	{
		if (!file_exists($path))
		{
			throw new GrabzItException("File: " . $path . " does not exist", GrabzItException::FILE_NON_EXISTANT_PATH);
		}
		$sig = $this->encode($this->applicationSecret."|".$identifier."|".((int)$xpos)."|".((int)$ypos));

		$boundary = '--------------------------'.microtime(true);

		$content = "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"watermark\"; filename=\"".basename($path)."\"\r\n".
				"Content-Type: image/jpeg\r\n\r\n".
				file_get_contents($path)."\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"key\"\r\n\r\n".
				$this->applicationKey . "\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"identifier\"\r\n\r\n".
				$identifier . "\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"xpos\"\r\n\r\n".
				intval($xpos) . "\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"ypos\"\r\n\r\n".
				intval($ypos) . "\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"sig\"\r\n\r\n".
				$sig. "\r\n";

		$content .= "--".$boundary."--\r\n";

		$opts = array('http' =>
			array(
			'method'  => 'POST',
			'header'  => 'Content-Type: multipart/form-data; boundary='.$boundary,
			'content' => $content
			)
		);

		$opts = $this->addProxyToStreamContext($opts);
		
		$context  = stream_context_create($opts);

		$response = @file_get_contents($this->getRootUrl() . 'addwatermark.ashx', false, $context);

		if (isset($http_response_header))
		{
			$this->checkResponseHeader($http_response_header);
		}
		
		if ($response === FALSE)
		{
			throw new GrabzItException("An unknown network error occurred.", GrabzItException::NETWORK_GENERAL_ERROR);
		}		

		return $this->isSuccessful($response);
	}

	/*
	Delete a custom watermark.

	identifier - The identifier of the custom watermark you want to delete

	This function returns true if the watermark was successfully deleted.
	*/
	public function DeleteWaterMark($identifier)
	{
		$sig = $this->encode($this->applicationSecret."|".$identifier);

		$qs = "key=" .urlencode($this->applicationKey)."&identifier=".urlencode($identifier)."&sig=".$sig;

		return $this->isSuccessful($this->Get($this->getRootUrl() . "deletewatermark.ashx?" . $qs));
	}

	/*
	Get a particular custom watermark.

	identifier - The identifier of a particular custom watermark you want to view

	This function returns a GrabzItWaterMark
	*/
	public function GetWaterMark($identifier)
	{
		$watermarks = $this->_getWaterMarks($identifier);

		if (!empty($watermarks) && count($watermarks) == 1)
		{
			return $watermarks[0];
		}

		return null;
	}

	/*
	Get your custom watermarks.

	This function returns an array of GrabzItWaterMark
	*/
	public function GetWaterMarks()
	{
		return $this->_getWaterMarks();
	}

	private function _getWaterMarks($identifier = null)
	{
		$sig = $this->encode($this->applicationSecret."|".$identifier );

		$qs = "key=" .urlencode($this->applicationKey)."&identifier=".urlencode($identifier)."&sig=".$sig;

		$obj = $this->getResultObject($this->Get($this->getRootUrl() . "getwatermarks.ashx?" . $qs));

		$result = array();

		foreach ($obj->WaterMarks->WaterMark as $waterMark)
		{
			$grabzItWaterMark = new GrabzItWaterMark();
			$grabzItWaterMark->Identifier = (string)$waterMark->Identifier;
			$grabzItWaterMark->XPosition = (string)$waterMark->XPosition;
			$grabzItWaterMark->YPosition = (string)$waterMark->YPosition;
			$grabzItWaterMark->Format = (string)$waterMark->Format;

			$result[] = $grabzItWaterMark;
		}

		return $result;
	}

	private function isSuccessful($result)
	{
		$obj = $this->getResultObject($result);
		return ((string)$obj->Result == GrabzItClient::TrueString);
	}

	private function getResultObject($result)
	{
		$obj = simplexml_load_string($result);

		if (!empty($obj->Message))
		{
			throw new GrabzItException($obj->Message, $obj->Code);
		}

		return $obj;
	}

	private function encode($text)
	{
		$sanitized = '';
		if (function_exists('mb_convert_encoding'))
		{
			$sanitized = mb_convert_encoding($text, "ASCII", mb_detect_encoding($text));
		}
		else
		{
			if (!empty($text))
			{
				$chars = preg_split('//u', $text, -1, PREG_SPLIT_NO_EMPTY);
				foreach($chars as $char)
				{
					if(preg_match('/[^\x20-\x7e]/', $char))
					{
						$sanitized .= '?';
						continue;
					}
					$sanitized .= $char;
				}
			}
		}
		return md5($sanitized);
	}

	private function Post($url, $parameters)
	{
		if (ini_get('allow_url_fopen'))
		{
			$options = array(
				'http' => array(
					'timeout' => $this->connectionTimeout,
					'header'  => array("Content-type: application/x-www-form-urlencoded"),
					'method'  => 'POST',
					'content' => http_build_query($parameters, '', '&')
				)
			);

			$options = $this->addProxyToStreamContext($options);
			
			$context  = stream_context_create($options);
			$response = @file_get_contents($url, false, $context);

			if (isset($http_response_header))
			{
				$this->checkResponseHeader($http_response_header);
			}
			
			if ($response === FALSE)
			{
				throw new GrabzItException("An unknown network error occurred.", GrabzItException::NETWORK_GENERAL_ERROR);
			}

			return $response;
		}
		
		if (function_exists('curl_version'))
		{
			$ch = curl_init();

			//set the url, number of POST vars, POST data
			curl_setopt($ch,CURLOPT_URL, $url);
			curl_setopt($ch,CURLOPT_POST, count($parameters));
			curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,$this->connectionTimeout);
			curl_setopt($ch,CURLOPT_POSTFIELDS, http_build_query($parameters, '', '&'));
			curl_setopt($ch,CURLOPT_RETURNTRANSFER,1);
			$this->addProxyToCurl($ch);
			
			//execute post
			$data = curl_exec($ch);

			$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

			$this->checkHttpCode($httpCode);

			//close connection
			curl_close($ch);

			return $data;
		}
		
		throw new GrabzItException("Unable to contact GrabzIt's servers. Please install the CURL extension or set allow_url_fopen to 1 in the php.ini file.", GrabzItException::GENERIC_ERROR);
	}

	private function Get($url)
	{
		if (ini_get('allow_url_fopen'))
		{
			$options = array('http' => array('timeout' => $this->connectionTimeout));
			$options = $this->addProxyToStreamContext($options);
			$context = stream_context_create($options);
			$response = @file_get_contents($url, false, $context);
			
			if (isset($http_response_header))
			{
				$this->checkResponseHeader($http_response_header);
			}
			
			if ($response === FALSE)
			{
				throw new GrabzItException("An unknown network error occurred.", GrabzItException::NETWORK_GENERAL_ERROR);
			}			

			return $response;
		}

		if (function_exists('curl_version'))
		{
			$ch = curl_init();
			curl_setopt($ch,CURLOPT_URL,$url);
			curl_setopt($ch,CURLOPT_RETURNTRANSFER,1);
			curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,$this->connectionTimeout);			
			$this->addProxyToCurl($ch);
			
			$data = curl_exec($ch);
			$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

			$this->checkHttpCode($httpCode);

			curl_close($ch);

			return $data;
		}

		throw new GrabzItException("Unable to contact GrabzIt's servers. Please install the CURL extension or set allow_url_fopen to 1 in the php.ini file.", GrabzItException::GENERIC_ERROR);
	}
	
	private function addProxyToCurl($ch)
	{
		if ($this->proxy != null)
		{
			curl_setopt($ch, CURLOPT_PROXYPORT, $this->proxy->port);
			curl_setopt($ch, CURLOPT_PROXYTYPE, 'HTTP');
			curl_setopt($ch, CURLOPT_PROXY, $this->proxy->host);
			if ($this->proxy->username != null && $this->proxy->password != null)
			{
				curl_setopt($ch, CURLOPT_PROXYUSERPWD, base64_encode($this->proxy->username . ':' . $this->proxy->password));
			}
		}		
	}
	
	private function addProxyToStreamContext($options)
	{
		if ($this->proxy != null)
		{
			$options['http']['request_fulluri'] = true;
			$options['http']['proxy'] = $this->proxy->host . ':' . $this->proxy->port;
			if ($this->proxy->username != null && $this->proxy->password != null)
			{
				$headers = array();
				if (isset($options['http']['header']))
				{
					$headers = $options['http']['header'];
				}
				array_push($headers, 'Proxy-Authorization: Basic' . base64_encode($this->proxy->username . ':' . $this->proxy->password));
				$options['http']['header'] = $headers;
			}
		}		
		
		return $options;
	}

	private function checkHttpCode($httpCode)
	{
		if ($httpCode == 403)
		{
			throw new GrabzItException('Rate limit reached. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.', GrabzItException::NETWORK_DDOS_ATTACK);
		}
		else if ($httpCode >= 400)
		{
			throw new GrabzItException("A network error occurred when connecting to GrabzIt.", GrabzItException::NETWORK_GENERAL_ERROR);
		}
	}
	
	private function getRootUrl()
	{
		return $this->protocol . GrabzItClient::WebServicesBaseURL;
	}

	private function checkResponseHeader($header)
	{
		list($version,$httpCode,$msg) = explode(' ',$header[0], 3);
		$this->checkHttpCode($httpCode);
	}
}
?>
