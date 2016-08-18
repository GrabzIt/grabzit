<?php
include("lib/GrabzItScrapeClient.class.php");
include("config.php");

$message = '';
$grabzIt = new GrabzItScrapeClient($grabzItApplicationKey, $grabzItApplicationSecret);

if (count($_GET) > 0)
{
	$id = $_GET["id"];
	$status = $_GET["status"];
	try
	{
		$grabzIt->SetScrapeStatus($id, $status);
	}
	catch (Exception $e)
	{
		$message =  $e->getMessage();
	}
}
?>
<html>
<head>
<title>GrabzIt Scraper Demo</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
</head>
<body>
<?php
if (count($_GET) > 0)
{
	if (!empty($message))
	{
	    ?><p><span style="color:red;font-weight:bold;"><?php echo $message; ?></span></p><?php
	}
	else
	{
	    ?><p><span style="color:green;font-weight:bold;">Succesfully update scrape</span></p><?php
	}
}
?>
<table>
<tr><th>Scrape Name</th><th>Scrape Status</th><th></th></tr>
<?php
$scrapes = $grabzIt->GetScrapes();
foreach($scrapes as $scrape)
{
	?><tr><td><?php
	echo $scrape->Name;
	?></td><td><?php
	echo $scrape->Status;
	?></td><td><a href="index.php?id=<?php	echo $scrape->id;?>&status=Start">Start</a> <a href="index.php?id=<?php	echo $scrape->id;?>&status=Stop">Stop</a> <a href="index.php?id=<?php	echo $scrape->id;?>&status=Disable">Disable</a> <a href="index.php?id=<?php	echo $scrape->id;?>&status=Enable">Enable</a>
	</td></tr><?php
}
?>
</table>
</body>
</html>