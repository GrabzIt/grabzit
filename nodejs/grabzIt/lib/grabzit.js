var crypto = require('crypto');
var file = require('fs');
var http = require('http');
var querystring = require('querystring');
var path = require('path');

var TRUE = "True";

function GrabzItClient(applicationKey, applicationSecret)
{
    this.ERROR_CODES = {
        SUCCESS: 0,
        PARAMETER_NO_URL: 100,
        PARAMETER_INVALID_URL: 101,
        PARAMETER_NON_EXISTANT_URL: 102,
        PARAMETER_MISSING_APPLICATION_KEY: 103,
        PARAMETER_UNRECOGNISED_APPLICATION_KEY: 104,
        PARAMETER_MISSING_SIGNATURE: 105,
        PARAMETER_INVALID_SIGNATURE: 106,
        PARAMETER_INVALID_FORMAT: 107,
        PARAMETER_INVALID_COUNTRY_CODE: 108,
        PARAMETER_DUPLICATE_IDENTIFIER: 109,
        PARAMETER_MATCHING_RECORD_NOT_FOUND: 110,
        PARAMETER_INVALID_CALLBACK_URL: 111,
        PARAMETER_NON_EXISTANT_CALLBACK_URL: 112,
        PARAMETER_IMAGE_WIDTH_TOO_LARGE: 113,
        PARAMETER_IMAGE_HEIGHT_TOO_LARGE: 114,
        PARAMETER_BROWSER_WIDTH_TOO_LARGE: 115,
        PARAMETER_BROWSER_HEIGHT_TOO_LARGE: 116,
        PARAMETER_DELAY_TOO_LARGE: 117,
        PARAMETER_INVALID_BACKGROUND: 118,
        PARAMETER_INVALID_INCLUDE_LINKS: 119,
        PARAMETER_INVALID_INCLUDE_OUTLINE: 120,
        PARAMETER_INVALID_PAGE_SIZE: 121,
        PARAMETER_INVALID_PAGE_ORIENTATION: 122,
        PARAMETER_VERTICAL_MARGIN_TOO_LARGE: 123,
        PARAMETER_HORIZONTAL_MARGIN_TOO_LARGE: 124,
        PARAMETER_INVALID_COVER_URL: 125,
        PARAMETER_NON_EXISTANT_COVER_URL: 126,
        PARAMETER_MISSING_COOKIE_NAME: 127,
        PARAMETER_MISSING_COOKIE_DOMAIN: 128,
        PARAMETER_INVALID_COOKIE_NAME: 129,
        PARAMETER_INVALID_COOKIE_DOMAIN: 130,
        PARAMETER_INVALID_COOKIE_DELETE: 131,
        PARAMETER_INVALID_COOKIE_HTTP: 132,
        PARAMETER_INVALID_COOKIE_EXPIRY: 133,
        PARAMETER_INVALID_CACHE_VALUE: 134,
        PARAMETER_INVALID_DOWNLOAD_VALUE: 135,
        PARAMETER_INVALID_SUPPRESS_VALUE: 136,
        PARAMETER_MISSING_WATERMARK_IDENTIFIER: 137,
        PARAMETER_INVALID_WATERMARK_IDENTIFIER: 138,
        PARAMETER_INVALID_WATERMARK_XPOS: 139,
        PARAMETER_INVALID_WATERMARK_YPOS: 140,
        PARAMETER_MISSING_WATERMARK_FORMAT: 141,
        PARAMETER_WATERMARK_TOO_LARGE: 142,
        PARAMETER_MISSING_PARAMETERS: 143,
        PARAMETER_QUALITY_TOO_LARGE: 144,
        PARAMETER_QUALITY_TOO_SMALL: 145,
        PARAMETER_REPEAT_TOO_SMALL: 149,
        PARAMETER_INVALID_REVERSE: 150,
        PARAMETER_FPS_TOO_LARGE: 151,
        PARAMETER_FPS_TOO_SMALL: 152,
        PARAMETER_SPEED_TOO_FAST: 153,
        PARAMETER_SPEED_TOO_SLOW: 154,
        PARAMETER_INVALID_ANIMATION_COMBINATION: 155,
        PARAMETER_START_TOO_SMALL: 156,
        PARAMETER_DURATION_TOO_SMALL: 157,
        PARAMETER_NO_HTML: 163,
        PARAMETER_INVALID_TARGET_VALUE: 165,
        PARAMETER_INVALID_HIDE_VALUE: 166,
        PARAMETER_INVALID_INCLUDE_IMAGES: 167,
        NETWORK_SERVER_OFFLINE: 200,
        NETWORK_GENERAL_ERROR: 201,
        NETWORK_DDOS_ATTACK: 202,
        RENDERING_ERROR: 300,
        RENDERING_MISSING_SCREENSHOT: 301,
        GENERIC_ERROR: 400,
        UPGRADE_REQUIRED: 500,
        FILE_SAVE_ERROR: 600,
        FILE_NON_EXISTANT_PATH: 601
        };

        this.request = null;
        this.applicationKey = applicationKey;
        this.applicationSecret = applicationSecret;
}

