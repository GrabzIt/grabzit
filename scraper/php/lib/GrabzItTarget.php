<?php
namespace GrabzIt\Scraper;
/*
Change the target of a scrape
*/
class GrabzItTarget implements GrabzItIProperty
{
	private $seedUrls = null;
	private $url = null;
	
	/*
	Specify the URL to start the scrape on
	*/
	public function SetURL($url)
	{
		$this->url = $url;
	}
	
	/*
	Specify the seed URL's of a scrape, if any
	*/
	public function SetSeedURLs($seedUrls)
	{
		$this->seedUrls = $seedUrls;
	}	
	
	public function GetTypeName()
	{
		return "Target";
	}
	
	public function ToXML()
	{
		$xml = '<?xml version="1.0" encoding="UTF-8"?><Target xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">';
		if ($this->seedUrls != null && is_array($this->seedUrls))
		{
			$xml .= '<SeedURLs>';
			foreach($this->seedUrls as $value)
			{
				$xml .= '<string>';
				$xml .= $value;
				$xml .= '</string>';
			}
			$xml .= '</SeedURLs>';
		}
		if ($this->url != null)
		{
			$xml .= '<URL>';
			$xml .= $this->url;
			$xml .= '</URL>';
		}
		$xml .= '</Target>';
		return $xml;
	}
}
?>