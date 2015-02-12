<?php
include("lib/GrabzItClient.class.php");
include("config.php");

$message = '';

if (count($_POST) > 0)
{
        if (isset($_POST["delete"]) && $_POST["delete"] == 1)
        {
            $files = glob('results/*');
            foreach($files as $file)
            {
                if(is_file($file))
                    unlink($file);
            }
        }
        else
        {
		$url = $_POST["url"];
		$format = $_POST["format"];
		try
		{
			$grabzIt = new GrabzItClient($grabzItApplicationKey, $grabzItApplicationSecret);
			if ($format == "pdf")
			{
			    $grabzIt->SetPDFOptions($url);
			}
			else
			{
			    $grabzIt->SetImageOptions($url);
			}
			$grabzIt->Save($grabzItHandlerUrl);
		}
		catch (Exception $e)
		{
		    $message =  $e->getMessage();
		}
	}
}
?>
<html>
<head>
<title>GrabzIt Demo</title>
<link rel="stylesheet" type="text/css" href="css/style.css">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.php" class="inputForms">
<p>Enter the URL of the website you want to take a screenshot of. Then resulting screenshot should be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>
<?php
if ($grabzItHandlerUrl == "URL OF YOUR handler.php FILE (http://www.example.com/grabzit/handler.php)")
{
        ?><p><span class="error">You must set your call back to a valid public URL.</span></p><?php
}
if (!is_writable("results"))
{
    ?><span class="error">The "results" directory is not writeable! This directory needs to be made writeable in order for this demo to work.</span><?php
    return;
}
if (count($_POST) > 0 && !isset($_POST["delete"]))
{
	if (!empty($message))
	{
	    ?><p><span class="error"><?php echo $message; ?></span></p><?php
	}
	else
	{
	    ?><p><span style="color:green;font-weight:bold;">Processing screenshot.</span></p><?php
	}
}
?>
<label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/> <select name="format">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
</select>
<input type="submit" value="Grabz It"></input>
</form>
<form method="post" action="index.php" class="inputForms">
<input type="hidden" name="delete" value="1"></input>
<input type="submit" value="Clear Results"></input>
</form>
    <br />
    <h2>Completed Screenshots</h2>
    <div id="divResults"></div>
</body>
</html>