function _extend() {
    for (var i = 1; i < arguments.length; i++)
        for (var key in arguments[i])
            if (arguments[i].hasOwnProperty(key))
                arguments[0][key] = arguments[i][key];
    return arguments[0];
}

function _convert(data, type){
    var result = JSON.parse(data);

    if (type == 'status'){
        //is status object
        var obj = new Object();
        obj.processing = result.Processing == TRUE;
        obj.cached = result.Cached == TRUE;
        obj.expired = result.Expired == TRUE;
        obj.message = result.Message;

        return obj;
    }
    if (type == 'screenshot'){
        return result.ID;
    }
    if (type == 'result'){
        return result.Result == TRUE;
    }
    if (type == 'cookies'){
        var cookies = new Array();
        if (result != null && result.Cookies != null){
            for (var i in result.Cookies) {
                var cookie = result.Cookies[i];
                var obj = new Object();
                obj.name = cookie.Name;
                obj.value = cookie.Value;
                obj.domain = cookie.Domain;
                obj.path = cookie.Path;
                obj.httponly = (cookie.HttpOnly == TRUE);
                obj.expires = cookie.Expires;
                obj.type = cookie.Type;
                cookies.push(obj);
            }
        }
        return cookies;
    }
    if (type == 'watermarks'){
        var watermarks = new Array();
        if (result != null && result.WaterMarks != null){
            for (var i in result.WaterMarks) {
                var watermark = result.WaterMarks[i];
                var obj = new Object();
                obj.identifier = watermark.Identifier;
                obj.xPosition = watermark.XPosition;
                obj.yPosition = watermark.YPosition;
                obj.format = watermark.Format;
                watermarks.push(obj);
            }
        }
        return watermarks;
    }
    return result;
}


function _createSignature(value){
    var md5 = crypto.createHash('md5');
    //replace non-ascii with question mark
    md5.update(value.replace(/[^\x00-\x7F]/g, "?"));
    return md5.digest('hex');
}

function _toInt(value){
    if (value){
        return 1;
    }
    return 0;
}

function _getWaterMarks(applicationKey, applicationSecret, identifier, oncomplete){
    var sig = _createSignature(applicationSecret + '|' + identifier);

    var params = {
        'key': applicationKey,
        'identifier': identifier,
        'sig': sig
    };

    _get(this, 'getwatermarks.ashx?' + querystring.stringify(params), 'watermarks', oncomplete);
}

function _getFormDataForPost(fields, files) {
        function encodeFieldPart(boundary,name,value) {
            var return_part = "--" + boundary + "\r\n";
            return_part += "Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n";
            return_part += value + "\r\n";
            return return_part;
        }

        function encodeFilePart(boundary,type,name,filename) {
            var return_part = "--" + boundary + "\r\n";
            return_part += "Content-Disposition: form-data; name=\"" + name + "\"; filename=\"" + filename + "\"\r\n";
            return_part += "Content-Type: " + type + "\r\n\r\n";
            return return_part;
        }

        var boundary = Math.random();
        var post_data = [];

        if (fields) {
            for (var key in fields) {
                var value = fields[key];
                post_data.push(new Buffer(encodeFieldPart(boundary, key, value), 'utf-8'));
            }
        }

        if (files) {
            for (var key in files) {
                var value = files[key];
                post_data.push(new Buffer(encodeFilePart(boundary, value.type, value.keyname, value.valuename), 'utf-8'));
                post_data.push(new Buffer(value.data, 'utf-8'));
        }
    }

    post_data.push(new Buffer("\r\n--" + boundary + "--\r\n"),  'utf-8');;

    var length = 0;

    for(var i = 0; i < post_data.length; i++) {
        length += post_data[i].length;
    }

    var params = {
        postdata : post_data,
        headers : {
            'Accept': 'application/json',
            'Content-Type': 'multipart/form-data; boundary=' + boundary,
            'Content-Length': length
        }
    };

    return params;
}

