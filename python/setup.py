#!/usr/bin/env python

from distutils.core import setup

setup(name='GrabzIt',
      version='2.2.2.1',
      summary='Capture websites as screenshots, CSVs and animated GIFs',
      author='GrabzIt',
      url='http://grabz.it/api/python/',
      license='MIT Licence',
      description='Capture websites with GrabzIt by taking highly customizable PDF or image screenshots. You can also convert the content of the websites by converting on-line videos into animated GIFs or extract tables from web pages and convert them into a CSV or Excel spreadsheets',
      data_files=[('bitmaps', ['css/pdf.png'])],
      scripts = [
              'config.ini',
              'handler.py',
              'index.py',
              'results/results.txt',
              'GrabzIt/ScreenShotStatus.py',
              'GrabzIt/GrabzItWaterMark.py',
              'GrabzIt/GrabzItException.py',
              'GrabzIt/GrabzItCookie.py',
              'GrabzIt/GrabzItClient.py',
              'GrabzIt/__init__.py',
              'css/style.css',
              'ajax/results.py',
              'ajax/ui.js'
      ]
)