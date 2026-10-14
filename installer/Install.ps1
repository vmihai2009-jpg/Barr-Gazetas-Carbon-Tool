<#
  Installs the Barr Gazetas Carbon Tool for one or more Revit versions.
  Build first (see README.md), then run from the repository root:
      powershell -ExecutionPolicy Bypass -File installer\Install.ps1 -Versions 2025,2026
  Add -Uninstall to remove it. Writes to %ProgramData%, so run as administrator.
#>
param(
    [int[]]$Versions = @(2025, 2026, 2027),
    [switch]$Uninstall
)

$root = Split-Path -Parent $PSScriptRoot
$addinRoot = Join-Path $env:ProgramData 'Autodesk\Revit\Addins'

foreach ($year in $Versions) {
    $yearFolder = Join-Path $addinRoot $year
    $toolFolder = Join-Path $yearFolder 'BarrGazetasCarbonTool'
    $manifest   = Join-Path $yearFolder 'BarrGazetasCarbonTool.addin'

    if ($Uninstall) {
        Remove-Item $toolFolder -Recurse -Force -ErrorAction SilentlyContinue
        Remove-Item $manifest -Force -ErrorAction SilentlyContinue
        Write-Host "Revit $year: removed"
        continue
    }

    # Release builds land in bin\R<year>\Release\<framework>\ (the framework folder differs per Revit version).
    $build = Get-ChildItem (Join-Path $root "bin\R$year") -Recurse -Filter 'RevitAddIn.dll' -ErrorAction SilentlyContinue |
             Where-Object { $_.FullName -match '\\Release\\' } | Select-Object -First 1
    if (-not $build) { Write-Warning "Revit ${year}: no Release build found - run: dotnet build -c Release -p:RevitVersion=$year"; continue }

    New-Item -ItemType Directory -Force $toolFolder | Out-Null
    Copy-Item (Join-Path $build.DirectoryName '*') $toolFolder -Recurse -Force
    Copy-Item (Join-Path $PSScriptRoot 'BarrGazetasCarbonTool.addin') $manifest -Force
    Write-Host "Revit ${year}: installed to $toolFolder"
}
