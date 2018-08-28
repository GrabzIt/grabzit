<?php
namespace GrabzIt\Scraper;

/*
Change a global variable contained within the scrape instructions
*/
class GrabzItVariable implements GrabzItIProperty
{
	private $name = null;
	private $value = null;
	
	/*
	Create the variable with the desired name. If a variable with the same name exists it will be overwritten
	*/
	public function __construct($name)
	{
		$this->name = $name;
	}
	
	/*
	Set the value of the variable
	*/
	public function SetValue($value)
	{
		$this->value = $value;
	}	
	
	public function GetTypeName()
	{
		return "Variable";
	}
	
	public function ToXML()
	{
		$xml = '<?xml version="1.0" encoding="UTF-8"?><Variable xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">';
		if ($this->value !== null && is_array($this->value))
		{
			$xml .= '<Array>';
			foreach($this->value as $key => $value)
			{
				if ($key === null && $value === null)
				{
					continue;
				}
				$xml .= '<KeyValue>';
				if ($key !== null)
				{
					$xml .= '<Key>';
					$xml .= $key;
					$xml .= '</Key>';
				}
				if ($value !== null)
				{
					$xml .= '<Value>';
					$xml .= $value;
					$xml .= '</Value>';
				}
				$xml .= '</KeyValue>';
			}
			$xml .= '</Array>';
		}
		else if ($this->value !== null)
		{
			$xml .= '<Value>';
			$xml .= $this->value;
			$xml .= '</Value>';
		}
		if ($this->name !== null)
		{
			$xml .= '<Name>';
			$xml .= $this->name;
			$xml .= '</Name>';
		}	
		$xml .= '</Variable>';
		return $xml;
	}
}
?>