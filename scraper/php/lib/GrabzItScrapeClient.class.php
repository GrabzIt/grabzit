<?php
include_once("GrabzItScrapeException.class.php");
include_once("GrabzItScrape.class.php");

class GrabzItScrapeClient
{
	const WebServicesBaseURL = "http://api.grabz.it/services/scraper/";
	const TrueString = "True";

	private $applicationKey;
	private $applicationSecret;
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

	/*
	Get all of the users scrapes

	id - If specified, just returns that scrape that matches the id.

	This function returns a array of Scrape objects
	*/
	public function GetScrapes($id = "")
	{
		$sig =  $this->encode($this->applicationSecret."|".$id);

		$qs = "key=" .urlencode($this->applicationKey)."&identifier=".$id."&sig=".$sig;

		$obj = $this->getResultObject($this->Get(GrabzItScrapeClient::WebServicesBaseURL . "getscrapes.ashx?" . $qs));

		$result = array();

		foreach ($obj->Scrapes->Scrape as $scrape)
		{
			$grabzitScrape = new GrabzItScrape();
			$grabzitScrape->ID = (string)$scrape->Identifier;
			$grabzitScrape->Name = (string)$scrape->Name;
			$grabzitScrape->Status = (string)$scrape->Status;
			$grabzitScrape->NextRun = (string)$scrape->NextRun;

			$result[] = $grabzitScrape;
		}

		return $result;
	}

	/*
	Sets the status of a scrape.

	id - The id of the scrape to set.
	status - The scrape status to set the scrape to. Options include Start, Stop, Enable and Disable

	This function returns true if the scrape was successfully set.
	*/
	public function SetScrapeStatus($id, $status)
	{
		if (empty($id))
		{
			return false;
		}

		$sig =  $this->encode($this->applicationSecret."|".$id."|".$status);

		$qs = "key=" .urlencode($this->applicationKey)."&id=".urlencode($id)."&status=".urlencode($status)."&sig=".$sig;

		return $this->isSuccessful($this->Get(GrabzItScrapeClient::WebServicesBaseURL . "setscrapestatus.ashx?" . $qs));
	}

	private function isSuccessful($result)
	{
		$obj = $this->getResultObject($result);
		return ((string)$obj->Result == GrabzItScrapeClient::TrueString);
	}

	private function getResultObject($result)
	{
		$obj = simplexml_load_string($result);

		if (!empty($obj->Message))
		{
			throw new GrabzItScrapeException($obj->Message, $obj->Code);
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

		throw new GrabzItScrapeException("Unable to contact GrabzIt's servers. Please install the CURL extension or set allow_url_fopen to 1 in the php.ini file.", GrabzItException::GENERIC_ERROR);
	}

	private function checkHttpCode($httpCode)
	{
	    if ($httpCode == 403)
	    {
			throw new GrabzItScrapeException('Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.', GrabzItException::NETWORK_DDOS_ATTACK);
	    }
	    else if ($httpCode >= 400)
	    {
			throw new GrabzItScrapeException("A network error occured when connecting to the GrabzIt servers.", GrabzItException::NETWORK_GENERAL_ERROR);
	    }
	}

	private function checkResponseHeader($header)
	{
	    list($version,$httpCode,$msg) = explode(' ',$header[0], 3);
		$this->checkHttpCode($httpCode);
	}
}
?>