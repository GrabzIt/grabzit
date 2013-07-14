var timeout = null;

function checkForResults() {
	//clear div
	$('#divResults').html('');
	//read result directory and display
	$.getJSON('ajax/results.php?r='+Math.floor((Math.random()*100000)+1), function(data) {
		$.each(data, function(key, val) {
                        if (val.indexOf(".pdf") !== -1)
                        {
			    $('#divResults').append('<a title="Click to open" target="_blank" href="'+val+'"><img class="result" src="css/pdf.png"></img></a>');
                        }
                        else
                        {
			    $('#divResults').append('<img title="Click to zoom in" class="result" onclick="zoom(\''+val+'\')" src="'+val+'"></img>');
                        }
		});
	});
	timeout = setTimeout("checkForResults()", 5000);
}

function zoom(url)
{
	clearTimeout(timeout);
	$('#divResults').html('<img class="zoomedResult" title="Click to zoom out" onclick="zoomout()" src="'+url+'">');
}

function zoomout()
{
	checkForResults();
}

$(document).ready(function() {
        checkForResults();
});