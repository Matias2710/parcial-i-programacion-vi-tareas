param([Parameter(Mandatory=$true)][string]$MongodPath)
$ErrorActionPreference='Stop'
Set-Location (Split-Path $PSScriptRoot -Parent)
$env:MONGOD_PATH=(Resolve-Path -LiteralPath $MongodPath).Path
dotnet test ParcialI.sln -c Release --logger 'trx;LogFileName=unit.trx' --results-directory evidencias
if($LASTEXITCODE -ne 0){throw 'Pruebas .NET fallaron'}
npm.cmd ci --prefix frontend --no-audit --no-fund
if($LASTEXITCODE -ne 0){throw 'Instalación Angular falló'}
npm.cmd run build --prefix frontend
if($LASTEXITCODE -ne 0){throw 'Compilación Angular falló'}
npm.cmd ci --no-audit --no-fund
if($LASTEXITCODE -ne 0){throw 'Instalación pruebas falló'}
npx.cmd playwright install chromium
if($LASTEXITCODE -ne 0){throw 'Chromium no está disponible'}
npm.cmd test
if($LASTEXITCODE -ne 0){throw 'Pruebas de integración/E2E fallaron'}
