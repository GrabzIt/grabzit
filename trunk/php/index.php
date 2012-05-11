<?php
include("GrabzItClient.class.php");
include("GrabzItConfig.php");

if (count($_POST) > 0)
{
	$url = $_POST["url"];
	$grabzIt = new GrabzItClient($grabzItApplicationKey, $grabzItApplicationSecret);
	$grabzIt->TakePicture($url, $grabzItHandlerUrl);
}
?>
<form method="post" handler="index.php">
<label>Url </label><input text="input" name="url"/>
<input type="submit" value="Grabz"></input>
</form>