function _post(self, url, params, type, oncomplete){
    var options = {
        host: 'grabz.it',
        port: 80,
        path: '/services/'+url,
        method: 'POST',
        headers: { 
          'Accept': 'application/json',
          'Content-Type': 'application/x-www-form-urlencoded',
          'Content-Length': Buffer.byteLength(params)
        }
    }

    var postRequest = _http(self, options, type, oncomplete);
    postRequest.write(params);
    postRequest.end();
}

function _get(self, url, type, oncomplete){
    var options = {
        host: 'api.grabz.it',
        port: 80,
        path: '/services/'+url,
        headers: { 'Accept': 'application/json' }
    }

    _http(self, options, type, oncomplete).end();
}

function _http(self, options, type, oncomplete){
    var data = '';

    var request = http.request(options, function (res) {
        if (type == 'binary') {
            res.setEncoding('binary');
        }

        res.on('data', function (chunk) {
            data += chunk;
        });

        res.on('end', function () {
            if (type != 'binary') {
                if (oncomplete != null) {
                    var error = _getError(self, res, data);
                    var obj = _convert(data, type);
                    if (error != null) {
                        obj = null;
                    }
                    oncomplete(error, obj);
                }
            } else if (oncomplete != null) {
                oncomplete(null, data);
            }
        });
    });

    return request;    
}

function _getError(self, res, data){
    if (res.statusCode == 403) {
        var error = new Error();
        error.message = 'Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.';
        error.code = self.ERROR_CODES.NETWORK_DDOS_ATTACK;

        return error;
    }

    if (res.statusCode >= 400) {
        var error = new Error();
        error.message = 'A network error occured when connecting to the GrabzIt servers.';
        error.code = self.ERROR_CODES.NETWORK_GENERAL_ERROR;

        return error;
    }

    var result = JSON.parse(data);

    if (result.Code != "0") {
        var error = new Error();
        error.message = result.Message;
        error.code = parseInt(result.Code);

        return error;
    }

    return null;
}

function Request(sUrl, oParameters, sSignatureOne, sSignatureTwo, bIsPost, iStartDelay, sTargetUrl) {
    this.url = sUrl;
    this.parameters = oParameters;
    this.signatureOne = sSignatureOne;
    this.signatureTwo = sSignatureTwo;
    this.isPost = bIsPost;
    this.targetUrl = sTargetUrl;
    this.startDelay = iStartDelay;

    this.createSignatureString = function (callBackUrl) {
        var sig = this.signatureOne;
        if (callBackUrl == null) {
            callBackUrl = '';
        }
        sig += callBackUrl;
        sig += this.signatureTwo;
        return sig;
    };

    this.getParameters = function (sCallback, sSig) {
        this.parameters.callback = sCallback;
        this.parameters.sig = sSig;
        return querystring.stringify(this.parameters);
    }
}

