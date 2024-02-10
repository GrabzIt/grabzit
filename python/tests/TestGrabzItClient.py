#!/usr/bin/python

import unittest
from GrabzIt import GrabzItClient

class TestGrabzItClient(unittest.TestCase):
    def setUp(self):
        self.client = GrabzItClient.GrabzItClient("YOUR APPLICATION KEY", "YOUR APPLICATION SECRET")

    def test_html_to_image(self):
        self.client.HTMLToImage("<h1>Hello world</h1>")
        self.assertNotEqual(self.client.Save(), "", "HTML not converted")

    def test_html_to_video(self):
        self.client.HTMLToVideo("<h1>Hello world</h1>")
        self.assertNotEqual(self.client.Save(), "", "HTML not converted")       

    def test_url_to_video(self):
        self.client.URLToVideo("https://grabz.it")
        self.assertNotEqual(self.client.Save(), "", "URL not converted")         

if __name__ == '__main__':
    unittest.main()