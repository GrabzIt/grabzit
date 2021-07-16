<%@page import="java.net.URLDecoder"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>GrabzIt Demo</title>
        <link rel="stylesheet" type="text/css" href="css/style.css">
        <script type="text/javascript" src="ajax/jquery.min.js"></script>
        <script type="text/javascript" src="ajax/ui.js"></script>
        <script>
        var ui = new UI('ajax/results?r=', 'css');
        </script>        
    </head>
    <body>
        <h1>GrabzIt Demo</h1>
        <form method="post" action="/" class="inputForms">
        <p><span id="spnIntro">Enter the HTML or URL you want to convert into a DOCX, PDF or Image. The resulting capture</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="https://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>
        <c:if test="${!useCallbackHandler}">
            <p>Either you have not updated the HandlerUrl setting found in the config.properties file to match the URL of the handler found in this demo app or you are using this demo application on your local machine.</p><p>This demo will still work although it will create captures synchronously, which will cause the web page to freeze when captures are generated. <u>Please wait for the capture to complete</u>.</p>
        </c:if>
        <p>
        <span class="error">
<%
    if (request.getParameter("error") != null) {
        out.println(URLDecoder.decode(request.getParameter("error"), "UTF-8"));
    }
%>
        </span>
        <span style="color:green;font-weight:bold;">
<%
    if (request.getParameter("message") != null) {
        out.println(URLDecoder.decode(request.getParameter("message"), "UTF-8"));
    }
%>
        </span>
        </p>
<div class="Row" id="divConvert">        
<label>Convert </label><select name="convert" onchange="ui.selectConvertChanged(this)">
  <option value="url">URL</option>
  <option value="html">HTML</option>
</select>
</div>        
<div id="divHTML" class="Row hidden">
<label>HTML </label><textarea name="html"><html><body><h1>Hello world!</h1></body></html></textarea>
</div>
<div id="divURL" class="Row">
<label>URL </label><input text="input" name="url" placeholder="http://www.example.com"/>
</div>
<div class="Row">
<label>Format </label><select name="format" onchange="ui.selectChanged(this)">
  <option value="jpg">JPG</option>
  <option value="pdf">PDF</option>
  <option value="docx">DOCX</option>
  <option value="gif">GIF</option>
  <option value="csv">CSV</option>
</select>
</div>
        <input type="submit" value="Grabz It" style="margin-left:12em"/>
        </form>
        <form method="post" action="/grabzit/clear" class="inputForms">
        <input type="submit" value="Clear Results"/>
        </form>
        <br />
        <h2>Completed Screenshots</h2>
        <div id="divResults"></div>
        </body>
        </html>
    </body>
</html>
