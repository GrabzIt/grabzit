#!/usr/bin/perl
use lib '../GrabzIt';
use GrabzItClient;
use Test::More;
use Test::Exception;

$grabzIt = GrabzItClient->new("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET");

$grabzIt->HTMLToImage("<h1>Hello World!</h1>");
lives_ok {$grabzIt->Save() } 'HTML to Image';

$grabzIt->HTMLToVideo("<h1>Hello World!</h1>");
lives_ok {$grabzIt->Save() } 'HTML to Video';

done_testing();