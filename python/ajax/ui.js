var timeout = null;

function checkForResults() {
	//clear div
	$('#divResults').html('');
	//read result directory and display
	$.getJSON('ajax/results.py?r='+Math.floor((Math.random()*100000)+1), function(data) {
		$.each(data, function(key, val) {
			if (val.indexOf(".pdf") !== -1)
			{
				$('#divResults').append('<a title="Click to open" target="_blank" href="'+val+'"><img class="result" src="css/pdf.png"></img></a>');
			}
			else if (val.indexOf(".docx") !== -1)
			{
				$('#divResults').append('<a title="Click to open" target="_blank" href="'+val+'"><img class="result" src="css/docx.png"></img></a>');
			}	
			else
			{
				$('#divResults').append('<img title="Click to zoom in" class="result" onclick="zoom(\''+val+'\')" src="'+val+'"></img>');
			}
		});
	});
	clearTimeout(timeout);
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

function selectChanged(select) {
    if ($(select).val() == 'gif') {
        $('#spnGif').show();
		$('#divURL').show();
		$('#divHTML').hide();
		$('#divConvert').hide();		
        $('#spnScreenshot').hide();
    }
    else {
		$('#divConvert select').change();
		$('#divConvert').show();
        $('#spnScreenshot').show();
        $('#spnGif').hide();
    }
}

function selectConvertChanged(select) {
    if ($(select).val() == 'url') {
		$('#divURL').show();
		$('#divHTML').hide();
    }
    else {
		$('#divHTML').show();
		$('#divURL').hide();
    }
}

$(document).ready(function() {
	checkForResults();
});