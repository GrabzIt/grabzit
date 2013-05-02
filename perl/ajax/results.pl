#!/usr/bin/perl

print "HTTP/1.0 200 OK";
print "Content-Type: application/json;charset=utf-8\r\n\r\n";

my $json;
@files = <../results/*>;

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
    