function _getDOCXRequestObject(applicationKey, applicationSecret, url, options, isPost, target) {
    var defaults = {
        'customId': '',
        'includeBackground': true,
        'pagesize': 'A4',
        'orientation': 'Portrait',
        'includeLinks': true,
        'includeImages': true,
        'title': '',
        'marginTop': 10,
        'marginLeft': 10,
        'marginBottom': 10,
        'marginRight': 10,
        'delay': '',
        'requestAs': 0,
        'quality': -1,
        'country': '',
        'hideElement': ''
    };

    context = _extend(defaults, options);

    var pagesize = context['pagesize'];
    if (pagesize != null) {
        pagesize = pagesize.toUpperCase();
    }
    var orientation = context['orientation'];
    if (orientation != null && orientation.length > 2) {
        var first = orientation[0];
        var rest = orientation.substr(1);
        orientation = first.toUpperCase() + rest.toLowerCase();
    }

    var startDelay = 0;

    if (context['delay'] != '') {
        startDelay = context['delay']
    }

    var requestParams = {
        'key': applicationKey,
        'background': _toInt(context['includeBackground']),
        'pagesize': pagesize,
        'orientation': orientation,
        'customid': context['customId'],
        'delay': context['delay'],
        'hide': context['hideElement'],
        'includelinks': _toInt(context['includeLinks']),
        'includeimages': _toInt(context['includeImages']),
        'title': context['title'],
        'mleft': parseInt(context['marginLeft']),
        'mright': parseInt(context['marginRight']),
        'mtop': parseInt(context['marginTop']),
        'mbottom': parseInt(context['marginBottom']),
        'delay': context['delay'],
        'requestmobileversion': parseInt(context['requestAs']),
        'quality': parseInt(context['quality']),
        'country': context['country']
    };

    requestParams = _addTargetToRequest(requestParams, isPost, target);

    var signaturePartTwo = '|' + context['customId'] + '|' + _toInt(context['includeBackground']) + '|' + pagesize + '|' + orientation + '|' + _toInt(context['includeImages'])
     + '|' + _toInt(context['includeLinks']) + '|' + context['title'] + '|' + parseInt(context['marginTop'])
     + '|' + parseInt(context['marginLeft']) + '|' + parseInt(context['marginBottom']) + '|' + parseInt(context['marginRight']) + '|' + context['delay']
     + '|' + parseInt(context['requestAs']) + '|' + context['country'] + '|' + parseInt(context['quality']) + '|' + context['hideElement'];

    return new Request(url, requestParams, _createFirstSignature(applicationSecret, target, isPost), signaturePartTwo, isPost, startDelay, target);
}

function _getPDFRequestObject(applicationKey, applicationSecret, url, options, isPost, target) {
    var defaults = {
        'customId': '',
        'includeBackground': true,
        'pagesize': 'A4',
        'orientation': 'Portrait',
        'includeLinks': true,
        'includeOutline': false,
        'title': '',
        'coverUrl': '',
        'marginTop': 10,
        'marginLeft': 10,
        'marginBottom': 10,
        'marginRight': 10,
        'delay': '',
        'requestAs': 0,
        'templateId': '',
        'customWaterMarkId': '',
        'quality': -1,
        'country': '',
        'hideElement': ''
    };

    context = _extend(defaults, options);

    var pagesize = context['pagesize'];
    if (pagesize != null) {
        pagesize = pagesize.toUpperCase();
    }
    var orientation = context['orientation'];
    if (orientation != null && orientation.length > 2) {
        var first = orientation[0];
        var rest = orientation.substr(1);
        orientation = first.toUpperCase() + rest.toLowerCase();
    }

    var startDelay = 0;

    if (context['delay'] != '') {
        startDelay = context['delay']
    }

    var requestParams = {
        'key': applicationKey,
        'background': _toInt(context['includeBackground']),
        'pagesize': pagesize,
        'orientation': orientation,
        'customid': context['customId'],
        'templateid': context['templateId'],
        'customwatermarkid': context['customWaterMarkId'],
        'delay': context['delay'],
        'hide': context['hideElement'],
        'includelinks': _toInt(context['includeLinks']),
        'includeoutline': _toInt(context['includeOutline']),
        'title': context['title'],
        'coverurl': context['coverUrl'],
        'mleft': parseInt(context['marginLeft']),
        'mright': parseInt(context['marginRight']),
        'mtop': parseInt(context['marginTop']),
        'mbottom': parseInt(context['marginBottom']),
        'delay': context['delay'],
        'requestmobileversion': parseInt(context['requestAs']),
        'quality': parseInt(context['quality']),
        'country': context['country']
    };

    requestParams = _addTargetToRequest(requestParams, isPost, target);

    var signaturePartTwo = '|' + context['customId'] + '|' + _toInt(context['includeBackground']) + '|' + pagesize + '|' + orientation + '|' + context['customWaterMarkId']
     + '|' + _toInt(context['includeLinks']) + '|' + _toInt(context['includeOutline']) + '|' + context['title'] + '|' + context['coverUrl'] + '|' + parseInt(context['marginTop'])
     + '|' + parseInt(context['marginLeft']) + '|' + parseInt(context['marginBottom']) + '|' + parseInt(context['marginRight']) + '|' + context['delay']
     + '|' + parseInt(context['requestAs']) + '|' + context['country'] + '|' + parseInt(context['quality']) + '|' + context['templateId'] + '|' + context['hideElement'];

    return new Request(url, requestParams, _createFirstSignature(applicationSecret, target, isPost), signaturePartTwo, isPost, startDelay, target);
}

