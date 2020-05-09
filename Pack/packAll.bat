set PACKAGEVERSION=1.2.44
msbuild ..\WPFControlNetFramework\WPFControlNetFramework.csproj -p:Configuration=Release 
msbuild ..\WPFControlNetFramework.Design\WPFControlNetFramework.Design.csproj -p:Configuration=Release 
..\libs\nuget.exe pack ..\WpfControlNetFramework\WpfControlNetFramework.nuspec -Version %PACKAGEVERSION% -OutputDirectory Packages