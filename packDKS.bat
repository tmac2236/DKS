D:
rd /q /s D:\shc_workspace\Publish\DKS-API
rd /q /s D:\shc_workspace\Publish\DKS-SPA
cd D:\shc_workspace\DKS\DKS-API
dotnet publish -o ..\..\Publish\DKS-API
cd ..\DKS-SPA
ng build --prod --build-optimizer=false
