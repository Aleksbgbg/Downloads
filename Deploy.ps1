cd Downloads

dotnet restore
dotnet publish -c Release

cd bin/Release/netcoreapp2.2/publish/Client/dist/Client

Remove-Item * -Include *.txt

cd ../../..

Remove-Item * -Include *.pdb
Remove-Item * -Include *.Development.json

echo "`r" | gcloud app deploy app.yaml