SET /P version=Please enter the version number

del /F /Q /S C:\Apps\grabzit\java\tmp-maven\tmp
cd C:\Apps\grabzit\java\grabzit\target\apidocs
"C:\Program Files\Java\jdk1.8.0_111\bin\jar" -cMf "C:\Apps\grabzit\java\tmp-maven\tmp\grabzit-%version%-javadoc.jar" .
cd C:\Apps\grabzit\java\grabzit\src\main\java
"C:\Program Files\Java\jdk1.8.0_111\bin\jar" -cMf "C:\Apps\grabzit\java\tmp-maven\tmp\grabzit-%version%-sources.jar" .

cd C:\Apps\grabzit\java\tmp-maven\tmp\
copy C:\Apps\grabzit\java\grabzit\pom.xml pom.xml
copy C:\Apps\grabzit\java\grabzit.jar grabzit-%version%.jar
gpg -ab pom.xml
gpg -ab grabzit-%version%-javadoc.jar
gpg -ab grabzit-%version%.jar
gpg -ab grabzit-%version%-sources.jar
del /F /Q /S C:\Apps\grabzit\java\tmp-maven\bundle.jar
cd C:\Apps\grabzit\java\tmp-maven\tmp
"C:\Program Files\Java\jdk1.8.0_111\bin\jar" -cMf "C:\Apps\grabzit\java\tmp-maven\bundle.jar" .
cd C:\Apps\grabzit\java\tmp-maven\
start chrome --new-window "https://oss.sonatype.org/"