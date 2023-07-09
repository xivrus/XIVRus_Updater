<#
    Created by EnderVAD - https://github.com/endervad
#>

$csproj_path  = '.\XIVRUS Updater\XIVRUS Updater.csproj'
$csproj_xpath = '/Project/PropertyGroup/Version'

$version_string = $(Select-Xml -Path $csproj_path -XPath $csproj_xpath | Select-Object -ExpandProperty 'Node').'#text'
csq pack -u 'XIVRUSUpdater' -v $version_string -p '.\XIVRUS Updater\bin\Release\net6.0-windows' -e 'XIVRUS Updater.exe'
