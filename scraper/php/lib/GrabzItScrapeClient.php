<?php
namespace GrabzIt\Scraper;

spl_autoload_register(function ($class_name) {
	$file_name = str_replace("GrabzIt\\Scraper\\", '', $class_name) . '.php';
	if (strpos($file_name, "GrabzIt") === 0) {
		include($file_name);
	}
});

class GrabzItScrapeClient
{
	const WebServicesBaseURL = "http://api.grabz.it/services/scraper/";
	const TrueString = "True";

	private $applicationKey;
	private $applicationSecret;
	private $connectionTimeout = 600;
	private $proxy = null;

	public function __construct($applicationKey, $applicationSecret)
	{
		$this->applicationKey = $applicationKey;
		$this->applicationSecret = $applicationSecret;
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
	Get all of the requested scrapes

	id - If specified, just returns that scrape that matches the id.

	This function returns a array of Scrape objects
	*/
	public function GetScrapes($id = "")
	{
		$sig =	$this->encode($this->applicationSecret."|".$id);

		$qs = "key=" .urlencode($this->applicationKey)."&identifier=".$id."&sig=".$sig;

		$obj = $this->getResultObject($this->Get(GrabzItScrapeClient::WebServicesBaseURL . "getscrapes.ashx?" . $qs));

		$result = array();

		if ($obj == null)
		{
			return $result;
		}
		
		foreach ($obj->Scrapes->Scrape as $scrape)
		{
			$grabzitScrape = new GrabzItScrape();
			$grabzitScrape->ID = (string)$scrape->Identifier;
			$grabzitScrape->Name = (string)$scrape->Name;
			$grabzitScrape->Status = (string)$scrape->Status;
			$grabzitScrape->NextRun = (string)$scrape->NextRun;
			foreach ($scrape->Results->Result as $scrapeResult)
			{
				$grabzItScrapeHistory = new GrabzItScrapeHistory();
				$grabzItScrapeHistory->ID = (string)$scrapeResult->Identifier;
				$grabzItScrapeHistory->Finished = (string)$scrapeResult->Finished;
				
				$grabzitScrape->Results[] = $grabzItScrapeHistory;
			}
			
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

		$sig =	$this->encode($this->applicationSecret."|".$id."|".$status);

		$qs = "key=" .urlencode($this->applicationKey)."&id=".urlencode($id)."&status=".urlencode($status)."&sig=".$sig;

		return $this->isSuccessful($this->Get(GrabzItScrapeClient::WebServicesBaseURL . "setscrapestatus.ashx?" . $qs));
	}
	
	/*
	Set a property of a scrape

	id - The id of the scrape to set.
	property - The property object that contains the required changes

	This function returns true if successful
	*/
	public function SetScrapeProperty($id, $property)
	{
		if (empty($id) || $property == null)
		{
			return false;
		}
		
		$sig =	$this->encode($this->applicationSecret."|".$id."|".$property->GetTypeName());
		
		$qs = array();
		$qs["key"] = $this->applicationKey;
		$qs["id"] = $id;
		$qs["payload"] = $property->ToXML();
		$qs["type"] = $property->GetTypeName();
		$qs["sig"] = $sig;

		return $this->isSuccessful($this->Post(GrabzItScrapeClient::WebServicesBaseURL . "setscrapeproperty.ashx", $qs));
	}
	
	/*
	Re-sends the scrape result with the matching scrape id and result id using the export parameters stored in the scrape.

	id - The id of the scrape that contains the result to re-send.
	resultId - The id of the result to re-send.

	This function returns true if the result was successfully requested to be re-sent.
	*/	  
	public function SendResult($id, $resultId)
	{
		if (empty($id) || empty($resultId))
		{
			return false;
		}

		$sig =	$this->encode($this->applicationSecret."|".$id."|".$resultId);

		$qs = "key=" .urlencode($this->applicationKey)."&id=".urlencode($id)."&spiderId=".urlencode($resultId)."&sig=".$sig;

		return $this->isSuccessful($this->Get(GrabzItScrapeClient::WebServicesBaseURL . "sendscrape.ashx?" . $qs));
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
			$opts = array('http' => array('timeout' => $this->connectionTimeout));
			$opts = $this->addProxyToStreamContext($opts);
			$context = stream_context_create($opts);
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
			$this->addProxyToCurl($ch);
			
			$data = curl_exec($ch);
			$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

			$this->checkHttpCode($httpCode);

			curl_close($ch);

			return $data;
		}
		throw new GrabzItScrapeException("Unable to contact GrabzIt's servers. Please install the CURL extension or set allow_url_fopen to 1 in the php.ini file.", GrabzItException::GENERIC_ERROR);
	}

	private function Post($url, $parameters)
	{
		if (ini_get('allow_url_fopen'))
		{
			$options = array(
				'http' => array(
					'timeout' => $this->connectionTimeout,
					'header'  => "Content-type: application/x-www-form-urlencoded\r\n",
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
				throw new GrabzItScrapeException("An unknown network error occurred.", GrabzItException::NETWORK_GENERAL_ERROR);
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
			$this->addProxyToCurl($ch);

			//execute post
			$data = curl_exec($ch);

			$httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

			$this->checkHttpCode($httpCode);

			//close connection
			curl_close($ch);

			return $data;
		}
		
		throw new GrabzItScrapeException("Unable to contact GrabzIt's servers. Please install the CURL extension or set allow_url_fopen to 1 in the php.ini file.", GrabzItScrapeException::GENERIC_ERROR);
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