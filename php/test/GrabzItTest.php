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
        $this->applicationKey = "YOUR APPLICATION SECRET";
    }
    public function testHtmlToImage(): void
    {
        $client = new GrabzItClient($this->applicationKey, $this->applicationSecret);
        $client->HTMLToImage("<h1>Hello world</h1>");
        $this->assertEquals(true, $client->Save(), "HTML not converted");
    }
}
