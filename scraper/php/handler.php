<?php
include("lib/ScrapeResult.php");

$scrapeResult = new \GrabzIt\Scraper\ScrapeResult();
$scrapeResult->save("results/".$scrapeResult->getFilename());
?>