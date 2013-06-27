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
<link rel="stylesheet" type="text/css" href="css/style.css">
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.pl" class="inputForms">
<p>Enter the URL of the website you want to take a screenshot of. Then resulting screenshot should be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>

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
		print '<p><span style="color:green;font-weight:bold;">Processing screenshot.</span></p>';
	}
}

print <<'FOOTER';
<label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/> <select name="format">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
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