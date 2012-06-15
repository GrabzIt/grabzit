#!/usr/bin/perl

package ScreenShotStatus;

sub new
{
    my $class = shift;
    my $self = {
        _processing => shift,
        _cached => shift,
        _expired      => shift,
        _message      => shift,
    };
    bless $self, $class;
    return $self;
}

sub getProcessing {
    my ($self) = @_;	
    
    return $self->{_processing};
}

sub getCached {
    my ($self) = @_;	
    
    return $self->{_cached};
}

sub getExpired {
    my ($self) = @_;	
    
    return $self->{_expired};
}

sub getMessage {
    my ($self) = @_;	
    
    return $self->{_message};
}
1;