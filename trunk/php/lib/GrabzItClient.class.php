<?php
include_once("GrabzItCookie.class.php");
include_once("GrabzItStatus.class.php");
include_once("GrabzItWaterMark.class.php");
include_once("GrabzItException.class.php");

class GrabzItClient
{
	const WebServicesBaseURL = "http://api.grabz.it/services/";
	const TrueString = "True";

	private $applicationKey;
	private $applicationSecret;
	private $signaturePartOne;
	private $signaturePartTwo;
	private $request;
	private $startDelay;
	private $connectionTimeout = 600;

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

	#
	# This method sets the parameters required to turn a online video into a animated GIF
	#
	# url - The URL of the online video
	# customId - A custom identifier that you can pass through to the animated GIF web service. This will be returned with the callback URL you have specified
	# width - The width of the resulting animated GIF in pixels
	# height - The height of the resulting animated GIF in pixels
	# start - The starting position of the video that should be converted into a animated GIF
	# duration - The length in seconds of the video that should be converted into a animated GIF
	# speed - The speed of the animated GIF from 0.2 to 10 times the original speed
	# framesPerSecond - The number of frames per second that should be captured from the video. From a minimum of 0.2 to a maximum of 60
	# repeat - The number of times to loop the animated GIF. If 0 it will loop forever
	# reverse - If true the frames of the animated GIF are reversed
	# customWaterMarkId - Add a custom watermark to the animated GIF
	# quality - The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
	# country - Request the screenshot from different countries: Default, UK or US
	#
	public function SetAnimationOptions($url, $customId = null, $width = null, $height = null, $start = null, $duration = null, $speed = null, $framesPerSecond = null, $repeat = null, $reverse = false, $customWaterMarkId = null, $quality = -1, $country = null)
	{
		$this->startDelay = 0;
		$this->request = GrabzItClient::WebServicesBaseURL . "takeanimation.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&width=".$width."&height=".$height."&duration=".$duration."&speed=".$speed."&start=".$start."&customid=".urlencode($customId)."&fps=".$framesPerSecond."&repeat=".$repeat."&customwatermarkid=".urlencode($customWaterMarkId)."&reverse=".intval($reverse)."&country=".urlencode($country)."&quality=".$quality."&callback=";
		$this->signaturePartOne = $this->applicationSecret."|".$url."|";
		$this->signaturePartTwo = "|".$height."|".$width."|".$customId."|".$framesPerSecond."|".$speed."|".$duration."|".$repeat."|".intval($reverse)."|".$start."|".$customWaterMarkId."|".$country."|".$quality;
	}

	/*
	This method sets the parameters required to take a screenshot of a web page.

	url - The URL that the screenshot should be made of
	browserWidth - The width of the browser in pixels
	browserHeight - The height of the browser in pixels
	outputHeight - The height of the resulting thumbnail in pixels
	outputWidth - The width of the resulting thumbnail in pixels
	customId - A custom identifier that you can pass through to the screenshot web service. This will be returned with the callback URL you have specified.
	format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
	delay - The number of milliseconds to wait before taking the screenshot
	targetElement - The id of the only HTML element in the web page to turn into a screenshot
	requestAs - Request screenshot in different forms: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
	customWaterMarkId - add a custom watermark to the image
	quality - The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality for the specified image format
	country - Request the screenshot from different countries: Default = "", UK = "UK", US = "US"
	*/
	public function SetImageOptions($url, $customId = null, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null, $targetElement = null, $requestAs = 0, $customWaterMarkId = null, $quality = -1, $country = null)
	{
		if ($delay == null)
		{
			$this->startDelay = 0;
		}
		else
		{
			$this->startDelay = $delay;
		}
		$this->request = GrabzItClient::WebServicesBaseURL . "takepicture.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&width=".$width."&height=".$height."&format=".$format."&bwidth=".$browserWidth."&bheight=".$browserHeight."&customid=".urlencode($customId)."&delay=".$delay."&target=".urlencode($targetElement)."&customwatermarkid=".urlencode($customWaterMarkId)."&requestmobileversion=".intval($requestAs)."&country=".urlencode($country)."&quality=".$quality."&callback=";
		$this->signaturePartOne = $this->applicationSecret."|".$url."|";
		$this->signaturePartTwo = "|".$format."|".$height."|".$width."|".$browserHeight."|".$browserWidth."|".$customId."|".$delay."|".$targetElement."|".$customWaterMarkId."|".intval($requestAs)."|".$country."|".$quality;
	}

