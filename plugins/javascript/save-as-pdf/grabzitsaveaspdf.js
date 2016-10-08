function GrabzIt(key)
{
	return new (function(key)
	{	
		this.key = key;
		this.data = null;
		this.dataKey = null;
		this.options = null;
		this.post = false;
		this.elem = null;
		
		this.ConvertURL = function(url, options)
		{
			this.data = url;
			this.dataKey = 'url';
			this.options = options;
			this.post = false;
			
			return this;
		};
		
		this.ConvertHTML = function(html, options)
		{
			this.data = html;
			this.dataKey = 'html';
			this.options = options;
			this.post = true;

			return this;			
		};		
		
		this._post = function(qs)
		{
			var xhttp;
			if (window.XMLHttpRequest) 
			{
				xhttp = new XMLHttpRequest();
			}
			else
			{
				xhttp = new ActiveXObject("Microsoft.XMLHTTP");
			}
			
			var that = this;
			
			xhttp.onreadystatechange = function()
			{
				if (this.readyState == 4 && this.status == 200)
				{					  
					that.elem.appendChild(that._handlePost(JSON.parse(this.responseText)));
				}
			};
				
			xhttp.open("POST", this._getBaseWebServiceUrl(), true);
			xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
			xhttp.send(qs);
		};
		
		this._getBaseWebServiceUrl = function()
		{
			var protocol = '//';
			if (window.location.protocol != 'https' && window.location.protocol != 'http')
			{
				protocol = 'http://';
			}

			return protocol + 'api.grabz.it/services/javascript.ashx';			
		}
		
		this._createQueryString = function(sKey, sValue)
		{			
			var qs = 'key='+encodeURIComponent(this.key)+'&'+sKey+'=' + encodeURIComponent(sValue);
		
			if (this.options == null)
			{
				return qs;
			}
		
			for(var k in this.options)
			{
				if (k != 'format' && k != 'cache' && k != 'customWaterMarkId' && k != 'quality'
				&& k != 'country' && k != 'filename' && k != 'errorid' && k != 'errorclass' &&
				k != 'onfinish' && k != 'onerror' && k != 'delay' && k != 'bwidth' && k != 'bheight' &&
				k != 'height' && k != 'width' && k != 'target' && k != 'requestas' && k != 'download' && k != 'suppresserrors' && k != 'displayid' && k != 'displayclass' && k != 'background' && k != 'pagesize' && k != 'orientation' && k != 'includelinks' && k != 'includeoutline' && k != 'title' && k != 'coverurl' && k != 'mtop' && k != 'mleft' && k != 'mbottom' && k != 'mright' && k != 'tabletoinclude' && k != 'includeheadernames' && k != 'includealltables' && k != 'start' && k != 'duration' && k != 'speed' && k != 'fps' && k != 'repeat' && k != 'reverse' && 
				k != 'templateid')
				{
					throw "Option " + k + " not recognized!";
				}
				
				var v = this.options[k];
				if (v != null)
                {
					qs += '&' + k + '=' + encodeURIComponent(v);
				}
			}			
			
			return qs;
		}
		
		this._createScriptNode = function(sUrl)
		{
			var scriptNode = document.createElement('script');
			scriptNode.src = sUrl;

			return scriptNode;
		}
		
		this._handlePost = function(obj)
		{
			if (obj != null)
			{
				if (obj.ID == null || obj.ID == '')
				{
					throw obj.Message;
				}
				return this._createScriptNode(this._getBaseWebServiceUrl() + '?' + this._createQueryString('id', obj.ID));
			}
		}
		
		this.Create = function()
		{
			var defaultNode = document.documentElement;
			if (document.body != null)
			{
				defaultNode = document.body;
			}
			this.AddTo(defaultNode);
		};

		this.AddTo = function(container)
		{			
			if (typeof container == 'string' || container instanceof String)
			{
				this.elem = document.getElementById(container);
				if (this.elem == null)
				{
					throw "An element with the id " + container + " was not found";
				}
			}
			else if (container.nodeType === 1)
			{
				this.elem = container;
			}

			if (this.elem == null)
			{
				throw "No valid element was provided to attach the screenshot to";
			}			
			
			if (this.post)
			{
				this._post(this._createQueryString(this.dataKey, this.data));
				return;
			}
			
			this.elem.appendChild(this._createScriptNode(this._getBaseWebServiceUrl() + '?' + this._createQueryString(this.dataKey, this.data)));
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
			GrabzIt(key).ConvertURL(url, clonedOptions).Create();
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