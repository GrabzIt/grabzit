#!/usr/bin/perl

use CGI;
use CGI::Carp qw(warningsToBrowser fatalsToBrowser); 
use Config::Tiny;
use GrabzItClient;

$cgi = new CGI;

$url = $cgi->param("url");

$Config = Config::Tiny->new();
$Config = Config::Tiny->read('GrabzIt.ini');

$grabzIt = new GrabzItClient($Config->{GrabzIt}->{applicationKey}, $Config->{GrabzIt}->{applicationSecret});
$grabzIt->TakePicture($url, $Config->{GrabzIt}->{handlerUrl});

print "Location: index.html\n\n";