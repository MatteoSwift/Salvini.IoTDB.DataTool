set BACK=%cd%
@cd %~dp0
dotnet publish -r linux-x64 -c Release -o ../../dist/linux-x64 --self-contained  /p:PublishSingleFile=true
dotnet publish -r win-x64 -c Release -o ../../dist/win-x64 --self-contained  /p:PublishSingleFile=true
dotnet publish -r osx-x64 -c Release -o ../../dist/osx-x64 --self-contained  /p:PublishSingleFile=true
@cd %BACK%