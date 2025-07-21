rmdir /Q /S dist
python setup.py sdist
python -m twine upload --repository grabzit dist/*