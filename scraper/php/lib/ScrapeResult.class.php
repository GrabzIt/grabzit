<?php
class ScrapeResult
{
    private $extension;
    private $filename;
	private $data;

	public function __construct($path = null)
	{
        if ($path != null)
        {
		    $this->data = file_get_contents($path);
		    $this->extension = strtolower(pathinfo($path, PATHINFO_EXTENSION));
            $this->filename = pathinfo($path, PATHINFO_BASENAME);
        }
        else if ($_SERVER['HTTP_USER_AGENT'] != 'GrabzIt')
        {
        	throw new Exception("A call originating from a non-GrabzIt server has been detected");
        }
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

    public function getFilename()
	{
		if ($this->filename == null && $_FILES['file']['name'] != null)
		{
			$this->filename = pathinfo($_FILES['file']['name'], PATHINFO_BASENAME);
		}
		return $this->filename;
	}

	public function getExtension()
	{
		if ($this->extension == null && $_FILES['file']['name'] != null)
		{
			$this->extension = strtolower(pathinfo($_FILES['file']['name'], PATHINFO_EXTENSION));
		}
		return $this->extension;
	}

	public function save($path)
	{
        $dat = $this->toString();
        if ($dat != null)
        {
		    return file_put_contents($path, $dat) !== false;
        }
        return false;
	}
}
?>