@ECHO OFF

SET GeneratorVersion=4.2.3



TITLE Generating TypeScript client for backend API

SET GeneratorFileName=openapi-generator-cli-%GeneratorVersion%.jar
SET GeneratorFilePath=%GeneratorFileName%
SET GeneratorDownloadUrl=https://repo1.maven.org/maven2/org/openapitools/openapi-generator-cli/%GeneratorVersion%/%GeneratorFileName%

REM Exit with an error if we don't have Java
WHERE java >nul 2>nul
IF %ERRORLEVEL% NEQ 0 (
    ECHO ERROR: Java not found in path. Java must be installed to run the API client generator. 1>&2
    EXIT /B 1
)

REM Download OpenAPI Generator if we don't already have it
IF NOT EXIST "%GeneratorFilePath%" (
    ECHO Downloading %GeneratorFileName%...
    powershell Invoke-WebRequest -OutFile "%GeneratorFilePath%" "%GeneratorDownloadUrl%"
)

ECHO Genrating client...
java -jar "%GeneratorFilePath%" generate -i ../backend/bin/WebApiSwagger.json -g typescript-fetch -o ./src/common/web-api/generated
