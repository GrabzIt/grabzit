
/**
 * Module dependencies.
 */

var express = require('express')
  , routes = require('./routes')
  , http = require('http')
  , path = require('path')
  , url = require('url')
  , file = require('fs')
  , config = require('./config.js')
  , grabzit = require('grabzit');

var app = express();

app.configure(function(){
  app.set('port', process.env.PORT || 3000);
  app.set('views', __dirname + '/views');
  app.set('view engine', 'jade');
  app.use(express.favicon());
  app.use(express.logger('dev'));
  app.use(express.bodyParser());
  app.use(express.methodOverride());
  app.use(app.router);
  app.use(express.static(path.join(__dirname, 'public')));
});

app.configure('development', function(){
  app.use(express.errorHandler());
});

app.get('/', routes.index);

app.get('/ajax/results', function (req, res) {
    file.readdir(path.join('public', 'results'), function (err, files) {
    res.writeHead(200, {"Content-Type": "application/json"});
    res.end(JSON.stringify(files));});
});

app.get('/delete', function (req, res) {
    file.readdir(path.join('public', 'results'), function (err, files) {
        files.forEach(function (filename) {
            file.unlink(path.join(path.join('public', 'results'), filename));
        });
        res.writeHead(302, {
          'Location': '/'
        });
        res.end();
    });
});

app.get('/handler', function (req, res) {
    var queryData = url.parse(req.url, true).query;

    var message = queryData.message;
    var customid = queryData.customid;
    var id = queryData.id;
    var filename = queryData.filename;
    var format = queryData.format;

    var client = new grabzit(config.applicationKey, config.applicationSecret);

    client.get_result(id, function(err, result){
        if (err != null) {
            return;
        }

        file.writeFile(path.join('public', path.join('results', filename)), result, 'binary');
    });

    res.end();
});

app.post('/', function (req, res) {
    var targetUrl = req.body.url;
    var isHtml = req.body.convert == "html";
    var client = new grabzit(config.applicationKey, config.applicationSecret);
    if (req.body.type == "jpg") {
		if (isHtml){
			client.html_to_image(req.body.html);
		}
		else {
        	client.url_to_image(targetUrl);
		}
    }
    else if (req.body.type == "docx") {
		if (isHtml){
			client.html_to_docx(req.body.html);
		}
		else {
        	client.url_to_docx(targetUrl);
		}
    }	
    else if (req.body.type == "gif") {
	    client.url_to_animation(targetUrl);
    }
    else {
		if (isHtml){
			client.html_to_pdf(req.body.html);
		}
		else {
			client.url_to_pdf(targetUrl);
		}
    }

    client.save(config.callbackHandlerUrl, function (error, id){
        if (error != null){
            res.render('index', {'message': error.message, 'success': false});
            return;
        }
        res.render('index', {'success': true});
    });
});

http.createServer(app).listen(app.get('port'), function(){
  console.log("Express server listening on port " + app.get('port'));
});
