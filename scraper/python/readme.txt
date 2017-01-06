GrabzIt Scraper 1.1
===================

This library enables you to process scraped data and integrate it into your own application.

It is usually best to place these package files in their own directory.

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+ For your application to be reachable by GrabzIt it must first be deployed to a publicly accessible web server.+
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

Running the Demo Application
----------------------------

First create a scrape here: https://grabz.it/scraper/scrapes.aspx On the Export Options tab choose "Callback URL" option from the Send Results Via drop down.

Next enter the URL of the handler.py so for instance http://www.example.com/scrape/handler.py

Ensure your Python application has read and write access to the "results" directory.

To see how you can control your scrapes automatically call the index.py file in a browser.

More documentation can be found at: https://grabz.it/scraper/api/python/