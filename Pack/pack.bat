set PACKAGEVERSION=1.2.63
msbuild ..\WPFControlNetFramework\WPFControlNetFramework.csproj -p:Configuration=Release 
msbuild ..\WPFControlNetFramework.Design\WPFControlNetFramework.Design.csproj -p:Configuration=Release 
msbuild ..\WPFControlNetFramework.ConsoleApp\WPFControlNetFramework.ConsoleApp.csproj -p:Configuration=Release 
..\libs\nuget.exe pack ..\WpfControlNetFramework\WpfControlNetFramework.nuspec -Version %PACKAGEVERSION% -OutputDirectory Packages