function _getTableRequestObject(applicationKey, applicationSecret, url, options, isPost, target) {
    var defaults = {
        'customId': '',
        'tableNumberToInclude': 1,
        'format': 'csv',
        'includeHeaderNames': true,
        'includeAllTables': false,
        'targetElement': '',
        'requestAs': 0,
        'country': ''
    };

    context = _extend(defaults, options);

    this.startDelay = 0;

    this.requestParams = {
        'key': applicationKey,
        'includeAllTables': _toInt(context['includeAllTables']),
        'includeHeaderNames': _toInt(context['includeHeaderNames']),
        'format': context['format'],
        'tableToInclude': parseInt(context['tableNumberToInclude']),
        'customid': context['customId'],
        'target': context['targetElement'],
        'requestmobileversion': parseInt(context['requestAs']),
        'country': context['country']
    };

    requestParams = _addTargetToRequest(requestParams, isPost, target);

    this.signaturePartOne = applicationSecret + '|' + url + '|';
    this.signaturePartTwo = '|' + context['customId'] + '|' + parseInt(context['tableNumberToInclude']) + '|' + _toInt(context['includeAllTables']) + '|' + _toInt(context['includeHeaderNames']) + '|' + context['targetElement']
     + '|' + context['format'] + '|' + parseInt(context['requestAs']) + '|' + context['country'];

    return new Request(url, requestParams, _createFirstSignature(applicationSecret, target, isPost), signaturePartTwo, isPost, startDelay, target);
}

function _getAnimationRequestObject(applicationKey, applicationSecret, url, options, isPost, target) {
    var defaults = {
        'customId': '',
        'width': 0,
        'height': 0,
        'start': 0,
        'duration': 0,
        'speed': 0,
        'framesPerSecond': 0,
        'repeat': 0,
        'reverse': false,
        'customWaterMarkId': '',
        'quality': -1,
        'country': ''
    };

    context = _extend(defaults, options);

    var startDelay = 0;

    this.requestParams = {
        'key': applicationKey,
        'width': parseInt(context['width']),
        'height': parseInt(context['height']),
        'duration': parseInt(context['duration']),
        'speed': parseFloat(context['speed']),
        'start': parseInt(context['start']),
        'customid': context['customId'],
        'fps': parseFloat(context['framesPerSecond']),
        'repeat': parseInt(context['repeat']),
        'customwatermarkid': context['customWaterMarkId'],
        'reverse': _toInt(context['reverse']),
        'country': context['country'],
        'quality': parseInt(context['quality'])
    };

    requestParams = _addTargetToRequest(requestParams, isPost, target);

    var signaturePartTwo = '|' + parseInt(context['height']) + '|' + parseInt(context['width']) + '|' + context['customId'] + '|' + parseFloat(context['framesPerSecond']) + '|' +parseFloat(context['speed'])
     + '|' + parseInt(context['duration']) + '|' + parseInt(context['repeat']) + '|' + _toInt(context['reverse']) + '|' + parseInt(context['start']) + '|' + context['customWaterMarkId'] + '|' + context['country'] + '|' + parseInt(context['quality']);

    return new Request(url, requestParams, _createFirstSignature(applicationSecret, target, isPost), signaturePartTwo, isPost, startDelay, target);
}

function _getImageRequestObject(applicationKey, applicationSecret, url, options, isPost, target) {
    var defaults = {
        'customId': '',
        'browserWidth': '',
        'browserHeight': '',
        'width': '',
        'height': '',
        'format': '',
        'delay': '',
        'targetElement': '',
        'hideElement': '',
        'requestAs': 0,
        'customWaterMarkId': '',
        'quality': -1,
        'country': ''
    };

    context = _extend(defaults, options);

    var startDelay = 0;

    if (context['delay'] != '') {
        startDelay = context['delay']
    }

    this.requestParams = {
        'key': applicationKey,
        'width': context['width'],
        'height': context['height'],
        'format': context['format'],
        'bwidth': context['browserWidth'],
        'bheight': context['browserHeight'],
        'customid': context['customid'],
        'delay': context['delay'],
        'target': context['targetElement'],
        'hide': context['hideElement'],
        'customwatermarkid': context['customWaterMarkId'],
        'requestmobileversion': parseInt(context['requestAs']),
        'country': context['country'],
        'quality': parseInt(context['quality'])
    };

    requestParams = _addTargetToRequest(requestParams, isPost, target);

    var signaturePartTwo = '|' + context['format'] + '|' + context['height'] + '|' + context['width'] + '|' + context['browserHeight'] + '|' + context['browserWidth']
     + '|' + context['customId'] + '|' + context['delay'] + '|' + context['targetElement'] + '|' + context['customWaterMarkId'] + '|' + _toInt(context['requestAs'])
     + '|' + context['country'] + '|' + context['quality'] + '|' + context['hideElement'];

    return new Request(url, requestParams, _createFirstSignature(applicationSecret, target, isPost), signaturePartTwo, isPost, startDelay, target);
}

