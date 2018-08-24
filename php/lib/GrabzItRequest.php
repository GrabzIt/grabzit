<?php
namespace GrabzIt;

class GrabzItRequest
{
	private $url;
	private $isPost = false;
	private $options;
	private $data;

	public function __construct($url, $isPost, $options, $data = null)
	{
		$this->url = $url;
		$this->data = $data;
		$this->isPost = $isPost;
		$this->options = $options;
	}

	public function isPost()
	{
		return $this->isPost;
	}
	
	public function getTargetUrl()
	{
		if ($this->isPost)
		{
			return '';
		}
		return $this->data;
	}

	public function getUrl()
	{
		return $this->url;
	}
	
	public function getData()
	{
		return $this->data;
	}

	public function getOptions()
	{
		return $this->options;
	}
}
?>