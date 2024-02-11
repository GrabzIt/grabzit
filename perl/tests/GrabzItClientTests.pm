#!/usr/bin/perl
use lib '../GrabzIt';
use GrabzItClient;
use Test::More;
use Test::Exception;

$grabzIt = GrabzItClient->new("c3VwcG9ydEBncmFiei5pdA==", "AD8/aT8/Pz8/Tz8/PwJ3Pz9sVSs/Pz8/Pz9DOzJodoi=");

$grabzIt->HTMLToImage("<h1>Hello World!</h1>");
lives_ok {$grabzIt->Save() } 'HTML to Image';

$grabzIt->HTMLToVideo("<h1>Hello World!</h1>");
lives_ok {$grabzIt->Save() } 'HTML to Video';

done_testing();