<?php
error_reporting(E_ALL);

include("lib/GrabzItClient.php");
include("config.php");

$message = '';

//this helper function checks if it is possible to use the callback url
function useCallbackHandler($grabzItHandlerUrl)
{
	$serverAddr = isset($_SERVER['SERVER_ADDR']) ? $_SERVER['SERVER_ADDR'] : '';
	$localAddr = isset($_SERVER['LOCAL_ADDR']) ? $_SERVER['LOCAL_ADDR'] : '';
	return $serverAddr != '::1' && $localAddr != '::1' && $serverAddr != '127.0.0.1' && $localAddr != '127.0.0.1' && filter_var($grabzItHandlerUrl, FILTER_VALIDATE_URL) !== FALSE;
}

if (count($_POST) > 0)
{
	if (function_exists('get_magic_quotes_gpc') && get_magic_quotes_gpc())
	{
		//remove magic quotes from the input
		$_POST = array_map( 'stripslashes', $_POST);
	}

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
		$html = $_POST["html"];
		$format = $_POST["format"];
		$convert = $_POST["convert"];

		try
		{
			$grabzIt = new \GrabzIt\GrabzItClient($grabzItApplicationKey, $grabzItApplicationSecret);
			if ($format == "pdf")
			{
				if ($convert == 'html')
				{
					$grabzIt->HTMLToPDF($html);
				}
				else
				{
					$grabzIt->URLToPDF($url);
				}
			}
			else if ($format == "docx")
			{
				if ($convert == 'html')
				{
					$grabzIt->HTMLToDOCX($html);
				}
				else
				{
					$grabzIt->URLToDOCX($url);
				}
			}	
			else if ($format == "csv")
			{
				if ($convert == 'html')
				{
					$grabzIt->HTMLToTable($html);
				}
				else
				{
					$grabzIt->URLToTable($url);
				}
			}			
			else if ($format == "gif")
			{
				$grabzIt->URLToAnimation($url);
			}
			else
			{
				if ($convert == 'html')
				{
					$grabzIt->HTMLToImage($html);
				}
				else
				{
					$grabzIt->URLToImage($url);
				}
			}
			if (useCallbackHandler($grabzItHandlerUrl))
			{
				//This demo has a callback handler and is on a publicly accessible machine so use the recommended method
				$grabzIt->Save($grabzItHandlerUrl);
			}
			else
			{
				//It is not possible to use the recommended method so save the capture synchronously
				$grabzIt->SaveTo("results" . DIRECTORY_SEPARATOR . rand() . "." . $format);
			}
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
<script src="ajax/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
<script>
var ui = new UI('ajax/results.php?r=', 'css');
</script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.php" class="inputForms">
<p><span id="spnIntro">Enter the HTML or URL you want to convert into a DOCX, PDF or Image. The resulting capture</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="https://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>
<?php
if (!useCallbackHandler($grabzItHandlerUrl))
{
        ?><p>Either you have not updated the $grabzItHandlerUrl variable found in config.php file to match the URL of the handler.php file found in this demo app or you are using this demo application on your local machine.</p><p>This demo will still work although it will create captures synchronously, which will cause the web page to freeze when captures are generated. <u>Please wait for the capture to complete</u>.</p><?php
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
	else if (useCallbackHandler($grabzItHandlerUrl))
	{
	    ?><p><span style="color:green;font-weight:bold;">Processing...</span></p><?php
	}
}
?>
<div class="Row" id="divConvert">
<label>Convert </label><select name="convert" onchange="ui.selectConvertChanged(this)">
  <option value="url">URL</option>
  <option value="html">HTML</option>
</select>
</div>
<div id="divHTML" class="Row hidden">
<label>HTML </label><textarea name="html"><html><body><h1>Hello world!</h1></body></html></textarea>
</div>
<div id="divURL" class="Row">
<label>URL </label><input text="input" name="url" placeholder="http://www.example.com"/>
</div>
<div class="Row">
<label>Format </label><select name="format" onchange="ui.selectChanged(this)">
  <option value="jpg">JPG</option>  
  <option value="pdf">PDF</option>
  <option value="docx">DOCX</option>
  <option value="gif">GIF</option>
  <option value="csv">CSV</option>
</select>
</div>
<input type="submit" value="Grabz It" style="margin-left:12em"></input>
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