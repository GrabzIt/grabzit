#!/usr/bin/env python

from distutils.core import setup
from os import path

this_directory = path.abspath(path.dirname(__file__))
with open(path.join(this_directory, 'README.txt')) as f:
    long_description = f.read()

setup(name='GrabzIt',
      version='3.5.1',
      author='GrabzIt',
      author_email='support@grabz.it',
      url='https://grabz.it/api/python/',
      license='MIT Licence',
      description='GrabzIt enables allows you to programmatically convert web pages and HTML into images, DOCX, rendered HTML, PDFs, CSVs and spreadsheets. Additionally GrabzIt allows you to convert online videos into animated GIFs',
      long_description=long_description,
      long_description_content_type='text/markdown',
      packages=['GrabzIt']      
)
