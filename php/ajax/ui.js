function UI(ajaxUrl, resourceDir){
	this.timeout = null;

	this.checkForResults = function() {
		//clear div
		$('#divResults').html('');
		//read result directory and display
		$.getJSON(ajaxUrl+Math.floor((Math.random()*100000)+1), function(data) {
			$.each(data, function(key, val) {
				if (val)
				{
					val = val.replace(/\\/g,"/");
				}
				if (val.indexOf(".pdf") !== -1)
				{
					$('#divResults').append('<a title="Click to open" target="_blank" href="'+val+'"><img class="result" src="'+resourceDir+'/pdf.png"></img></a>');
				}
				else if (val.indexOf(".docx") !== -1)
				{
					$('#divResults').append('<a title="Click to open" target="_blank" href="'+val+'"><img class="result" src="'+resourceDir+'/docx.png"></img></a>');
				}	
				else if (val.indexOf(".csv") !== -1)
				{
					$('#divResults').append('<a title="Click to open" target="_blank" href="'+val+'"><img class="result" src="'+resourceDir+'/csv.png"></img></a>');
				}				
				else
				{
					$('#divResults').append('<img title="Click to zoom in" class="result" onclick="ui.zoom(\''+val+'\')" src="'+val+'"></img>');
				}
			});
		});
		clearTimeout(this.timeout);
		this.timeout = setTimeout("ui.checkForResults()", 1000);
	}

	this.zoom = function(url)
	{
		clearTimeout(this.timeout);
		$('#divResults').html('<img class="zoomedResult" title="Click to zoom out" onclick="ui.zoomout()" src="'+url+'">');
	}

	this.zoomout = function()
	{
		this.checkForResults();
	}

	this.selectChanged = function(select) {
		if ($(select).val() == 'gif') {
			$('#spnIntro').html('Enter the URL of the online video you want to convert into a animated GIF. The resulting animated GIF');
			$('#divURL').show();
			$('#divHTML').hide();
			$('#divConvert').hide();	
		}
		else if ($(select).val() == 'csv') {
			$('#spnIntro').html('Enter HTML or a URL that contains a HTML table, that you want to convert into a CSV. The resulting document');
			$('#divConvert select').change();
			$('#divConvert').show();	
		}
		else {
			$('#spnIntro').html('Enter the HTML or URL you want to convert into a DOCX, PDF or Image. The resulting capture');
			$('#divConvert select').change();
			$('#divConvert').show();
		}
	}

	this.selectConvertChanged = function(select) {
		if ($(select).val() == 'url') {
			$('#divURL').show();
			$('#divHTML').hide();
		}
		else {
			$('#divHTML').show();
			$('#divURL').hide();
		}
	}

	var that = this;
	$(document).ready(function() {
		that.checkForResults();
	});
}