<?php
class GrabzItClient
{
	const WebServicesBaseURL = "http://grabz.it/services/";

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
	format - The format the screenshot should be in: jpg, gif, png
	delay - The number of milliseconds to wait before taking the screenshot

	This function returns the unique identifier of the screenshot. This can be used to get the screenshot with the GetPicture method.
	*/
	public function TakePicture($url, $callback = null, $customId = null, $browserWidth = null, $browserHeight = null, $width = null, $height = null, $format = null, $delay = null)
	{
		$qs = "key=" .$this->applicationKey."&url=".urlencode($url)."&width=".$width."&height=".$height."&format=".$format."&bwidth=".$browserWidth."&bheight=".$browserHeight."&callback=".urlencode($callback)."&customid=".urlencode($customId)."&delay=".$delay;
		$sig =  md5($this->applicationSecret."|".$url."|".$callback."|".$format."|".$height."|".$width."|".$browserHeight."|".$browserWidth."|".$customId."|".$delay);
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
