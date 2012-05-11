<?php
include("GrabzItClient.class.php");
include("GrabzItConfig.php");

//This PHP file handles the GrabzIt callback

$message = $_GET["message"];
$customId = $_GET["customid"];
$id = $_GET["id"];
$filename = $_GET["filename"];

//Custom id can be used to store user ids or whatever is needed for the later processing of the
//resulting screenshot

$grabzIt = new GrabzItClient($grabzItApplicationKey, $grabzItApplicationSecret);
$result = $grabzIt->GetPicture($id);

if (!$result)
{
	return;
}

//Ensure that the application has the correct rights for this directory.
file_put_contents("images" . DIRECTORY_SEPARATOR . $filename, $result);
?>