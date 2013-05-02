#!/usr/bin/perl

package GrabzItWaterMark;

sub new
{
    my $class = shift;
    my $self = {
        _identifier => shift,
        _xPosition => shift,
        _yPosition => shift,
        _format    => shift,
    };
    bless $self, $class;
    return $self;
}

sub getIdentifier {
    my ($self) = @_;	
    
    return $self->{_identifier};
}

sub getXPosition {
    my ($self) = @_;	
    
    return $self->{_xPosition};
}

sub getYPosition {
    my ($self) = @_;	
    
    return $self->{_yPosition};
}

sub getFormat {
    my ($self) = @_;	
    
    return $self->{_path};
}
1;