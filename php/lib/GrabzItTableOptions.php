<?php
namespace GrabzIt;

class GrabzItTableOptions extends GrabzItBaseOptions
{
	private $requestAs = 0;
	private $format = 'csv';
	private $targetElement = null;
	private $tableNumberToInclude = 1;
	private $includeHeaderNames = true;
	private $includeAllTables = false;
	private $address = null;
	
	/*
	Set to true to extract every table on the web page into a separate spreadsheet sheet. Only available with the XLSX and JSON formats.
	*/
	public function setIncludeAllTables($value)
	{
		$this->includeAllTables = $value;
	}

	/*
	Get if every table on will be extracted with each table appearing in a separate spreadsheet sheet.
	*/
	public function getIncludeAllTables()
	{
		return $this->includeAllTables;
	}

	/*
	Set to true to include header names into the table.
	*/
	public function setIncludeHeaderNames($value)
	{
		$this->includeHeaderNames = $value;
	}

	/*
	Get if the header names are included in the table.
	*/
	public function getIncludeHeaderNames()
	{
		return $this->includeHeaderNames;
	}

	/*
	Set the index of the table to be converted, were all tables in a web page are ordered from the top of the web page to bottom.
	*/
	public function setTableNumberToInclude($value)
	{
		$this->tableNumberToInclude = $value;
	}

	/*
	Get the index of the table to be converted.
	*/
	public function getTableNumberToInclude()
	{
		return $this->tableNumberToInclude;
	}

	/*
	Set the format the table should be in: csv, xlsx or json.
	*/
	public function setFormat($value)
	{
		$this->format = $value;
	}

	/*
	Get the format of the table should be.
	*/
	public function getFormat()
	{
		return $this->format;
	}

	/*
	Set the id of the only HTML element in the web page that should be used to extract tables from.
	*/
	public function setTargetElement($value)
	{
		$this->targetElement = $value;
	}

	/*
	Get the id of the only HTML element in the web page that should be used to extract tables from.
	*/
	public function getTargetElement()
	{
		return $this->targetElement;
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
	Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
	GrabzIt to perform a HTTP post.

    name - The name of the HTTP Post parameter.
	value - The value of the HTTP Post parameter.
    */		
	public function AddPostParameter($name, $value)
	{
		$this->post = $this->appendParameter($this->post, $name, $value);
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

		return $this->nullToEmpty($applicationSecret)."|". $urlParam . $callBackURLParam .
		"|".$this->nullToEmpty($this->getCustomId())."|".$this->nullToEmpty($this->tableNumberToInclude)."|".$this->nullToEmpty(intval($this->includeAllTables))."|".
		$this->nullToEmpty(intval($this->includeHeaderNames))."|".$this->nullToEmpty($this->targetElement)."|".$this->nullToEmpty($this->format)."|".
		$this->nullToEmpty(intval($this->requestAs))."|".$this->nullToEmpty($this->getCountry()."|".$this->nullToEmpty($this->getExportURL()))."|".
		$this->nullToEmpty($this->getEncryptionKey())."|".$this->nullToEmpty($this->post)."|".$this->nullToEmpty($this->getProxy())."|".$this->nullToEmpty($this->address);
	}

	public function _getParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue)
	{
		$params = $this->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
		$params['includeAllTables'] = $this->nullToEmpty(intval($this->includeAllTables));
		$params['includeHeaderNames'] = $this->nullToEmpty(intval($this->includeHeaderNames));
		$params['format'] = $this->nullToEmpty($this->format);
		$params['tableToInclude'] = $this->nullToEmpty($this->tableNumberToInclude);
		$params['target'] = $this->nullToEmpty($this->targetElement);
		$params['requestmobileversion'] = $this->nullToEmpty(intval($this->requestAs));
		$params['post'] = $this->nullToEmpty($this->post);	
		$params['address'] = $this->nullToEmpty($this->address);
		
		return $params;
	}
}
?>