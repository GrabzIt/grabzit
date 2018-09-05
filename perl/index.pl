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
        $html = $cgi->param("html");
		$format = $cgi->param("format");
        $convert = $cgi->param("convert");

		my $ConfigFile = 'config.ini';
		tie my %ini, 'Config::IniFiles', (-file => $ConfigFile);
		my %Config = %{$ini{"GrabzIt"}};

		$grabzIt = GrabzItClient->new($Config{applicationKey}, $Config{applicationSecret});
		if ($format eq "pdf")
		{
            if ($convert eq "html")
            {
                $grabzIt->HTMLToPDF($html);
            }
            else
            {
                $grabzIt->URLToPDF($url);
            }
		}
		elsif ($format eq "docx")
		{
            if ($convert eq "html")
            {
                $grabzIt->HTMLToDOCX($html);
            }
            else
            {
                $grabzIt->URLToDOCX($url);
            }
		}        
		elsif ($format eq "csv")
		{
            if ($convert eq "html")
            {
                $grabzIt->HTMLToTable($html);
            }
            else
            {
                $grabzIt->URLToTable($url);
            }
		}		
		elsif ($format eq "gif")
		{
		    $grabzIt->URLToAnimation($url);
		}
		else
		{
            if ($convert eq "html")
            {
                $grabzIt->HTMLToImage($html);
            }
            else
            {
                $grabzIt->URLToImage($url);
            }
		}
		
		if ($cgi->remote_host() == '::1' || $cgi->remote_host() == "127.0.0.1")
		{
			eval {
				$grabzIt->SaveTo(File::Spec->catfile("results",(int(rand(9999999)).".".$format)));
			};		
		}
		else
		{
			eval {
				$grabzIt->Save($Config{handlerUrl});
			};
		}

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
<script src="ajax/jquery.min.js"></script>
<script src="ajax/ui.js"></script>
<script>
var ui = new UI('ajax/results.pl?r=', 'css');
</script>
</head>
<body>
<h1>GrabzIt Demo</h1>
<form method="post" action="index.pl" class="inputForms">
<p><span id="spnIntro">Enter the HTML or URL you want to convert into a DOCX, PDF or Image. The resulting capture</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="https://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>

HEADER
if ($cgi->remote_host() == '::1' || $cgi->remote_host() == "127.0.0.1")
{
	print '<p>As you are using this demo application on your local machine it will create captures synchronously, which will cause the web page to freeze while captures are generated. <u>Please wait for the capture to complete</u>.</p>';
}
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