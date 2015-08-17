<%@page import="java.net.URLDecoder"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>GrabzIt Demo</title>
        <link rel="stylesheet" type="text/css" href="css/style.css">
        <script type="text/javascript"  src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
        <script type="text/javascript"  src="ajax/ui.js"></script>
    </head>
    <body>
        <h1>GrabzIt Demo</h1>
        <form method="post" action="/grabzit/takescreenshot" class="inputForms">
        <p><span id="spnScreenshot">Enter the URL of the website you want to take a screenshot of. The resulting screenshot</span><span class="hidden" id="spnGif">Enter the URL of the online video you want to convert into a animated GIF. The resulting animated GIF</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>
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
        <label style="font-weight:bold;margin-right:1em;">URL </label><input text="input" name="url"/> <select name="format" onchange="selectChanged(this)">
          <option value="jpg">JPG</option>
          <option value="pdf">PDF</option>
          <option value="gif">GIF</option>
        </select>
        <input type="submit" value="Grabz It"/>
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
