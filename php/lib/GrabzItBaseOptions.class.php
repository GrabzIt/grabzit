<?php
class GrabzItBaseOptions
{
	private $customId = null;
	private $country = null;
	protected $delay = null;	
	
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
	Set the country the capture should be created from: Default = "", UK = "UK", US = "US".
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
	
	protected function createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = array();
		$params['key'] = $this->nullToEmpty($applicationKey);
		$params['country'] = $this->nullToEmpty($this->country);
		$params['customid'] = $this->nullToEmpty($this->customId);
		$params['callback'] = $this->nullToEmpty($callBackURL);
		$params['sig'] = $sig;		
		$params[$dataName] = $this->nullToEmpty($dataValue);
		
		return $params;
	}
	
	protected function nullToEmpty($value)
	{
		if ($value == null)
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