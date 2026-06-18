#!/usr/bin/perl
use lib '../GrabzIt';
use GrabzItClient;
use GrabzItPDFOptions;
use Test::More;
use Test::Exception;

$grabzIt = GrabzItClient->new("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET");

$grabzIt->HTMLToImage("<h1>Hello World!</h1>");
lives_ok {$grabzIt->Save() } 'HTML to Image';

$grabzIt->HTMLToVideo("<h1>Hello World!</h1>");
lives_ok {$grabzIt->Save() } 'HTML to Video';

$options = GrabzItPDFOptions->new();
$options->hoverElement(".demo-card");
$options->delay(5000);
$grabzIt->URLToPDF("https://grabz.it/tests/hover.html", $options);
lives_ok {$grabzIt->Save() } 'URL to PDF';

done_testing();