	/*
	This method sets the parameters required to extract all tables from a web page.

	url - The URL that the should be used to extract tables
	format - The format the tableshould be in: csv, xlsx
	customId - A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified.
	includeHeaderNames - If true header names will be included in the table
	includeAllTables - If true all table on the web page will be extracted with each table appearing in a separate spreadsheet sheet. Only available with the XLSX format.
	targetElement - The id of the only HTML element in the web page that should be used to extract tables from
	requestAs - Request screenshot in different forms: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
	country - Request the screenshot from different countries: Default = "", UK = "UK", US = "US"
	*/
	public function SetTableOptions($url, $customId = null, $tableNumberToInclude = 1, $format = 'csv', $includeHeaderNames = true, $includeAllTables = false, $targetElement = null, $requestAs = 0, $country = null)
	{
		$this->startDelay = 0;
		$this->request = GrabzItClient::WebServicesBaseURL . "taketable.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&includeAllTables=".intval($includeAllTables)."&includeHeaderNames=".intval($includeHeaderNames) ."&format=".$format."&tableToInclude=".$tableNumberToInclude."&customid=".urlencode($customId)."&target=".urlencode($targetElement)."&requestmobileversion=".intval($requestAs)."&country=".urlencode($country)."&callback=";
		$this->signaturePartOne = $this->applicationSecret."|".$url."|";
		$this->signaturePartTwo = "|".$customId."|".$tableNumberToInclude ."|".intval($includeAllTables)."|".intval($includeHeaderNames)."|".$targetElement."|".$format."|".intval($requestAs)."|".$country;
	}

	/*
	This method sets the parameters required to convert a web page into a PDF.

	url - The URL that the should be converted into a pdf
	customId - A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified.
	includeBackground - If true the background of the web page should be included in the screenshot
	pagesize - The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5', 'Letter'.
	orientation - The orientation of the PDF to be returned: 'Landscape' or 'Portrait'
	includeLinks - True if links should be included in the PDF
	includeOutline - True if the PDF outline should be included
	title - Provide a title to the PDF document
	coverURL - The URL of a web page that should be used as a cover page for the PDF
	marginTop - The margin that should appear at the top of the PDF document page
	marginLeft - The margin that should appear at the left of the PDF document page
	marginBottom - The margin that should appear at the bottom of the PDF document page
	marginRight - The margin that should appear at the right of the PDF document
	delay - The number of milliseconds to wait before taking the screenshot
	requestAs - Request screenshot in different forms: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
	customWaterMarkId - add a custom watermark to the image
	quality - The quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
	country - Request the screenshot from different countries: Default = "", UK = "UK", US = "US"
	*/
	public function SetPDFOptions($url, $customId = null, $includeBackground = true, $pagesize = 'A4', $orientation = 'Portrait', $includeLinks = true, $includeOutline = false, $title = null, $coverURL = null, $marginTop = 10, $marginLeft = 10, $marginBottom = 10, $marginRight = 10, $delay = null, $requestAs = 0, $customWaterMarkId = null, $quality = -1, $country = null)
	{
		if ($delay == null)
		{
			$this->startDelay = 0;
		}
		else
		{
			$this->startDelay = $delay;
		}

		$pagesize = strtoupper($pagesize);
		$orientation = ucfirst($orientation);

		$this->request = GrabzItClient::WebServicesBaseURL . "takepdf.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&background=".intval($includeBackground) ."&pagesize=".$pagesize."&orientation=".$orientation."&customid=".urlencode($customId)."&customwatermarkid=".urlencode($customWaterMarkId)."&includelinks=".intval($includeLinks)."&includeoutline=".intval($includeOutline)."&title=".urlencode($title)."&coverurl=".urlencode($coverURL)."&mleft=".$marginLeft."&mright=".$marginRight."&mtop=".$marginTop."&mbottom=".$marginBottom."&delay=".$delay."&requestmobileversion=".intval($requestAs)."&country=".urlencode($country)."&quality=".$quality."&callback=";

		$this->signaturePartOne = $this->applicationSecret."|".$url."|";
		$this->signaturePartTwo = "|".$customId ."|".intval($includeBackground) ."|".$pagesize ."|".$orientation."|".$customWaterMarkId."|".intval($includeLinks)."|".intval($includeOutline)."|".$title."|".$coverURL."|".$marginTop."|".$marginLeft."|".$marginBottom."|".$marginRight."|".$delay."|".intval($requestAs)."|".$country."|".$quality;
	}

