<?php
namespace GrabzIt;

class GrabzItBaseOptions
{
	private $customId = null;
	private $country = null;
	private $exportUrl = null;
	private $encryptionKey = null;
	private $proxy = null;
	protected $delay = null;
	protected $post = null;
	
	/*
	A custom identifier that you can pass through to the web service. This will be returned with the callback URL you have specified.
	*/
	public function setCustomId($value)
	{
		$this->customId = $value;
	}

	/*
	Get the custom identifier that you are passing through to the web service.
	*/
	public function getCustomId()
	{
		return $this->customId;
	}	
	
	/*
	Set the country the capture should be created from: Default = "", Singapore = "SG", UK = "UK", US = "US".
	*/
	public function setCountry($value)
	{
		$this->country = $value;
	}

	/*
	Get the country the capture should be created from.
	*/
	public function getCountry()
	{
		return $this->country;
	}
	
	/*
	Set the export URL that should be used to transfer the capture to a third party location.
	*/
	public function setExportURL($value)
	{
		$this->exportUrl = $value;
	}

	/*
	Get the export URL that should be used to transfer the capture.
	*/
	public function getExportURL()
	{
		return $this->exportUrl;
	}
	
	/*
	Set the encryption key that will be used to encrypt your capture.
	*/
	public function setEncryptionKey($value)
	{
		$this->encryptionKey = $value;
	}
	
	/*
	Get the encryption key that will be used to encrypt your capture.
	*/
	public function getEncryptionKey()
	{
		return $this->encryptionKey;
	}
	
	/*
	Set the HTTP proxy that should be used to create the capture.
	*/
	public function setProxy($value)
	{
		$this->proxy = $value;
	}

	/*
	Get the HTTP proxy that should be used to create the capture.
	*/
	public function getProxy()
	{
		return $this->proxy;
	}	
	
	protected function appendParameter($qs, $name, $value)
	{
		$val = '';
		if (!empty($name))
		{
			$val = urlencode($name);
			$val .= "=";
			if (!empty($value))
			{
				$val .= urlencode($value);
			}
		}
		if (empty($val))
		{
			return;
		}
		if (!empty($qs))
		{
			$qs .= "&"; 
		}
		$qs .= $val;
		return $qs;
	}	
	
	protected function createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = array();
		$params['key'] = $this->nullToEmpty($applicationKey);
		$params['country'] = $this->nullToEmpty($this->country);
		$params['customid'] = $this->nullToEmpty($this->customId);
		$params['callback'] = $this->nullToEmpty($callBackURL);
		$params['export'] = $this->nullToEmpty($this->exportUrl);
		$params['encryption'] = $this->nullToEmpty($this->encryptionKey);
		$params['proxy'] = $this->nullToEmpty($this->proxy);
		$params['sig'] = $sig;
		$params[$dataName] = $this->nullToEmpty($dataValue);
		
		return $params;
	}
	
	protected function nullToEmpty($value)
	{
		if (is_null($value))
		{
			return '';
		}
		return $value;
	}  	
	
	public function _getStartDelay()
	{
		if ($this->delay == null)
		{
			return 0;
		}
		else
		{
			return $this->delay;
		}
	}
}
?>