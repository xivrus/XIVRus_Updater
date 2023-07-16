<#
    Created by EnderVAD - https://github.com/endervad
#>

$csproj_path  = '.\XIVRus Updater\XIVRus Updater.csproj'
$csproj_xpath = '/Project/PropertyGroup/Version'

$version_string = $(Select-Xml -Path $csproj_path -XPath $csproj_xpath | Select-Object -ExpandProperty 'Node').'#text'
csq pack -u 'XIVRusUpdater' -v $version_string -p '.\XIVRus Updater\bin\Release\net6.0-windows' -e 'XIVRus Updater.exe'
