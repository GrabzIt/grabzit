<?php
include("lib/GrabzItClient.class.php");
include("config.php");

//This PHP file handles the GrabzIt callback

$message = $_GET["message"];
$customId = $_GET["customid"];
$id = $_GET["id"];
$filename = $_GET["filename"];
$format = $_GET["format"];

//Custom id can be used to store user ids or whatever is needed for the later processing of the
//resulting screenshot

$grabzIt = new GrabzItClient($grabzItApplicationKey, $grabzItApplicationSecret);
$result = $grabzIt->GetResult($id);

if (!$result)
{
   return;
}

//Ensure that the application has the correct rights for this directory.
file_put_contents("results" . DIRECTORY_SEPARATOR . $filename, $result);
?>