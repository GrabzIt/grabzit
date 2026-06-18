<?php declare(strict_types=1);
use GrabzIt\GrabzItClient;
require("../vendor/autoload.php");
include("../lib/GrabzItClient.php");
use PHPUnit\Framework\TestCase;

final class GrabzItTest extends TestCase
{
    private $applicationKey;
    private $applicationSecret;
    protected function setUp(): void
    {        
        $this->applicationKey = "YOUR APPLICATION KEY";
        $this->applicationSecret = "YOUR APPLICATION SECRET";
    }
    public function testHtmlToImage(): void
    {
        $client = new GrabzItClient($this->applicationKey, $this->applicationSecret);
        $client->HTMLToImage("<h1>Hello world</h1>");
        $this->assertEquals(true, $client->Save(), "HTML not converted");
    }

    public function testHtmlToVideo(): void
    {
        $client = new GrabzItClient($this->applicationKey, $this->applicationSecret);
        $client->HTMLToVideo("<h1>Hello world</h1>");
        $this->assertEquals(true, $client->Save(), "HTML not converted");
    }  

    public function testHtmlToDocx(): void
    {
        $client = new GrabzItClient($this->applicationKey, $this->applicationSecret);
        $client->HTMLToDOCX("<h1>Hello world</h1>");
        $this->assertEquals(true, $client->Save(), "HTML not converted");
    }  	
	
    public function testHtmlToPDF(): void
    {
        $client = new GrabzItClient($this->applicationKey, $this->applicationSecret);
        $client->HTMLToPDF("<h1>Hello world</h1>");
        $this->assertEquals(true, $client->Save(), "HTML not converted");
    }

    public function testHoverImage(): void
    {
		$options = new \GrabzIt\GrabzItImageOptions();
		$options->setHoverElement(".demo-card");
		$options->setDelay(5000);
        $client = new GrabzItClient($this->applicationKey, $this->applicationSecret);
        $client->URLToImage("https://grabz.it/tests/hover.html", $options);
        $this->assertEquals(true, $client->Save(), "HTML not converted");
    }	
}
