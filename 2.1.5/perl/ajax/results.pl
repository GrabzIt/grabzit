#!/usr/bin/perl

print "Content-Type: application/json;charset=utf-8\n\n";

my $json;
@files = glob('./results/*');

foreach $file (@files) 
{
	if (index($file, ".txt") == -1) 
	{
		if ($json ne "")
		{
			$json .= ",";
		}
		$file =~ s/\.\.\///ig;
		$json .= "\"".$file."\"";
	}
}

print "[", $json, "]";
    
