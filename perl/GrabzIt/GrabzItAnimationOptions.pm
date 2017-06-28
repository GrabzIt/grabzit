#!/usr/bin/perl 

package GrabzItAnimationOptions;

use GrabzIt::GrabzItBaseOptions;

@ISA = qw(GrabzItBaseOptions);

sub new
{
    my $class = shift;           
    my $self = GrabzItBaseOptions->new(@_);
    $self->{"width"} = 0;
    $self->{"height"} = 0;
    $self->{"start"} = 0;
    $self->{"duration"} = 1;
    $self->{"speed"} = 0;
    $self->{"framesPerSecond"} = 0;	
    $self->{"repeat"} = 0;
    $self->{"reverse"} = 0;
    $self->{"customWaterMarkId"} = '';
    $self->{"quality"} = -1;

    bless $self, $class;

    return $self;
}

#
# The width of the resulting animated GIF in pixels
#
sub width
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"width"} = shift;
    }
    return $self->{"width"};
}

#
# The height of the resulting animated GIF in pixels
#
sub height
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"height"} = shift;
    }
    return $self->{"height"};
}

#
# The starting position of the video that should be converted into an animated GIF
#
sub start
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"start"} = shift;
    }
    return $self->{"start"};
}

#
# The length in seconds of the video that should be converted into a animated GIF
#
sub duration
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"duration"} = shift;
    }
    return $self->{"duration"};
}

#
# The speed of the animated GIF from 0.2 to 10 times the original speed
#
sub speed
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"speed"} = shift;
    }
    return $self->{"speed"};
}

#
# The number of frames per second that should be captured from the video
#
sub framesPerSecond
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"framesPerSecond"} = shift;
    }
    return $self->{"framesPerSecond"};
}

#
# The number of times to loop the animated GIF. If 0 it will loop forever
#
sub repeat
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"repeat"} = shift;
    }
    return $self->{"repeat"};
}

#
# True if the frames of the animated GIF should be reversed
#
sub reverse
{
    my $self = shift;   
    if (scalar(@_) == 1)
    {
        $self->{"reverse"} = shift;
    }
    return $self->{"reverse"};
}

#
# The custom watermark to add to the animated GIF
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
# The quality of the image where 0 is poor and 100 excellent. The default is -1 which uses the recommended quality
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
    "|".$self->height()."|".$self->width()."|".$self->customId()."|".$self->framesPerSecond()."|".$self->speed()."|".$self->duration()."|".$self->repeat().
    "|".$self->reverse()."|".$self->start()."|".$self->customWaterMarkId()."|".$self->country()."|".$self->quality()."|".$self->exportUrl();
}

sub _getParameters($$$$$)
{
    my ($self, $applicationKey, $sig, $callBackURL, $dataName, $dataValue) = @_;
    
    $params = $self->createParameters($applicationKey, $sig, $callBackURL, $dataName, $dataValue);
    $params->{'width'} = $self->width();
    $params->{'height'} = $self->height();
    $params->{'duration'} = $self->duration();
    $params->{'speed'} = $self->speed();
    $params->{'start'} = $self->start();
    $params->{'fps'} = $self->framesPerSecond();
    $params->{'repeat'} = $self->repeat();
    $params->{'customwatermarkid'} = $self->customWaterMarkId();
    $params->{'reverse'} = $self->reverse();
    $params->{'quality'} = $self->quality();

    return $params;
}
1;