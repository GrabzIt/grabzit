<?php
include_once("GrabzItCookie.class.php");
include_once("GrabzItStatus.class.php");
include_once("GrabzItWaterMark.class.php");

class GrabzItClient
{
	const WebServicesBaseURL = "http://grabz.it/services/";
	const TrueString = "True";

	private $applicationKey;
	private $applicationSecret;
    private $signaturePartOne;
    private $signaturePartTwo;
    private $request;

	public function __construct($applicationKey, $applicationSecret)
	{
		$this->applicationKey = $applicationKey;
		$this->applicationSecret = $applicationSecret;
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
		This method sets the parameters required to take a screenshot of a web page.

		url - The URL that the screenshot should be made of
		browserWidth - The width of the browser in pixels
		browserHeight - The height of the browser in pixels
		outputHeight - The height of the resulting thumbnail in pixels
		outputWidth - The width of the resulting thumbnail in pixels
		customId - A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.
		format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
		delay - The number of milliseconds to wait before taking the screenshot
		targetElement - The id of the only HTML element in the web page to turn into a screenshot
		requestMobileVersion - Request a mobile version of the target website if possible
		customWaterMarkId - add a custom watermark to the image
		*/
		public function SetImageOptions($url, $customId = null, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null, $targetElement = null, $requestMobileVersion = false, $customWaterMarkId = null)
		{
			$this->request = GrabzItClient::WebServicesBaseURL . "takepicture.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&width=".$width."&height=".$height."&format=".$format."&bwidth=".$browserWidth."&bheight=".$browserHeight."&customid=".urlencode($customId)."&delay=".$delay."&target=".urlencode($targetElement)."&customwatermarkid=".urlencode($customWaterMarkId)."&requestmobileversion=".intval($requestMobileVersion)."&callback=";
			$this->signaturePartOne = $this->applicationSecret."|".$url."|";
			$this->signaturePartTwo = "|".$format."|".$height."|".$width."|".$browserHeight."|".$browserWidth."|".$customId."|".$delay."|".$targetElement."|".$customWaterMarkId."|".intval($requestMobileVersion);
		}

		/*
		This method sets the parameters required to extract all tables from a web page.

		url - The URL that the should be used to extract tables
		format - The format the tableshould be in: csv, xlsx
		customId - A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.
		includeHeaderNames - If true header names will be included in the table
		includeAllTables - If true all table on the web page will be extracted with each table appearing in a seperate spreadsheet sheet. Only available with the XLSX format.
		targetElement - The id of the only HTML element in the web page that should be used to extract tables from
		requestMobileVersion - Request a mobile version of the target website if possible
		*/
		public function SetTableOptions($url, $customId = null, $tableNumberToInclude = 1, $format = 'csv', $includeHeaderNames = true, $includeAllTables = false, $targetElement = null, $requestMobileVersion = false)
		{
			$this->request = GrabzItClient::WebServicesBaseURL . "taketable.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&includeAllTables=".intval($includeAllTables)."&includeHeaderNames=".intval($includeHeaderNames) ."&format=".$format."&tableToInclude=".$tableNumberToInclude."&customid=".urlencode($customId)."&target=".urlencode($targetElement)."&requestmobileversion=".intval($requestMobileVersion)."&callback=";
			$this->signaturePartOne = $this->applicationSecret."|".$url."|";
			$this->signaturePartTwo = "|".$customId."|".$tableNumberToInclude ."|".intval($includeAllTables)."|".intval($includeHeaderNames)."|".$targetElement."|".$format."|".intval($requestMobileVersion);
		}

		/*
		This method sets the parameters required to convert a web page into a PDF.

		url - The URL that the should be converted into a pdf
		customId - A custom identifier that you can pass through to the webservice. This will be returned with the callback URL you have specified.
		includeBackground - If true the background of the web page should be included in the screenshot
		pagesize - The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'B3', 'B4', 'B5'.
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
		requestMobileVersion - Request a mobile version of the target website if possible
		customWaterMarkId - add a custom watermark to each page of the PDF document
		*/
		public function SetPDFOptions($url, $customId = null, $includeBackground = true, $pagesize = 'A4', $orientation = 'Portrait', $includeLinks = true, $includeOutline = false, $title = null, $coverURL = null, $marginTop = 10, $marginLeft = 10, $marginBottom = 10, $marginRight = 10, $delay = null, $requestMobileVersion = false, $customWaterMarkId = null)
		{
			$pagesize = strtoupper($pagesize);
			$orientation = ucfirst($orientation);

			$this->request = GrabzItClient::WebServicesBaseURL . "takepdf.ashx?key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&background=".intval($includeBackground) ."&pagesize=".$pagesize."&orientation=".$orientation."&customid=".urlencode($customId)."&customwatermarkid=".urlencode($customWaterMarkId)."&includelinks=".intval($includeLinks)."&includeoutline=".intval($includeOutline)."&title=".urlencode($title)."&coverurl=".urlencode($coverURL)."&mleft=".$marginLeft."&mright=".$marginRight."&mtop=".$marginTop."&mbottom=".$marginBottom."&delay=".$delay."&requestmobileversion=".intval($requestMobileVersion)."&callback=";

			$this->signaturePartOne = $this->applicationSecret."|".$url."|";
			$this->signaturePartTwo = "|".$customId ."|".intval($includeBackground) ."|".$pagesize ."|".$orientation."|".$customWaterMarkId."|".intval($includeLinks)."|".intval($includeOutline)."|".$title."|".$coverURL."|".$marginTop."|".$marginLeft."|".$marginBottom."|".$marginRight."|".$delay."|".intval($requestMobileVersion);
		}

		/*
		This function attempts to Save the result asynchronously and returns a unique identifier, which can be used to get the screenshot with the GetResult method.

		This is the recommended method of saving a file.
		*/
		public function Save($callBackURL = null)
		{
			if (empty($this->signaturePartOne) && empty($this->signaturePartTwo) && empty($this->request))
			{
				  throw new Exception("No screenshot parameters have been set.");
			}
			$sig =  md5($this->signaturePartOne.$callBackURL.$this->signaturePartTwo);
			$this->request .= urlencode($callBackURL)."&sig=".$sig;
			$obj = $this->getResultObject($this->Get($this->request));

			return $obj->ID;
		}

		/*
		This function attempts to Save the result synchronously to a file.

		WARNING this method is synchronous so will cause a application to pause while the result is processed.

		This function returns the true if it is successful otherwise it throws an exception.
		*/
		public function SaveTo($saveToFile)
		{
			$id = $this->Save();

			//Wait for it to be ready.
			while(true)
			{
				$status = $this->GetStatus($id);

				if (!$status->Cached && !$status->Processing)
				{
					throw new Exception("The screenshot did not complete with the error: " . $status->Message);
					break;
				}
				else if ($status->Cached)
				{
					$result = $this->GetResult($id);
					if (!$result)
					{
						throw new Exception("The screenshot image could not be found on GrabzIt.");
						break;
					}
					file_put_contents($saveToFile, $result);
					break;
				}

				sleep(1);
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
			$sig =  md5($this->applicationSecret."|".$domain);

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
			$sig =  md5($this->applicationSecret."|".$name."|".$domain."|".$value."|".$path."|".((int)$httponly)."|".$expires."|0");

			$qs = "key=" .urlencode($this->applicationKey)."&domain=".urlencode($domain)."&name=".urlencode($name)."&value=".urlencode($value)."&path=".urlencode($path)."&httponly=".intval($httponly)."&expires=".$expires."&sig=".$sig;

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
			$sig =  md5($this->applicationSecret."|".$name."|".$domain."|1");

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
					throw new Exception("File: " . $path . " does not exist");
		        }
                $sig =  md5($this->applicationSecret."|".$identifier."|".((int)$xpos)."|".((int)$ypos));

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

		return $this->isSuccessful(file_get_contents(GrabzItClient::WebServicesBaseURL . 'addwatermark.ashx', false, $context));
        }

		/*
		Delete a custom watermark.

		identifier - The identifier of the custom watermark you want to delete

		This function returns true if the watermark was successfully deleted.
		*/
		public function DeleteWaterMark($identifier)
		{
			$sig = md5($this->applicationSecret."|".$identifier);

			$qs = "key=" .urlencode($this->applicationKey)."&identifier=".urlencode($identifier)."&sig=".$sig;

			return $this->isSuccessful($this->Get(GrabzItClient::WebServicesBaseURL . "deletewatermark.ashx?" . $qs));
		}

		/*
		Get your custom watermarks.

		identifier - The identifier of a particular custom watermark you want to view

		This function returns an array of GrabzItWaterMark
		*/
		public function GetWaterMarks($identifier = null)
		{
			$sig =  md5($this->applicationSecret."|".$identifier );

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

				$result[] = $grabzItWaterMark ;
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
			throw new Exception($obj->Message);
		}

                return $obj;
        }

		private function Get($url)
		{
			if (ini_get('allow_url_fopen'))
			{
				return file_get_contents($url);
			}
			else
			{
				$ch = curl_init();
				$timeout = 5;
				curl_setopt($ch,CURLOPT_URL,$url);
				curl_setopt($ch,CURLOPT_RETURNTRANSFER,1);
				curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,$timeout);
				$data = curl_exec($ch);
				curl_close($ch);
				return $data;
			}
		}
}
?>