#!/usr/bin/perl

use CGI;
use CGI::Carp qw(warningsToBrowser fatalsToBrowser); 
use Config::Tiny;
use File::Spec;
use GrabzItClient;

#This Perl file handles the GrabzIt callback

$cgi = new CGI;

$Config = Config::Tiny->new();
$Config = Config::Tiny->read('GrabzIt.ini');

$message = $cgi->param("message");
$customId = $cgi->param("customid");
$id = $cgi->param("id");
$filename = $cgi->param("filename");

#Custom id can be used to store user ids or whatever is needed for the later processing of the
#resulting screenshot

$grabzIt = new GrabzItClient($Config->{GrabzIt}->{applicationKey}, $Config->{GrabzIt}->{applicationSecret});
$result = $grabzIt->GetPicture($id);

if ($result)
{
	#Ensure that the application has the correct rights for this directory.
	open FILE, ">".File::Spec->catfile("images",$filename) or die $!; 
	binmode FILE;
	print FILE $result; 
	close FILE;
}

print <<ENDOFTEXT;
HTTP/1.0 200 OK

ENDOFTEXT
exit(0);