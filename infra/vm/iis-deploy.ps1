param([string]$SiteName="CareerCompassWeb",[string]$PublishFolder="C:\deploy\web\publish",[string]$IisPath="C:\inetpub\wwwroot\CareerCompassWeb",[int]$Port=80)
New-Item -ItemType Directory -Force -Path $PublishFolder | Out-Null
New-Item -ItemType Directory -Force -Path $IisPath | Out-Null
Copy-Item -Path "$PublishFolder\*" -Destination $IisPath -Recurse -Force
Import-Module WebAdministration
if(-not (Test-Path "IIS:\Sites\$SiteName")){New-Item "IIS:\Sites\$SiteName" -bindings @{protocol="http";bindingInformation="*:${Port}:"} -physicalPath $IisPath | Out-Null}else{Restart-WebAppPool "$SiteName"}
# Add URL Rewrite manually: /api/* -> http://vm-api:5000
