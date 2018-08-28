<?php
namespace GrabzIt\Scraper;

use Exception;

class ScrapeResult
{
    private $extension;
    private $filename;
    private $data;

    public function __construct($path = null)
    {
        if ($path != null)
        {
            $this->data = $this->file_get_contents_utf8($path);
            $this->extension = strtolower(pathinfo($path, PATHINFO_EXTENSION));
            $this->filename = pathinfo($path, PATHINFO_BASENAME);
        }
        else if ($_SERVER['HTTP_USER_AGENT'] != 'GrabzIt')
        {
            throw new \Exception("A call originating from a non-GrabzIt server has been detected");
        }
    }
    
    private function file_get_contents_utf8($fn)
    { 
        $content = file_get_contents($fn); 
        return mb_convert_encoding($content, 'UTF-8', mb_detect_encoding($content, 'UTF-8, ISO-8859-1', true)); 
    }

    public function __toString()
    {
        return $this->toString();
    }

    public function toString()
    {
        if ($this->data == null && $_FILES['file']['error'] == UPLOAD_ERR_OK && is_uploaded_file($_FILES['file']['tmp_name']))
        {
            $this->data = $this->file_get_contents_utf8($_FILES['file']['tmp_name']);
        }
        if ($this->data != null && substr($this->data, 0, 3) == "\xef\xbb\xbf") 
        {
            $this->data = substr($this->data, 3);            
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