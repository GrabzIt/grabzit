
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

app.get('/', function(req, res) {
    res.render('index', {'useCallbackHandler': useCallbackHandler(), 'success': false, 'message': null});
});

app.get('/ajax/results', function (req, res) {
    file.readdir(path.join('public', 'results'), function (err, files) {
    var results = new Array();
    for(var i=0;i<files.length;i++){
        if (files[i] == 'results.txt'){
            continue;
        }
       results.push('results/'+files[i]);
    }
    res.writeHead(200, {"Content-Type": "application/json"});
    res.end(JSON.stringify(results));});
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
	var targeterror = queryData.targeterror;

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
    else if (req.body.type == "csv") {
        if (isHtml){
            client.html_to_table(req.body.html);
        }
        else {
            client.url_to_table(targetUrl);
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

    if (useCallbackHandler())
    {
        client.save(config.callbackHandlerUrl, function (error, id){
            if (error != null){
                res.render('index', {'useCallbackHandler': useCallbackHandler(), 'message': error.message, 'success': false});
                return;
            }
            res.render('index', {'useCallbackHandler': useCallbackHandler(), 'success': true, 'message': null});
        });
    }
    else
    {
        client.save_to(path.join('public', 'results', (''+Math.floor(Math.random() * 9999999) + 1))+'.'+req.body.type, function (error, id){
            if (error != null){
                res.render('index', {'useCallbackHandler': useCallbackHandler(), 'message': error.message, 'success': false});
                return;
            }
            res.render('index', {'useCallbackHandler': useCallbackHandler(), 'success': false, 'message': null});        
        });
    }
});

var server = http.createServer(app).listen(app.get('port'), function(){
  console.log("Express server listening on port " + app.get('port'));
});

function useCallbackHandler()
{
    var RegExp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
    if(!RegExp.test(config.callbackHandlerUrl)){
        return false;
    }   
    return server.address().address != '::' && server.address().address != '127.0.0.1'; 
}
