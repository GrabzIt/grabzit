#!/usr/bin/perl 

package GrabzItPDFOptions;

use GrabzIt::GrabzItBaseOptions;

@ISA = qw(GrabzItBaseOptions);

sub new
{
    my $class = shift;           
    my $self = GrabzItBaseOptions->new(@_);
    
    $self->{"browserWidth"} = 0;
    $self->{"includeBackground"} = 1;
    $self->{"pagesize"} = "A4";
    $self->{"orientation"} = "Portrait";
    $self->{"includeLinks"} = 1;
    $self->{"includeOutline"} = 0;
    $self->{"title"} = '';
    $self->{"coverURL"} = '';
    $self->{"marginTop"} = 10;
    $self->{"marginLeft"} = 10;
    $self->{"marginBottom"} = 10;
    $self->{"marginRight"} = 10;
    $self->{"requestAs"} = 0;
    $self->{"customWaterMarkId"} = '';
    $self->{"quality"} = -1;
    $self->{"templateId"} = '';
    $self->{"targetElement"} = '';
    $self->{"hideElement"} = '';
    $self->{"waitForElement"} = '';
    $self->{"noAds"} = 0;
    $self->{"templateVariables"} = '';
    $self->{"width"} = 0;
    $self->{"height"} = 0;
	$self->{"mergeId"} = '';
	$self->{"address"} = '';
	$self->{"noCookieNotifications"} = 0;
	$self->{"cssMediaType"} = '';
	$self->{"password"} = '';
    $self->{"clickElement"} = '';
        
    bless $self, $class;

    return $self;
}

#
# True if the background of the web page should be included in the PDF
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
# The page size of the PDF to be returned: 'A3', 'A4', 'A5', 'A6', 'B3', 'B4', 'B5', 'B6', 'Letter'
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
# The orientation of the PDF to be returned: 'Landscape' or 'Portrait'
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
# The CSS Media Type of the PDF to be returned: 'Print' or 'Screen'
#
sub cssMediaType
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"cssMediaType"} = ucfirst(shift);
    }
    return $self->{"cssMediaType"};
}

#
# Protect the PDF document with this password
#
sub password
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"password"} = shift;
    }
    return $self->{"password"};
}

#
# True if links should be included in the PDF
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
# True if the PDF outline should be included
#
sub includeOutline
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"includeOutline"} = shift;
    }
    return $self->{"includeOutline"};
}


#
# Title for the PDF document
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
# The URL of a web page that should be used as a cover page for the PDF
#
sub coverURL
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"coverURL"} = shift;
    }
    return $self->{"coverURL"};
}

#
# The margin that should appear at the top of the PDF document page
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
# The margin that should appear at the left of the PDF document page
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
# The margin that should appear at the bottom of the PDF document page
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
# The margin that should appear at the right of the PDF document
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
# The width of the browser in pixels
#
sub browserWidth
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"browserWidth"} = shift;
    }
    return $self->{"browserWidth"};
}

#
# The width of the PDF in mm
#
sub pageWidth
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"width"} = shift;
    }
    return $self->{"width"};
}

#
# The height of the PDF in mm
#
sub pageHeight
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"height"} = shift;
    }
    return $self->{"height"};
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
# The custom watermark to add to the PDF
#
sub customWaterMarkId
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"customWaterMarkId"} = shift;
    }
    return $self->{"customWaterMarkId"};
}

#
# The quality of the PDF where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
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
# The template ID that specifies the header and footer of the PDF document
#
sub templateId
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"templateId"} = shift;
    }
    return $self->{"templateId"};
}

#
# The ID of a capture that should be merged at the beginning of the new PDF document
#
sub mergeId
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"mergeId"} = shift;
    }
    return $self->{"mergeId"};
}

#
# The CSS selector of the HTML element in the web page to click
#
sub clickElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"clickElement"} = shift;
    }
    return $self->{"clickElement"};
}

#
# The CSS selector of the only HTML element in the web page to capture
#
sub targetElement
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"targetElement"} = shift;
    }
    return $self->{"targetElement"};
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
# True if cookie notification should be automatically hidden.
#
sub noCookieNotifications
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"noCookieNotifications"} = shift;
    }
    return $self->{"noCookieNotifications"};
}

#
# The URL to execute the HTML code in.
#
sub address
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"address"} = shift;
    }
    return $self->{"address"};
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

#
#Define a custom Template parameter and value, this method can be called multiple times to add multiple parameters.
#
#name - The name of the template parameter
#value - The value of the template parameter
#
sub AddTemplateParameter($$)
{
    my ($self, $name, $value) = @_;
    $self->{"templateVariables"} = $self->_appendPostParameter($self->{"templateVariables"}, $name, $value);
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
    "|".$self->customId() ."|".$self->includeBackground() ."|".$self->pagesize() ."|".$self->orientation()."|".$self->customWaterMarkId()."|".$self->includeLinks().
    "|".$self->includeOutline()."|".$self->title()."|".$self->coverURL()."|".$self->marginTop()."|".$self->marginLeft()."|".$self->marginBottom()."|".$self->marginRight().
    "|".$self->delay()."|".$self->requestAs()."|".$self->country()."|".$self->quality()."|".$self->templateId()."|".$self->hideElement().
    "|".$self->targetElement()."|".$self->exportURL()."|".$self->waitForElement()."|".$self->encryptionKey()."|".$self->noAds()."|".$self->{"post"}.
    "|".$self->browserWidth()."|".$self->pageHeight()."|".$self->pageWidth()."|".$self->{"templateVariables"}."|".$self->proxy()."|".$self->mergeId().
	"|".$self->address()."|".$self->noCookieNotifications()."|".$self->cssMediaType()."|".$self->password()."|".$self->clickElement();
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'background'} = $self->includeBackground();
    $params->{'pagesize'} = $self->pagesize();
    $params->{'orientation'} = $self->orientation();
    $params->{'templateid'} = $self->templateId();
    $params->{'customwatermarkid'} = $self->customWaterMarkId();
    $params->{'includelinks'} = $self->includeLinks();
    $params->{'includeoutline'} = $self->includeOutline();
    $params->{'title'} = $self->title();
    $params->{'coverurl'} = $self->coverURL();
    $params->{'mleft'} = $self->marginLeft();
    $params->{'mright'} = $self->marginRight();
    $params->{'mtop'} = $self->marginTop();
    $params->{'mbottom'} = $self->marginBottom();
    $params->{'delay'} = $self->delay();
    $params->{'requestmobileversion'} = $self->requestAs();
    $params->{'quality'} = $self->quality();
    $params->{'target'} = $self->targetElement();
    $params->{'hide'} = $self->hideElement();
    $params->{'waitfor'} = $self->waitForElement();
    $params->{'noads'} = $self->noAds();
    $params->{'post'} = $self->{"post"};
    $params->{'bwidth'} = $self->browserWidth();
    $params->{'width'} = $self->pageWidth();
    $params->{'height'} = $self->pageHeight();
    $params->{'tvars'} = $self->{"templateVariables"};
    $params->{'mergeid'} = $self->mergeId();
	$params->{'nonotify'} = $self->noCookieNotifications();
	$params->{'address'} = $self->address();
	$params->{'media'} = $self->cssMediaType();
	$params->{'password'} = $self->password();
    $params->{'click'} = $self->clickElement();
	
    return $params;
}
1;