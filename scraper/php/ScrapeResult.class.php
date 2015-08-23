<?php
class ScrapeResult
{
	private $extension;
	private $data;

	public function __construct($data = null, $extension = null)
	{
		$this->data = $data;
		$this->extension = $extension;
	}

	public function __toString()
	{
		return $this->toString();
	}

	public function toString()
	{
		if ($this->data == null && $_FILES['file']['error'] == UPLOAD_ERR_OK && is_uploaded_file($_FILES['file']['tmp_name']))
		{
			$this->data = file_get_contents($_FILES['file']['tmp_name']);
		}
		return $this->data;
	}

	public function toXML()
	{
		if ($this->getExtension() == 'xml')
		{
			return new SimpleXMLElement($this->toString());
		}
		return null;
	}

	public function toJSON()
	{
		if ($this->getExtension() == 'json')
		{
			return json_decode($this->toString());
		}
		return null;
	}

	public function getExtension()
	{
		if ($this->extension == null && $_FILES['file']['name'] != null)
		{
			$this->extension = pathinfo($_FILES['file']['name'], PATHINFO_EXTENSION);
		}
		return $this->extension;
	}

	public function save($filename)
	{
		return file_put_contents($filename, $this->toString()) !== false;
	}
}