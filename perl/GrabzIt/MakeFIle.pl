#!/usr/bin/perl

use strict;
use warnings;
use ExtUtils::MakeMaker;
 
WriteMakefile(
    NAME         => 'GrabzItClient',
    AUTHOR       => q{GrabzIt <support@grabz.it>},
    VERSION => '3.5.8',
    ABSTRACT     => 'GrabzIt enables allows you to programmatically convert web pages and HTML into images, DOCX, videos, rendered HTML, PDFs, CSVs and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIFs',
    ( $ExtUtils::MakeMaker::VERSION >= 6.3002
        ? ( 'LICENSE' => 'perl' )
        : () ),
    PL_FILES  => {},
    PREREQ_PM => {
        'Digest::MD5'      => 2.58,
        'Encode'   => 3.08,
		'LWP::Simple'   => 6.55,
		'LWP::UserAgent'   => 6.55,
		'LWP::UserAgent'   => 6.55,
		'HTTP::Request::Common'   => 6.27,
		'URI::Escape'   => 5.06,
		'XML::Twig'   => 3.52,
		'File::Spec'   => 3.78,
		'File::Basename'   => 2.85,
    },
    TEST_REQUIRES => {
          'Test::More'    => 1.302183,
		  'Test::Exception' => 0.43,
    },
);