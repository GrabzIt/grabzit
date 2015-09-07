#!/usr/bin/python
from GrabzIt import ScrapeResult
scrapeResult = ScrapeResult.ScrapeResult()
scrapeResult.save("results/"+scrapeResult.getFilename())

print "Content-type: text/html"
print