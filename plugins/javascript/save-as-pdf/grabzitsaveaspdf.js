function GrabzIt(key)
{
    return new (function(key)
	{
		this.key = key;

		this.Create = function(url, options)
		{
			var protocol = '//';
			if (window.location.protocol != 'https' && window.location.protocol != 'http')
			{
				protocol = 'http://';
			}

			var grabzItUrl = protocol + 'api.grabz.it/services/javascript.ashx?key='+encodeURIComponent(this.key)+'&url=' + encodeURIComponent(url);

			for(var k in options)
			{
				if (k != 'format' && k != 'cache' && k != 'customWaterMarkId' && k != 'quality'
				&& k != 'country' && k != 'filename' && k != 'errorid' && k != 'errorclass' &&
				k != 'onfinish' && k != 'onerror' && k != 'delay' && k != 'bwidth' && k != 'bheight' &&
				k != 'height' && k != 'width' && k != 'target' && k != 'requestas' && k != 'download' && k != 'suppresserrors' && k != 'displayid' && k != 'displayclass' && k != 'background' && k != 'pagesize' && k != 'orientation' && k != 'includelinks' && k != 'includeoutline' && k != 'title' && k != 'coverurl' && k != 'mtop' && k != 'mleft' && k != 'mbottom' && k != 'mright' && k != 'tabletoinclude' && k != 'includeheadernames' && k != 'includealltables' && k != 'start' && k != 'duration' && k != 'speed' && k != 'fps' && k != 'repeat' && k != 'reverse')
				{
					throw "Option " + k + " not recognized!";
				}

				var v = options[k];
				if (v != null)
                		{
					grabzItUrl += '&' + k + '=' + encodeURIComponent(v);
				}
			}

			var scriptNode = document.createElement('script');
			scriptNode.src = grabzItUrl;

			return scriptNode;
		};

		this.AddTo = function(container, url, options)
		{
			var elem = null;
			if (typeof container == 'string' || container instanceof String)
			{
				elem = document.getElementById(container);
				if (elem == null)
				{
					throw "An element with the id " + container + " was not found";
				}
			}
			else if (container.nodeType === 1)
			{
				elem = container;
			}

			if (elem == null)
			{
				throw "No valid element was provided to attach the screenshot to";
			}

			elem.appendChild(this.Create(url, options));
		}
	})(key);
}

function GrabzItSaveAsPDF(key, options)
{
	this.options = options;
	this.key = key;
	this.pluuginId = null;

	this.clicked = function(oObj)
	{
		if (oObj == null || oObj.className.indexOf("grabzit-pdf-save-progress") !== -1)
		{
			return false;
		}

		oObj.className += " grabzit-pdf-save-progress";

		var id = oObj.getAttribute("grabzit-id");
		var url = null;

		if (oObj.getAttribute("grabzit-url") != null)
		{
			url = oObj.getAttribute("grabzit-url");
		}

		if (url == null)
		{
			url = location.href;
		}

    	var functionName = null;

		if (!id)
		{
			id = Math.floor(Math.random() * (1000000000+1));
			oObj.setAttribute("grabzit-id",id);
			functionName = 'grabzit'+id;

			window[functionName] = function () {
				var links = document.getElementsByClassName("grabzit-pdf-save-progress");

				for (var i = 0; i < links.length; i++)
				{
					var link = links[i];

					if (link.getAttribute("grabzit-id") == id)
					{
						link.className = link.className.replace(/\b grabzit-pdf-save-progress\b/,'');
					}
				}
			}
		}

        functionName = 'grabzit'+id;

		var clonedOptions = JSON.parse(JSON.stringify(options));

		clonedOptions['onfinish'] = functionName;
		clonedOptions['onerror'] = functionName;

		try
		{
			GrabzIt(key).AddTo(document.documentElement, url, clonedOptions);
		}
		catch(e)
		{
			alert(e);
		}

		return false;
	}

	this._getJSFunction = function(jsAttribute)
	{
		if (jsAttribute != null)
		{
			return jsAttribute + ";";
		}
		return "";
	}

	var __construct = function(that)
	{
		that.pluginId = 'grabzItSaveToPDF' + Math.floor(Math.random() * (1000000000+1));

		if (options == null)
		{
			options = {};
			options.filename = document.title + '.pdf';
		}

		if (options != null)
		{
			options.format = 'pdf';
		}

		var links = document.getElementsByClassName("grabzit-pdf-save");

		for (var i = 0; i < links.length; i++)
		{
			var link = links[i];
			var eventVal = '';
			if (link.getAttribute('onclick') != null)
			{
				eventVal = link.getAttribute('onclick');
			}
			link.setAttribute('onclick', that._getJSFunction(eventVal + 'return '+that.pluginId+'.clicked(this)'));
		}

		window[that.pluginId] = that;
	}(this)
}