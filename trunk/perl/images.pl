#!/usr/bin/perl

print <<ENDOFSTARTTEXT;
HTTP/1.0 200 OK
Content-Type: text/html

<html>
<head>
</head>
<body>
<div style='width:800px;'>
ENDOFSTARTTEXT

@files = <images/*>;
foreach $file (@files) 
{
	if (index($file, ".txt") == -1) 
	{
		print "<img src='".$file."' style='margin-right:1em;'/>";
	}
}
    
print "</div>";
print "</body>";
print "</html>";