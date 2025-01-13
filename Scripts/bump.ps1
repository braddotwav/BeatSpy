Param(
    [string]
    $ProjectPath
)

# -------------------------------------------
# Set Up
# -------------------------------------------

if(!($ProjectPath) -or !(Test-Path -Path $ProjectPath))
{
    Write-Host "Project file not found at the following location: $ProjectPath" -ForegroundColor Red
    exit
}

[xml]$proj = Get-Content $ProjectPath -ErrorAction Stop
$version = $proj.Project.PropertyGroup.Version
Write-Host "Successfully fetched project file that is currently at version: $version" -ForegroundColor Green

# -------------------------------------------
# Bump
# -------------------------------------------

$versionParts = $version -split '\.'

if ($versionParts -ge 2)
{
    $major = [int]$versionParts[0]
    $minor = [int]$versionParts[1]
    $patch = if ($versionParts.Count -ge 3) { [int]$versionParts[2] } else { 0 }
    
    $minor++
    $newVersion = "$major.$minor.$patch"
    
    Write-Host "Bumping version to: $newVersion" -ForegroundColor Yellow

    $proj.Project.PropertyGroup.Version = $newVersion
    $proj.Project.PropertyGroup.FileVersion = $newVersion
    $proj.Project.PropertyGroup.AssemblyVersion = $newVersion
    
    $proj.Save($ProjectPath)
}
else 
{
    Write-Host "Failed to parse version in project file" -ForegroundColor Red
    exit
}

Start-Sleep -Seconds 2
Write-Host "Version bump complete!" -ForegroundColor Green