function _addTargetToRequest(requestParams, isPost, target){
    if (!isPost)
    {
        requestParams.url = target;
    }
    else
    {
        requestParams.html = target;
    }

    return requestParams;    
}

function _createFirstSignature(applicationSecret, target, isPost){
    var signaturePartOne = applicationSecret + '|';

    if (!isPost)
    {
        signaturePartOne += target + '|';
    }

    return signaturePartOne;
}

function _readHTMLFile(filePath)
{
    if (!file.existsSync(filePath)) {
        var error = new Error();
        error.message = 'File: ' + filePath + ' does not exist';
        error.code = self.FILE_NON_EXISTANT_PATH;
        throw error;        
    };
    return file.readFileSync(filePath);
}

/*
* This method specifies the URL that should be converted into a PDF.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.url_to_pdf = function (url, options) {
    this.request = _getPDFRequestObject(this.applicationKey, this.applicationSecret, 'takepdf.ashx', options, false, url);
};

/*
* This method specifies the HTML that should be converted into a PDF.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.html_to_pdf = function (html, options) {
    this.request = _getPDFRequestObject(this.applicationKey, this.applicationSecret, 'takepdf.ashx', options, true, html);
};

/*
* This method specifies a HTML file that should be converted into a PDF.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.file_to_pdf = function (path, options) {
    this.html_to_pdf(_readHTMLFile(path), options);
};

/*
* This method specifies the URL that should be converted into a DOCX document.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.url_to_docx = function (url, options) {
    this.request = _getDOCXRequestObject(this.applicationKey, this.applicationSecret, 'takedocx.ashx', options, false, url);
};

/*
* This method specifies the HTML that should be converted into a DOCX document.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.html_to_docx = function (html, options) {
    this.request = _getDOCXRequestObject(this.applicationKey, this.applicationSecret, 'takedocx.ashx', options, true, html);
};

/*
* This method specifies a HTML file that should be converted into a DOCX document.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.file_to_docx = function (path, options) {
    this.html_to_docx(_readHTMLFile(path), options);
};

/*
* This method specifies the URL that the HTML tables should be extracted from.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.url_to_table = function (url, options) {
     this.request = _getTableRequestObject(this.applicationKey, this.applicationSecret, 'taketable.ashx', options, false, url);
};

/*
* This method specifies the HTML that the HTML tables should be extracted from.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.html_to_table = function (html, options) {
    this.request = _getTableRequestObject(this.applicationKey, this.applicationSecret, 'taketable.ashx', options, true, html);
};

/*
* This method specifies a HTML file that the HTML tables should be extracted from.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.file_to_table = function (path, options) {
    this.html_to_table(_readHTMLFile(path), options);
};

/*
* This method specifies the URL of the online video that should be converted into a animated GIF.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.url_to_animation = function (url, options) {
    this.request = _getAnimationRequestObject(this.applicationKey, this.applicationSecret, 'takeanimation.ashx', options, false, url);
};

/*
* This method specifies the URL that should be converted into a image screenshot.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.url_to_image = function (url, options) {
    this.request = _getImageRequestObject(this.applicationKey, this.applicationSecret, 'takepicture.ashx', options, false, url);
};

/*
* This method specifies the HTML that should be converted into a image.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.html_to_image = function (html, options) {
    this.request = _getImageRequestObject(this.applicationKey, this.applicationSecret, 'takepicture.ashx', options, true, html);
};

/*
* This method specifies a HTML file that should be converted into a image.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx
*/
GrabzItClient.prototype.file_to_image = function (path, options) {
    this.html_to_image(_readHTMLFile(path), options);
};

