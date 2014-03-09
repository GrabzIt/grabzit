var crypto = require('crypto');
var file = require('fs');
var http = require('http');
var querystring = require('querystring');
var path = require('path'); 

var TRUE = "True";   

function GrabzItClient(applicationKey, applicationSecret)
{
        this.SUCCESS = 0;
        this.PARAMETER_NO_URL = 100;
        this.PARAMETER_INVALID_URL = 101;
        this.PARAMETER_NON_EXISTANT_URL = 102;
        this.PARAMETER_MISSING_APPLICATION_KEY = 103;
        this.PARAMETER_UNRECOGNISED_APPLICATION_KEY = 104;
        this.PARAMETER_MISSING_SIGNATURE = 105;
        this.PARAMETER_INVALID_SIGNATURE = 106;
        this.PARAMETER_INVALID_FORMAT = 107;
        this.PARAMETER_INVALID_COUNTRY_CODE = 108;
        this.PARAMETER_DUPLICATE_IDENTIFIER = 109;
        this.PARAMETER_MATCHING_RECORD_NOT_FOUND = 110;
        this.PARAMETER_INVALID_CALLBACK_URL = 111;
        this.PARAMETER_NON_EXISTANT_CALLBACK_URL = 112;
        this.PARAMETER_IMAGE_WIDTH_TOO_LARGE = 113;
        this.PARAMETER_IMAGE_HEIGHT_TOO_LARGE = 114;
        this.PARAMETER_BROWSER_WIDTH_TOO_LARGE = 115;
        this.PARAMETER_BROWSER_HEIGHT_TOO_LARGE = 116;
        this.PARAMETER_DELAY_TOO_LARGE = 117;
        this.PARAMETER_INVALID_BACKGROUND = 118;
        this.PARAMETER_INVALID_INCLUDE_LINKS = 119;
        this.PARAMETER_INVALID_INCLUDE_OUTLINE = 120;
        this.PARAMETER_INVALID_PAGE_SIZE = 121;
        this.PARAMETER_INVALID_PAGE_ORIENTATION = 122;
        this.PARAMETER_VERTICAL_MARGIN_TOO_LARGE = 123;
        this.PARAMETER_HORIZONTAL_MARGIN_TOO_LARGE = 124;
        this.PARAMETER_INVALID_COVER_URL = 125;
        this.PARAMETER_NON_EXISTANT_COVER_URL = 126;
        this.PARAMETER_MISSING_COOKIE_NAME = 127;
        this.PARAMETER_MISSING_COOKIE_DOMAIN = 128;
        this.PARAMETER_INVALID_COOKIE_NAME = 129;
        this.PARAMETER_INVALID_COOKIE_DOMAIN = 130;
        this.PARAMETER_INVALID_COOKIE_DELETE = 131;
        this.PARAMETER_INVALID_COOKIE_HTTP = 132;
        this.PARAMETER_INVALID_COOKIE_EXPIRY = 133;
        this.PARAMETER_INVALID_CACHE_VALUE = 134;
        this.PARAMETER_INVALID_DOWNLOAD_VALUE = 135;
        this.PARAMETER_INVALID_SUPPRESS_VALUE = 136;
        this.PARAMETER_MISSING_WATERMARK_IDENTIFIER = 137;
        this.PARAMETER_INVALID_WATERMARK_IDENTIFIER = 138;
        this.PARAMETER_INVALID_WATERMARK_XPOS = 139;
        this.PARAMETER_INVALID_WATERMARK_YPOS = 140;
        this.PARAMETER_MISSING_WATERMARK_FORMAT = 141;
        this.PARAMETER_WATERMARK_TOO_LARGE = 142;
        this.PARAMETER_MISSING_PARAMETERS = 143;
        this.PARAMETER_QUALITY_TOO_LARGE = 144;
        this.PARAMETER_QUALITY_TOO_SMALL = 145;
        this.NETWORK_SERVER_OFFLINE = 200;
        this.NETWORK_GENERAL_ERROR = 201;
        this.NETWORK_DDOS_ATTACK = 202;
        this.RENDERING_ERROR = 300;
        this.RENDERING_MISSING_SCREENSHOT = 301;
        this.GENERIC_ERROR = 400;
        this.UPGRADE_REQUIRED = 500;
        this.FILE_SAVE_ERROR = 600;
        this.FILE_NON_EXISTANT_PATH = 601;   

        this.request = '';
        this.requestParams = null;
        this.signaturePartOne = '';
        this.signaturePartTwo = '';
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
    md5.update(value);
    return md5.digest('hex');
}

