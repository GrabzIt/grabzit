rmdir /Q /S dist
C:\Python39\python setup.py sdist
C:\Python39\python -m twine upload --repository grabzit dist/*