/*
* Calls the GrabzIt web service to take the screenshot.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#save
*/
GrabzItClient.prototype.save = function (callBackUrl, oncomplete) {
    if (this.request == null) {
        if (oncomplete != null) {
            var error = new Error();
            error.message = 'No parameters have been set.';
            error.code = this.ERROR_CODES.PARAMETER_MISSING_PARAMETERS;
            oncomplete(error, null);
        }
        return;
    }

    var sig = _createSignature(this.request.createSignatureString(callBackUrl));

    if (this.request.isPost) {
        _post(this, this.request.url, this.request.getParameters(callBackUrl, sig), 'screenshot', oncomplete)
    }
    else {
        _get(this, this.request.url + '?' + this.request.getParameters(callBackUrl, sig), 'screenshot', oncomplete);        
    }
};

/*
* Calls the GrabzIt web service to take the screenshot and saves it to the target path provided. If no target path is provided it returns the screenshot byte data.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#saveto
*/
GrabzItClient.prototype.save_to = function (saveToFile, oncomplete) {
    if (saveToFile == null) {
        saveToFile = '';
    }

    var oncompleteEvent = oncomplete;
    var self = this;

    this.save(null, function (err, id) {
        if (err != null) {
            if (oncompleteEvent != null) {
                oncompleteEvent(err, null);
            }
            return;
        }

        setTimeout(function () {
            var intervalId = setInterval(function () {
                self.get_status(id, function (err, status) {
                    if (err != null) {
                        clearInterval(intervalId);

                        if (oncompleteEvent != null) {
                            oncompleteEvent(err, null);
                        }
                        return;
                    }

                    if (!status.cached && !status.processing) {
                        clearInterval(intervalId);

                        if (oncompleteEvent != null) {
                            var error = new Error();
                            error.message = 'The capture did not complete with the error: ' + status.message;
                            error.code = self.ERROR_CODES.RENDERING_ERROR;

                            oncompleteEvent(error, null);
                        }
                    }
                    else if (status.cached) {
                        clearInterval(intervalId);

                        self.get_result(id, function (err, result) {
                            if (result == null || result == '') {
                                if (oncompleteEvent != null) {
                                    var error = new Error();
                                    error.message = 'The capture could not be found on GrabzIt.';
                                    error.code = self.ERROR_CODES.RENDERING_MISSING_SCREENSHOT;

                                    oncompleteEvent(error, null);

                                    return;
                                }
                            }

                            if (saveToFile == '') {
                                if (oncompleteEvent != null) {
                                    oncompleteEvent(null, result)
                                }
                            }

                            file.writeFile(saveToFile, result, 'binary', function (err) {
                                if (oncompleteEvent != null) {
                                    oncompleteEvent(err, null)
                                }
                            });
                        });
                    }
                });
            }, 3000);
        }, (3000 + self.request.startDelay));
    });
};

/*
* This method returns the screenshot itself. If nothing is returned then something has gone wrong or the screenshot is not ready yet.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#getresult
*/
GrabzItClient.prototype.get_result = function (id, oncomplete) {
    if (id == '' || id == null) {
        if (oncomplete != null) {
            oncomplete(null, null);
        }
        return;
    }
    _get(this, 'getfile.ashx?id=' + id, 'binary', oncomplete)
};

/*
* Get the current status of a GrabzIt screenshot.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#getstatus
*/
GrabzItClient.prototype.get_status = function (id, oncomplete) {
    if (id == '' || id == null) {
        if (oncomplete != null) {
            oncomplete(null, null);
        }
        return;
    }
    _get(this, 'getstatus.ashx?id=' + id, 'status', oncomplete);
};

/*
* Get all the cookies that GrabzIt is using for a particular domain. This may include your user set cookies as well.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#getcookies
*/
GrabzItClient.prototype.get_cookies = function (domain, oncomplete) {
    var sig = _createSignature(this.applicationSecret+'|'+domain);

    var params = {
        'key':this.applicationKey,
        'domain':domain,
        'sig':sig
    };

    _get(this, 'getcookies.ashx?' + querystring.stringify(params), 'cookies', oncomplete)
};

