<?php
include("GrabzItClient.class.php");
include("GrabzItConfig.php");

$message = '';

if (count($_POST) > 0)
{
	$url = $_POST["url"];
	try
	{
		$grabzIt = new GrabzItClient($grabzItApplicationKey, $grabzItApplicationSecret);
		$grabzIt->TakePicture($url, $grabzItHandlerUrl);
	}
	catch (Exception $e)
	{
	    $message =  $e->getMessage();
	}	
}
?>
<html>
<head>
<title>GrabzIt Demo</title>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.php">
<p>Enter the URL of the website you want to take a screenshot of. Then resulting screenshot should be saved in the <a href="images/">images directory</a>. It may take a few seconds for it to appear!</p>
<p>If nothing is happening check the <a href="http://grabz.it/account/diagnostics">diagnostics panel</a> to see if there is an error.</p>
<?php
if (count($_POST) > 0)
{
	if (!empty($message))
	{
	    ?><p><span style="color:red;font-weight:bold;"><?php echo $message; ?></span></p><?php
	}
	else
	{
	    ?><p><span style="color:green;font-weight:bold;">Processing screenshot.</span></p><?php
	}
}
?>
<label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/>
<input type="submit" value="Grabz It"></input>
</form>
    <br />
    <h2>Completed Screenshots</h2>
    <iframe id="iFImages" src="images.php" width="100%" height="500px;" style="border:0;overflow-y:auto;"></iframe>
    </div>
    </form>
    <script type="text/javascript">
            function reloadIFrame() {
                var iframe = document.getElementById('iFImages');
                iframe.src = iframe.src;
                setTimeout("reloadIFrame()", 5000);
            }

            reloadIFrame();
    </script>
</body>
</html>