	/*
	This function attempts to Save the result asynchronously and returns a unique identifier, which can be used to get the screenshot with the GetResult method.

	This is the recommended method of saving a file.
	*/
	public function Save($callBackURL = null)
	{
		if (empty($this->signaturePartOne) && empty($this->signaturePartTwo) && empty($this->request))
		{
			throw new GrabzItException("No screenshot parameters have been set.", GrabzItException::PARAMETER_MISSING_PARAMETERS);
		}

		$sig =  $this->encode($this->signaturePartOne.$callBackURL.$this->signaturePartTwo);
		$currentRequest = $this->request;

		$currentRequest .= urlencode($callBackURL)."&sig=".$sig;
		$obj = $this->getResultObject($this->Get($currentRequest));

		return $obj->ID;
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
		usleep((3000 + $this->startDelay) * 1000);

		//Wait for it to be ready.
		while(true)
		{
			$status = $this->GetStatus($id);

			if (!$status->Cached && !$status->Processing)
			{
				throw new GrabzItException("The screenshot did not complete with the error: " . $status->Message, GrabzItException::RENDERING_ERROR);
				break;
			}
			else if ($status->Cached)
			{
				$result = $this->GetResult($id);
				if (!$result)
				{
					throw new GrabzItException("The screenshot could not be found on GrabzIt.", GrabzItException::RENDERING_MISSING_SCREENSHOT);
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

		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "getstatus.ashx?id=" . $id);

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

		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "getfile.ashx?id=" . $id);

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
		$sig =  $this->encode($this->applicationSecret."|".$domain);

		$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&sig=".$sig;

		$obj = $this->getResultObject($this->Get(GrabzItClient::WebServicesBaseURL . "getcookies.ashx?" . $qs));

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
		$sig =  $this->encode($this->applicationSecret."|".$name."|".$domain."|".$value."|".$path."|".((int)$httponly)."|".$expires."|0");

		$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&name=".urlencode($name)."&value=".urlencode($value)."&path=".urlencode($path)."&httponly=".intval($httponly)."&expires=".urlencode($expires)."&sig=".$sig;

		return $this->isSuccessful($this->Get(GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs));
	}

	/*
	Delete a custom cookie or block a global cookie from being used.

	name - The name of the cookie to delete
	domain - The website the cookie belongs to

	This function returns true if the cookie was successfully set.
	*/
	public function DeleteCookie($name, $domain)
	{
		$sig =  $this->encode($this->applicationSecret."|".$name."|".$domain."|1");

		$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&name=".urlencode($name)."&delete=1&sig=".$sig;

		return $this->isSuccessful($this->Get(GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs));
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
		$sig =  $this->encode($this->applicationSecret."|".$identifier."|".((int)$xpos)."|".((int)$ypos));

		$boundary = '--------------------------'.microtime(true);

		$content =  "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"watermark\"; filename=\"".basename($path)."\"\r\n".
				"Content-Type: image/jpeg\r\n\r\n".
				file_get_contents($path)."\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"key\"\r\n\r\n".
				urlencode($this->applicationKey) . "\r\n";

		$content .= "--".$boundary."\r\n".
				"Content-Disposition: form-data; name=\"identifier\"\r\n\r\n".
				urlencode($identifier) . "\r\n";

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

		$context  = stream_context_create($opts);

		$response = @file_get_contents('http://grabz.it/services/addwatermark.ashx', false, $context);

		if (isset($http_response_header))
		{
			$this->checkResponseHeader($http_response_header);
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

		return $this->isSuccessful($this->Get(GrabzItClient::WebServicesBaseURL . "deletewatermark.ashx?" . $qs));
	}

	/*
	Get a particular custom watermark.

    identifier - The identifier of a particular custom watermark you want to view

    This function returns a GrabzItWaterMark
    */
	public function GetWaterMark($identifier)
	{
		$watermarks[] = _getWaterMarks($identifier);

		if ($watermarks != null && count($watermarks) == 1)
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
		return _getWaterMarks();
	}

	private function _getWaterMarks($identifier = null)
	{
		$sig =  $this->encode($this->applicationSecret."|".$identifier );

		$qs = "key=" .urlencode($this->applicationKey)."&identifier=".urlencode($identifier)."&sig=".$sig;

		$obj = $this->getResultObject($this->Get(GrabzItClient::WebServicesBaseURL . "getwatermarks.ashx?" . $qs));

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

	/*
	DEPRECATED - Use GetResult method instead
	*/
	public function GetPicture($id)
	{
		return $this->GetResult($id);
	}

	/*
	DEPRECATED - Use the SetImageOptions and SaveTo methods instead
	*/
	public function SavePicture($url, $saveToFile, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null, $targetElement = null)
	{
		$this->SetImageOptions($url, '', $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement);
		return $this->SaveTo($saveToFile);
	}

	/*
	DEPRECATED - Use SetImageOptions and Save method instead
	*/
	public function TakePicture($url, $callback = null, $customId = null, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null, $targetElement = null)
	{
		$this->SetImageOptions($url, $customId, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement);
		return $this->Save($callback);
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
		return md5(mb_convert_encoding($text, "ASCII", mb_detect_encoding($text)));
	}

	private function Get($url)
	{
		if (ini_get('allow_url_fopen'))
		{
			$timeout = array('http' => array('timeout' => $this->connectionTimeout));
			$context = stream_context_create($timeout);
			$response = @file_get_contents($url, false, $context);

			if (isset($http_response_header))
			{
				$this->checkResponseHeader($http_response_header);
			}

			return $response;
		}

		if (function_exists('curl_version'))
		{
			$ch = curl_init();
			curl_setopt($ch,CURLOPT_URL,$url);
			curl_setopt($ch,CURLOPT_RETURNTRANSFER,1);
			curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,$this->connectionTimeout);
			$data = curl_exec($ch);
			$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

			$this->checkHttpCode($httpCode);

			curl_close($ch);

			return $data;
		}

		throw new GrabzItException("Unable to contact GrabzIt's servers. Please install the CURL extension or set allow_url_fopen to 1 in the php.ini file.", GrabzItException::GENERIC_ERROR);
	}

	private function checkHttpCode($httpCode)
	{
	    if ($httpCode == 403)
	    {
			throw new GrabzItException('Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.', GrabzItException::NETWORK_DDOS_ATTACK);
	    }
	    else if ($httpCode >= 400)
	    {
			throw new GrabzItException("A network error occured when connecting to the GrabzIt servers.", GrabzItException::NETWORK_GENERAL_ERROR);
	    }
	}

	private function checkResponseHeader($header)
	{
	    list($version,$httpCode,$msg) = explode(' ',$header[0], 3);
		$this->checkHttpCode($httpCode);
	}
}
?>