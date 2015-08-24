<?php
include("ScrapeResult.class.php");
$scrapeResult = new ScrapeResult();
$scrapeResult->save("results/".$scrapeResult->getFilename());
?>