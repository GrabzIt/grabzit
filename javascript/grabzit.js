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