function _toInt(value){
    if (value){
        return 1;
    }
    return 0;
}

function _getWaterMarks(applicationKey, applicationSecret, identifier, onComplete){
    var sig = _createSignature(applicationSecret + '|' + identifier);

    var params = {
        'key': applicationKey,
        'identifier': identifier,
        'sig': sig
    };

    _get(this, 'getwatermarks.ashx?' + querystring.stringify(params), 'watermarks', onComplete);
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

function _get(self, url, type, onComplete){
    var options = {
        host: 'grabz.it',
        port: 80,
        path: '/services/'+url,
        headers: { 'Accept': 'application/json' }
    }

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
                if (onComplete != null) {
                    var error = _getError(res, data);
                    var obj = _convert(data, type);
                    if (error != null) {
                        obj = null;
                    }
                    onComplete(error, obj);
                }
            } else if (onComplete != null) {
                onComplete(null, data);
            }
        });
    });

    request.end();
}

function _getError(res, data){
    if (res.statusCode == 403) {
        var error = new Error();
        error.message = 'Potential DDOS Attack Detected. Please wait for your service to resume shortly. Also please slow the rate of requests you are sending to GrabzIt to ensure this does not happen in the future.';
        error.code = self.NETWORK_DDOS_ATTACK;
            
        return error;
    }

    if (res.statusCode >= 400) {
        var error = new Error();
        error.message = 'A network error occured when connecting to the GrabzIt servers.';
        error.code = self.NETWORK_GENERAL_ERROR;

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

GrabzItClient.prototype.set_pdf_options = function (url, options) {
    var defaults = {
        'customId': '',
        'includeBackground': true,
        'pagesize': 'A4',
        'orientation': 'Portrait',
        'includeLinks': true,
        'includeOutline': false,
        'title': '',
        'targetElement': '',
        'coverUrl': '',
        'marginTop': 10,
        'marginLeft': 10,
        'marginBottom': 10,
        'marginRight': 10,
        'delay': '',
        'requestAs': 0,
        'customWaterMarkId': '',
        'quality': -1,
        'country': ''
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

    this.requestParams = {
        'key':this.applicationKey,
        'url':url,
        'background':_toInt(context['includeBackground']),
        'pagesize':pagesize,
        'orientation':orientation,
        'customid':context['customId'],
        'customwatermarkid':context['customWaterMarkId'],        
        'delay':context['delay'],
        'target':context['targetElement'],
        'customwatermarkid':context['customWaterMarkId'],
        'includelinks':_toInt(context['includeLinks']),
        'includeoutline':_toInt(context['includeOutline']),
        'title':context['title'],
        'coverurl':context['coverUrl'],
        'mleft':parseInt(context['marginLeft']),
        'mright':parseInt(context['marginRight']),
        'mtop':parseInt(context['marginTop']),
        'mbottom':parseInt(context['marginBottom']),
        'delay':context['delay'],
        'requestmobileversion':parseInt(context['requestAs']),
        'quality':parseInt(context['quality']),
        'country':context['country']
    };

    this.request = 'takepdf.ashx?';
    this.signaturePartOne = this.applicationSecret + '|' + url + '|';
    this.signaturePartTwo = '|' + context['customId'] + '|' + _toInt(context['includeBackground']) + '|' + pagesize + '|' + orientation + '|' + context['customWaterMarkId']
     + '|' + _toInt(context['includeLinks']) + '|' + _toInt(context['includeOutline']) + '|' + context['title'] + '|' + context['coverUrl'] + '|' + parseInt(context['marginTop'])
     + '|' +  parseInt(context['marginLeft']) + '|' + parseInt(context['marginBottom']) + '|' + parseInt(context['marginRight']) + '|' + context['delay']
     + '|' +  parseInt(context['requestAs'])  + '|' + context['country'] + '|' + context['quality'];
};

GrabzItClient.prototype.set_table_options = function (url, options) {
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

    this.requestParams = {
        'key': this.applicationKey,
        'url': url,
        'includeAllTables': _toInt(context['includeAllTables']),
        'includeHeaderNames': _toInt(context['includeHeaderNames']),
        'format': context['format'],
        'tableToInclude': parseInt(context['tableNumberToInclude']),
        'customid': context['customId'],
        'target': context['targetElement'],
        'requestmobileversion': parseInt(context['requestAs']),
        'country': context['country']
    };

    this.request = 'taketable.ashx?';
    this.signaturePartOne = this.applicationSecret + '|' + url + '|';
    this.signaturePartTwo = '|' + context['customId'] + '|' + parseInt(context['tableNumberToInclude']) + '|' + _toInt(context['includeAllTables']) + '|' + _toInt(context['includeHeaderNames']) + '|' + context['targetElement']
     + '|' + context['format'] + '|' + parseInt(context['requestAs']) + '|' + context['country'];

};

GrabzItClient.prototype.set_image_options = function (url, options) {
    var defaults = {
        'customId': '',
        'browserWidth': '',
        'browserHeight': '',
        'width': '',
        'height': '',
        'format': '',
        'delay': '',
        'targetElement': '',
        'requestAs': 0,
        'customWaterMarkId': '',
        'quality': -1,
        'country': ''
    };

    context = _extend(defaults, options);

    this.requestParams = {
        'key':this.applicationKey,
        'url':url,
        'width':context['width'],
        'height':context['height'],
        'format':context['format'],
        'bwidth':context['browserWidth'],
        'bheight':context['browserHeight'],
        'customid':context['customid'],
        'delay':context['delay'],
        'target':context['targetElement'],
        'customwatermarkid':context['customWaterMarkId'],
        'requestmobileversion':parseInt(context['requestAs']),
        'country':context['country'],
        'quality':parseInt(context['quality'])
    };

    this.request = 'takepicture.ashx?';
    this.signaturePartOne = this.applicationSecret + '|' + url + '|';
    this.signaturePartTwo = '|' + context['format'] + '|' + context['height'] + '|' + context['width'] + '|' + context['browserHeight'] + '|' + context['browserWidth']
     + '|' + context['customId'] + '|' + context['delay'] + '|' + context['targetElement'] + '|' + context['customWaterMarkId'] + '|' + _toInt(context['requestAs']) 
     + '|' + context['country'] + '|' + context['quality'];
};

GrabzItClient.prototype.save = function (options) {
    var defaults = {
        'callBackUrl': '',
        'onComplete': null
    };

    context = _extend(defaults, options);

    onComplete = context['onComplete'];
        
    if (this.signaturePartOne == '' && this.signaturePartTwo == '' && this.request == '') {
        if (onComplete != null){
            var error = new Error();
            error.message = 'No screenshot parameters have been set.';
            error.code = this.PARAMETER_MISSING_PARAMETERS;
            onComplete(error, null);
        }
        return;
	}

	this.requestParams.callback = context['callBackUrl'];

    var sig = _createSignature(this.signaturePartOne+context['callBackUrl']+this.signaturePartTwo);                                              
    var currentRequest = this.request + querystring.stringify(this.requestParams) + '&sig=' + sig;

    _get(this, currentRequest, 'screenshot', onComplete);
};

GrabzItClient.prototype.save_to = function (options) {
    var defaults = {
        'saveToFile': '',
        'onComplete': null
    };

    context = _extend(defaults, options);

    saveToFile = context['saveToFile'];
    onCompleteEvent = context['onComplete'];

    var self = this;

    this.save({ 'onComplete': function (err, id) {
        if (err != null) {
            if (onCompleteEvent != null) {
                onCompleteEvent(err, null);
            }
            return;
        }

        var intervalId = setInterval(function () {
            self.get_status(id, function (err, status) {
                if (err != null) {
                    clearInterval(intervalId);
                        
                    if (onCompleteEvent != null) {
                        onCompleteEvent(err, null);
                    }
                    return;
                }

                if (!status.cached && !status.processing) {
                    clearInterval(intervalId);

                    if (onCompleteEvent != null) {
                        var error = new Error();
                        error.message = 'The screenshot did not complete with the error: ' + status.message;
                        error.code = this.RENDERING_ERROR;

                        onCompleteEvent(error, null);
                    }
                }
                else if (status.cached) {
                    clearInterval(intervalId);

                    self.get_result(id, function (err, result) {
                        if (result == null || result == '') {
                            if (onCompleteEvent != null) {
                                var error = new Error();
                                error.message = 'The screenshot could not be found on GrabzIt.';
                                error.code = this.RENDERING_MISSING_SCREENSHOT;

                                onCompleteEvent(error, null);

                                return;
                            }
                        }

                        if (saveToFile == '') {
                            if (onCompleteEvent != null) {
                                onCompleteEvent(null, result)
                            }
                        }

                        file.writeFile(saveToFile, result, 'binary', function(err) {
                            if (onCompleteEvent != null) {
                                onCompleteEvent(err, null)
                            }
                        });
                    });
                }
            });
        }, 3000);
    }
    });
};

GrabzItClient.prototype.get_result = function (id, onComplete) {
    _get(this, 'getfile.ashx?id=' + id, 'binary', onComplete)
};

GrabzItClient.prototype.get_status = function (id, onComplete) {
    _get(this, 'getstatus.ashx?id=' + id, 'status', onComplete);
};

GrabzItClient.prototype.get_cookies = function (domain, onComplete) {
    var sig = _createSignature(this.applicationSecret+'|'+domain);

    var params = {
        'key':this.applicationKey,
        'domain':domain,
        'sig':sig
    };    

    _get(this, 'getcookies.ashx?' + querystring.stringify(params), 'cookies', onComplete)
};

GrabzItClient.prototype.set_cookie = function (name, domain, options, onComplete) {

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

    _get(this, 'setcookie.ashx?' + querystring.stringify(params), 'result', onComplete);
};

GrabzItClient.prototype.delete_cookie = function (name, domain, onComplete) {

    var sig = _createSignature(this.applicationSecret + '|' + name + '|' + domain + '|1');

    var params = {
        'key': this.applicationKey,
        'name': name,
        'domain': domain,
        'delete': 1,
        'sig': sig
    };

    _get(this, 'setcookie.ashx?' + querystring.stringify(params), 'result', onComplete);
};

GrabzItClient.prototype.add_watermark = function (identifier, filePath, xpos, ypos, onComplete) {

    var self = this;

    file.exists(filePath, function (exists) {
        if (!exists) {
            if (onComplete != null) {
                var error = new Error();
                error.message = 'File: ' + filePath + ' does not exist';
                error.code = self.FILE_NON_EXISTANT_PATH;

                onComplete(error, null);
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
                if (onComplete != null) {
                    var error = _getError(res, data);
                    var obj = _convert(data, 'result');
                    if (error != null) {
                        obj = null;
                    }

                    onComplete(error, obj);
                }
            });
        });

        for (var i = 0; i < headerparams.postdata.length; i++) {
            request.write(headerparams.postdata[i]);
        }
        request.end();
    });
};

GrabzItClient.prototype.delete_watermark = function (identifier, onComplete) {
    var sig = _createSignature(this.applicationSecret + '|' + identifier);

    var params = {
        'key': this.applicationKey,
        'identifier': identifier,
        'sig': sig
    };

    _get(this, 'deletewatermark.ashx?' + querystring.stringify(params), 'result', onComplete);
};

GrabzItClient.prototype.get_watermarks = function (onComplete) {
    _getWaterMarks(this.applicationKey, this.applicationSecret, '', onComplete);
}

GrabzItClient.prototype.get_watermark = function (identifier, onComplete) {
    _getWaterMarks(this.applicationKey, this.applicationSecret, identifier, function (err, watermarks) {
        if (err != null) {
            if (onComplete != null) {
                onComplete(err, null);
            }
            return;
        }
        if (watermarks != null && watermarks.length == 1) {
            if (onComplete != null) {
                onComplete(null, watermarks[0]);
            }
            return;
        }

        if (onComplete != null) {
            onComplete(null, null);
        }
    });
}

module.exports = GrabzItClient;