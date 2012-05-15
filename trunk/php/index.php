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
<html>
<head>
<title>GrabzIt Demo</title>
</head>
<body>
<form method="post" handler="index.php">
<p>Enter the URL of the website you want to take a screenshot of. Then check the <a href="images/">images directory</a> for the result. It may take a few seconds for it to appear!</p>
<p>If nothing is happening check the <a href="http://grabz.it/account/diagnostics">diagnostics panel</a> to see if there is an error.</p>
<label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/>
<input type="submit" value="Grabz"></input>
</form>
</body>
</html>