/*
* Sets a new custom cookie on GrabzIt, if the custom cookie has the same name and domain as a global cookie the global cookie is overridden.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#setcookie
*/
GrabzItClient.prototype.set_cookie = function (name, domain, options, oncomplete) {

    var defaults = {
        'value': '',
        'path': '/',
        'httponly': false,
        'expires': ''
    };

    context = _extend(defaults, options);

    var sig = _createSignature(this.applicationSecret + '|' + name + '|' + domain + '|' + context['value'] + '|' + context['path'] + '|' + _toInt(context['httponly']) + '|' + context['expires'] + '|0');

    var params = {
        'key': this.applicationKey,
        'name': name,
        'domain': domain,
        'value': context['value'],
        'path': context['path'],
        'httponly': _toInt(context['httponly']),
        'expires': context['expires'],
        'sig': sig
    };

    _get(this, 'setcookie.ashx?' + querystring.stringify(params), 'result', oncomplete);
};

/*
* Delete a custom cookie or block a global cookie from being used.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#deletecookie
*/
GrabzItClient.prototype.delete_cookie = function (name, domain, oncomplete) {

    var sig = _createSignature(this.applicationSecret + '|' + name + '|' + domain + '|1');

    var params = {
        'key': this.applicationKey,
        'name': name,
        'domain': domain,
        'delete': 1,
        'sig': sig
    };

    _get(this, 'setcookie.ashx?' + querystring.stringify(params), 'result', oncomplete);
};

/*
* Add a new custom watermark.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#addwatermark
*/
GrabzItClient.prototype.add_watermark = function (identifier, filePath, xpos, ypos, oncomplete) {

    var self = this;

    file.exists(filePath, function (exists) {
        if (!exists) {
            if (oncomplete != null) {
                var error = new Error();
                error.message = 'File: ' + filePath + ' does not exist';
                error.code = self.FILE_NON_EXISTANT_PATH;

                oncomplete(error, null);
            }
            return;
        }

        var sig = _createSignature(self.applicationSecret + '|' + identifier + '|' + parseInt(xpos) + '|' + parseInt(ypos));

        var files = new Array();

        var watermark = new Object();
        watermark.keyname = 'watermark';
        watermark.valuename = path.basename(filePath);
        watermark.type = 'image/jpeg';
        watermark.data = file.readFileSync(filePath);
        files.push(watermark);

        var fields = {
            'key': self.applicationKey,
            'identifier': identifier,
            'xpos': xpos,
            'ypos': ypos,
            'sig': sig
        };

        var headerparams = _getFormDataForPost(fields, files);

        var post_options = {
            host: 'grabz.it',
            method: 'POST',
            port: 80,
            path: '/services/addwatermark.ashx',
            headers: headerparams.headers
        };

        var request = http.request(post_options, function (response) {
            var data = '';

            response.setEncoding('utf-8');

            response.on('data', function (chunk) {
                data += chunk;
            });

            response.on('end', function () {
                if (oncomplete != null) {
                    var error = _getError(self, res, data);
                    var obj = _convert(data, 'result');
                    if (error != null) {
                        obj = null;
                    }

                    oncomplete(error, obj);
                }
            });
        });

        for (var i = 0; i < headerparams.postdata.length; i++) {
            request.write(headerparams.postdata[i]);
        }
        request.end();
    });
};

/*
* Delete a custom watermark.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#deletewatermark
*/
GrabzItClient.prototype.delete_watermark = function (identifier, oncomplete) {
    var sig = _createSignature(this.applicationSecret + '|' + identifier);

    var params = {
        'key': this.applicationKey,
        'identifier': identifier,
        'sig': sig
    };

    _get(this, 'deletewatermark.ashx?' + querystring.stringify(params), 'result', oncomplete);
};

/*
* Get your uploaded custom watermarks.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#getwatermarks
*/
GrabzItClient.prototype.get_watermarks = function (oncomplete) {
    _getWaterMarks(this.applicationKey, this.applicationSecret, '', oncomplete);
}

/*
* Get your uploaded custom watermark.
*
* For more detailed documentation please visit: http://grabz.it/api/nodejs/grabzitclient.aspx#getwatermark
*/
GrabzItClient.prototype.get_watermark = function (identifier, oncomplete) {
    _getWaterMarks(this.applicationKey, this.applicationSecret, identifier, function (err, watermarks) {
        if (err != null) {
            if (oncomplete != null) {
                oncomplete(err, null);
            }
            return;
        }
        if (watermarks != null && watermarks.length == 1) {
            if (oncomplete != null) {
                oncomplete(null, watermarks[0]);
            }
            return;
        }

        if (oncomplete != null) {
            oncomplete(null, null);
        }
    });
}

module.exports = GrabzItClient;