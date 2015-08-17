#!/usr/bin/perl

use CGI;
use CGI::Carp qw(warningsToBrowser fatalsToBrowser); 
use File::Path;
use Config::IniFiles;
use GrabzIt::GrabzItClient;

$cgi = new CGI;
$message = '';

if ($cgi->request_method() eq 'POST')
{
	if ($cgi->param("delete") == "1")
	{
		unlink glob('results/*');
	}
	else
	{
		$url = $cgi->param("url");
		$format = $cgi->param("format");		

		my $ConfigFile = 'config.ini';
		tie my %ini, 'Config::IniFiles', (-file => $ConfigFile);
		my %Config = %{$ini{"GrabzIt"}};

		$grabzIt = new GrabzItClient($Config{applicationKey}, $Config{applicationSecret});
		if ($format eq "pdf")
		{
		    $grabzIt->SetPDFOptions($url);
		}
		if ($format eq "gif")
		{
		    $grabzIt->SetAnimationOptions($url);
		}		
		else
		{
		    $grabzIt->SetImageOptions($url);
		}		
		
		eval {
			$grabzIt->Save($Config{handlerUrl});
		};

		if ($@) {
		    $message = $@;
		}		
	}
}

print "Content-type: text/html\n\n";
print <<'HEADER';
<html>
<head>
<title>GrabzIt Demo</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<link rel="stylesheet" type="text/css" href="css/style.css">
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.pl" class="inputForms">
<p><span id="spnScreenshot">Enter the URL of the website you want to take a screenshot of. The resulting screenshot</span><span class="hidden" id="spnGif">Enter the URL of the online video you want to convert into a animated GIF. The resulting animated GIF</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>

HEADER

if ($cgi->request_method() eq 'POST' && $cgi->param("delete") != "1")
{ 
	if ($message ne '')
	{
		print '<p><span class="error">';
		print $message;
		print '</span></p>';
	}
	else
	{
		print '<p><span style="color:green;font-weight:bold;">Processing...</span></p>';
	}
}

print <<'FOOTER';
<label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/> <select name="format" onchange="selectChanged(this)">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
  <option value="gif">GIF</option>
</select>
<input type="submit" value="Grabz It"></input>
</form>
<form method="post" action="index.pl" class="inputForms">
<input type="hidden" name="delete" value="1"></input>
<input type="submit" value="Clear Results"></input>
</form>
    <br />
    <h2>Completed Screenshots</h2>
    <div id="divResults"></div>
</body>
</html>

FOOTER

1;