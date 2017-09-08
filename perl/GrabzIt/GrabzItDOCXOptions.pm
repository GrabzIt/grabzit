#!/usr/bin/perl 

package GrabzItDOCXOptions;

use GrabzIt::GrabzItBaseOptions;

@ISA = qw(GrabzItBaseOptions);

sub new
{
    my $class = shift;           
    my $self = GrabzItBaseOptions->new(@_);
    
    $self->{"includeBackground"} = 1;
    $self->{"pagesize"} = "A4";
    $self->{"orientation"} = "Portrait";
    $self->{"includeLinks"} = 1;
    $self->{"includeImages"} = 1;
    $self->{"title"} = '';
    $self->{"marginTop"} = 10;
    $self->{"marginLeft"} = 10;
    $self->{"marginBottom"} = 10;
    $self->{"marginRight"} = 10;
    $self->{"requestAs"} = 0;
    $self->{"quality"} = -1;
    $self->{"hideElement"} = '';
    $self->{"waitForElement"} = '';
	$self->{"noAds"} = 0;    
        
    bless $self, $class;

    return $self;
}

#
# True if the background images of the web page should be included in the DOCX
#
sub includeBackground
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"includeBackground"} = shift;
    }
    return $self->{"includeBackground"};
}

#
# The page size of the DOCX to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'
#
sub pagesize
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"pagesize"} = uc(shift);
    }
    return $self->{"pagesize"};
}

#
# The orientation of the DOCX to be returned: 'Landscape' or 'Portrait'
#
sub orientation
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"orientation"} = ucfirst(shift);
    }
    return $self->{"orientation"};
}

#
# True if links should be included in the DOCX
#
sub includeLinks
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"includeLinks"} = shift;
    }
    return $self->{"includeLinks"};
}

#
# True if the images of the web page should be included in the DOCX
#
sub includeImages
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"includeImages"} = shift;
    }
    return $self->{"includeImages"};
}


#
# Title for the DOCX document
#
sub title
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"title"} = shift;
    }
    return $self->{"title"};
}

#
# The margin that should appear at the top of the DOCX document page
#
sub marginTop
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"marginTop"} = shift;
    }
    return $self->{"marginTop"};
}

#
# The margin that should appear at the left of the DOCX document page
#
sub marginLeft
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"marginLeft"} = shift;
    }
    return $self->{"marginLeft"};
}

#
# The margin that should appear at the bottom of the DOCX document page
#
sub marginBottom
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"marginBottom"} = shift;
    }
    return $self->{"marginBottom"};
}

#
# The margin that should appear at the right of the DOCX document
#
sub marginRight
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"marginRight"} = shift;
    }
    return $self->{"marginRight"};
}

#
# The number of milliseconds to wait before creating the capture
#
sub delay
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"delay"} = shift;
    }
    return $self->{"delay"};
}

#
# The user agent type should be used: Standard Browser = 0, Mobile Browser = 1, Search Engine = 2 and Fallback Browser = 3
#
sub requestAs
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"requestAs"} = shift;
    }
    return $self->{"requestAs"};
}

#
# The quality of the DOCX where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
#
sub quality
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"quality"} = shift;
    }
    return $self->{"quality"};
}

#
# The CSS selector(s) of the one or more HTML elements in the web page to hide
#
sub hideElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"hideElement"} = shift;
    }
    return $self->{"hideElement"};
}

#
# The CSS selector of the HTML element in the web page that must be visible before the capture is performed
#
sub waitForElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"waitForElement"} = shift;
    }
    return $self->{"waitForElement"};
}

#
# True if adverts should be automatically hidden.
#
sub noAds
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"noAds"} = shift;
    }
    return $self->{"noAds"};
}

#
#Define a HTTP Post parameter and optionally value, this method can be called multiple times to add multiple parameters. Using this method will force 
#GrabzIt to perform a HTTP post.
#
#name - The name of the HTTP Post parameter.
#value - The value of the HTTP Post parameter
#
sub AddPostParameter($$)
{
	my ($self, $name, $value) = @_;
	$self->{"post"} = $self->_appendPostParameter($self->{"post"}, $name, $value);
}

sub _getSignatureString($$;$)
{
    my ($self, $applicationSecret, $callBackURL, $url) = @_;
    
    $url ||= '';
    
    $urlParam = '';
    if ($url ne '')
    {
        $urlParam = $url."|";
    }
    
    $callBackURLParam = '';
    if ($callBackURL ne '')
    {
        $callBackURLParam = $callBackURL;
    }
    
    return $applicationSecret."|". $urlParam . $callBackURLParam .
    "|".$self->customId() ."|".$self->includeBackground() ."|".$self->pagesize() ."|".$self->orientation()."|".$self->includeImages()."|".$self->includeLinks()."|".$self->title()."|".$self->marginTop()."|".$self->marginLeft()."|".$self->marginBottom()."|".$self->marginRight().
    "|".$self->delay()."|".$self->requestAs()."|".$self->country()."|".$self->quality()."|".$self->hideElement()."|".$self->exportURL()."|".
    $self->waitForElement()."|".$self->encryptionKey()."|".$self->noAds()."|".$self->{"post"};
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'background'} = $self->includeBackground();
    $params->{'pagesize'} = $self->pagesize();
    $params->{'orientation'} = $self->orientation();
    $params->{'includelinks'} = $self->includeLinks();
    $params->{'includeimages'} = $self->includeImages();
    $params->{'title'} = $self->title();
    $params->{'mleft'} = $self->marginLeft();
    $params->{'mright'} = $self->marginRight();
    $params->{'mtop'} = $self->marginTop();
    $params->{'mbottom'} = $self->marginBottom();
    $params->{'delay'} = $self->delay();
    $params->{'requestmobileversion'} = $self->requestAs();
    $params->{'quality'} = $self->quality();
    $params->{'hide'} = $self->hideElement();
    $params->{'waitfor'} = $self->waitForElement();
	$params->{'noads'} = $self->noAds();
	$params->{'post'} = $self->{"post"};
    
    return $params;
}
1;