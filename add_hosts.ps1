$hostsPath = "C:\Windows\System32\drivers\etc\hosts"
$entry = "`r`n# Dofus 3 MITM Proxy`r`n127.0.0.1 dofus2-co-production.ankama-games.com"
Add-Content -Path $hostsPath -Value $entry -Encoding ASCII
Write-Host "OK: hosts file updated"
