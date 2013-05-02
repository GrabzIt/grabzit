<?php
include_once("GrabzItCookie.class.php");
include_once("ScreenShotStatus.class.php");

class GrabzItClient
{
	const WebServicesBaseURL = "http://grabz.it/services/";
	const TrueString = "True";

	private $applicationKey;
	private $applicationSecret;

	public function __construct($applicationKey, $applicationSecret)
	{
		$this->applicationKey = $applicationKey;
		$this->applicationSecret = $applicationSecret;
    }

	/*
	This method calls the GrabzIt web service to take the screenshot.

	url - The URL that the screenshot should be made of
	callback - The handler the GrabzIt web service should call after it has completed its work
	browserWidth - The width of the browser in pixels
	browserHeight - The height of the browser in pixels
	outputHeight - The height of the resulting thumbnail in pixels
	outputWidth - The width of the resulting thumbnail in pixels
	customId - A custom identifier that you can pass through to the screenshot webservice. This will be returned with the callback URL you have specified.
	format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
	delay - The number of milliseconds to wait before taking the screenshot
	targetElement - The id of the only HTML element in the web page to turn into a screenshot

	This function returns the unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.
	*/
	public function TakePicture($url, $callback = null, $customId = null, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null, $targetElement = null)
	{
		$qs = "key=" .urlencode($this->applicationKey)."&url=".urlencode($url)."&width=".$width."&height=".$height."&format=".$format."&bwidth=".$browserWidth."&bheight=".$browserHeight."&callback=".urlencode($callback)."&customid=".urlencode($customId)."&delay=".$delay."&target=".urlencode($targetElement);
		$sig =  md5($this->applicationSecret."|".$url."|".$callback."|".$format."|".$height."|".$width."|".$browserHeight."|".$browserWidth."|".$customId."|".$delay."|".$targetElement);
		$qs .= "&sig=".$sig;
		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "takepicture.ashx?" . $qs);
		$obj = simplexml_load_string($result);

		if (!empty($obj->Message))
		{
			throw new Exception($obj->Message);
		}

		return $obj->ID;
	}

	/*
	This method takes the screenshot and then saves the result to a file. WARNING this method is synchronous.

	url - The URL that the screenshot should be made of
	saveToFile - The file path that the screenshot should saved to: e.g. images/test.jpg
	browserWidth - The width of the browser in pixels
	browserHeight - The height of the browser in pixels
	outputHeight - The height of the resulting thumbnail in pixels
	outputWidth - The width of the resulting thumbnail in pixels
	format - The format the screenshot should be in: bmp8, bmp16, bmp24, bmp, gif, jpg, png
	delay - The number of milliseconds to wait before taking the screenshot
	targetElement - The id of the only HTML element in the web page to turn into a screenshot

	This function returns the true if it is successfull otherwise it throws an exception.
	*/
	public function SavePicture($url, $saveToFile, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null, $targetElement = null)
	{
		$id = $this->TakePicture($url, null, null, $browserWidth, $browserHeight, $width, $height, $format, $delay, $targetElement);

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
				$result = $this->GetPicture($id);
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

		$status = new ScreenShotStatus();
		$status->Processing = ((string)$obj->Processing == GrabzItClient::TrueString);
		$status->Cached = ((string)$obj->Cached == GrabzItClient::TrueString);
		$status->Expired = ((string)$obj->Expired == GrabzItClient::TrueString);
		$status->Message = (string)$obj->Message;

		return $status;
	}

	/*
	This method returns the image itself. If nothing is returned then something has gone wrong or the image is not ready yet.

	id - The unique identifier of the screenshot, returned by the callback handler or the TakePicture method

	This function returns the screenshot
	*/
	public function GetPicture($id)
	{
		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "getpicture.ashx?id=" . $id);

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

		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "getcookies.ashx?" . $qs);

		$obj = simplexml_load_string($result);

		if (!empty($obj->Message))
		{
			throw new Exception($obj->Message);
		}

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

		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs);

		$obj = simplexml_load_string($result);

		if (!empty($obj->Message))
		{
			throw new Exception($obj->Message);
		}

		return ((string)$obj->Result == GrabzItClient::TrueString);
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

		$result = $this->Get(GrabzItClient::WebServicesBaseURL . "setcookie.ashx?" . $qs);

		$obj = simplexml_load_string($result);

		if (!empty($obj->Message))
		{
			throw new Exception($obj->Message);
		}

		return ((string)$obj->Result == GrabzItClient::TrueString);
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
