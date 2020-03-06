@ECHO OFF

SET GeneratorVersion=4.2.3



TITLE Generating TypeScript client for backend API

SET GeneratorFileName=openapi-generator-cli-%GeneratorVersion%.jar
SET GeneratorFilePath=%GeneratorFileName%
SET GeneratorDownloadUrl=https://repo1.maven.org/maven2/org/openapitools/openapi-generator-cli/%GeneratorVersion%/%GeneratorFileName%

IF NOT EXIST "%GeneratorFilePath%" (
    ECHO Downloading %GeneratorFileName%...
    powershell Invoke-WebRequest -OutFile "%GeneratorFilePath%" "%GeneratorDownloadUrl%"
)

ECHO Genrating client...
java -jar "%GeneratorFilePath%" generate -i ../backend/bin/WebApiSwagger.json -g typescript-fetch -o ./src/common/web-api/generated
