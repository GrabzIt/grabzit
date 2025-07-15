SET /P version=Please enter the version number

mkdir C:\Apps\grabzit\java\tmp-maven\tmp
mkdir C:\Apps\grabzit\java\tmp-maven\tmp\it\grabz\grabzit\grabzit\%version%
del /F /Q /S C:\Apps\grabzit\java\tmp-maven\tmp
cd C:\Apps\grabzit\java\grabzit\target\apidocs
"C:\Program Files\Java\jdk-1.8\bin\jar" -cMf "C:\Apps\grabzit\java\tmp-maven\tmp\it\grabz\grabzit\grabzit\%version%\grabzit-%version%-javadoc.jar" .
cd C:\Apps\grabzit\java\grabzit\src\main\java
"C:\Program Files\Java\jdk-1.8\bin\jar" -cMf "C:\Apps\grabzit\java\tmp-maven\tmp\it\grabz\grabzit\grabzit\%version%\grabzit-%version%-sources.jar" .

cd C:\Apps\grabzit\java\tmp-maven\tmp\it\grabz\grabzit\grabzit\%version%
copy C:\Apps\grabzit\java\grabzit\pom.xml grabzit-%version%.pom
copy C:\Apps\grabzit\java\grabzit.jar grabzit-%version%.jar
gpg -ab grabzit-%version%.pom
CertUtil -hashfile grabzit-%version%.pom MD5 | find /v "hash" > grabzit-%version%.pom.md5
CertUtil -hashfile grabzit-%version%.pom SHA1 | find /v "hash" > grabzit-%version%.pom.sha1
gpg -ab grabzit-%version%-javadoc.jar
CertUtil -hashfile grabzit-%version%-javadoc.jar MD5 | find /v "hash" > grabzit-%version%-javadoc.jar.md5
CertUtil -hashfile grabzit-%version%-javadoc.jar SHA1 | find /v "hash" > grabzit-%version%-javadoc.jar.sha1
gpg -ab grabzit-%version%.jar
CertUtil -hashfile grabzit-%version%.jar MD5 | find /v "hash" > grabzit-%version%.jar.md5
CertUtil -hashfile grabzit-%version%.jar SHA1 | find /v "hash" > grabzit-%version%.jar.sha1
gpg -ab grabzit-%version%-sources.jar
CertUtil -hashfile grabzit-%version%-sources.jar MD5 | find /v "hash" > grabzit-%version%-sources.jar.md5
CertUtil -hashfile grabzit-%version%-sources.jar SHA1 | find /v "hash" > grabzit-%version%-sources.jar.sha1
del /F /Q /S C:\Apps\grabzit\java\tmp-maven\tmp\bundle.jar
cd C:\Apps\grabzit\java\tmp-maven\tmp
"C:\Program Files\Java\jdk-1.8\bin\jar" -cMf "C:\Apps\grabzit\java\tmp-maven\tmp\bundle.jar" .
cd C:\Apps\grabzit\java\tmp-maven\tmp\
start chrome --new-window "https://central.sonatype.com/publishing"