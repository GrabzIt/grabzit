GrabzIt

This software allows you to programmatically take screenshots of websites.

Its usually best to place these files in thier own directory.

To run the sample

Open web.config and change the application key and application secret to match your settings the first two parameters can be got from http://grabz.it/api

  <appSettings>
    <add key="ApplicationKey" value="APPLICATION KEY"/>
    <add key="ApplicationSecret" value="APPLICATION SECRET"/>
  </appSettings>

Next add the GrabzIt http handler to the web.config with the location it is currently stored at.

      <httpHandlers>
        <add verb="*" path="GrabzIt.ashx" type="GrabzIt.Handler, GrabzIt" />
      </httpHandlers>

Ensure your application has read and write access to the "screenshots" directory.

Finally run the Default.aspx to start taking screenshots.

More documentation can be found at: http://grabz.it/api/aspnet