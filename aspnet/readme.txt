GrabzIt 3.4
===========

This library allows you to programmatically convert web pages and HTML into images, DOCX documents, PDF's, CSV's and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIF's.

It is usually best to place these files in their own directory.

To run the demos:

First open the GrabzIt solution file then follow the below instructions for the three sample projects contained within.

Captures in a Web Application
=============================

The solution contains two web application projects called Sample and Sample MVC, which is an example web application using Web Forms and MVC respectively. In order to use the callback handler contained in the sample projects, the project must be deployed to a publicly accessible web server. However, these demos will still work on a local machine by automatically using synchronous methods of downloading the captures instead.

To configure these projects do the following:

Open the web.config contained in the project and change the application key and application secret to match your settings, which can be obtained from https://grabz.it/api

  <appSettings>
    <add key="ApplicationKey" value="APPLICATION KEY"/>
    <add key="ApplicationSecret" value="APPLICATION SECRET"/>
  </appSettings>

Ensure your application has read and write access to the "results" directory.

Finally run your desired project to start creating captures.

Captures in a Windows Application
=================================

The solution also contains a project called SampleConsole, which is a example windows console application that can be run on your local machine. To configure this do the following:

Then open App.config and change the application key and application secret to match your settings, which can be obtained from https://grabz.it/api

  <appSettings>
    <add key="ApplicationKey" value="APPLICATION KEY"/>
    <add key="ApplicationSecret" value="APPLICATION SECRET"/>
  </appSettings>
  
Finally run the console application to create captures in windows.

More documentation can be found at: https://grabz.it/api/aspnet/