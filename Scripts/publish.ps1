Param(
    # Output Directory
    [Parameter(HelpMessage = "The location to store the zipped builds")]
    [string]
    $output
)

# -------------------------------------------
# Set Up
# -------------------------------------------

Write-Host "Starting Setup Process..."
Start-Sleep -Seconds 1

Write-Host "Checking Output Directory..."
if(!($output) -or !(Test-Path -Path $output))
{
    $current = Get-Location
    $output = Join-Path -Path $current -ChildPath "dist"
    Write-Host "No output path was explicitly set. \n The output will now default to: $output" -ForegroundColor Yellow
}

Write-Host "Setting Path..."
Set-Location -Path './BeatSpy'

Write-Host "Fetching Version..."
try {
    [xml]$proj = Get-Content BeatSpy.csproj
    $version = $proj.Project.PropertyGroup.Version
    Write-Host "Successfully Fetched Version: $version" -ForegroundColor Green
}
catch {
    Write-Host "Failed To Fetch Version"
    exit
}

Write-Host "Cleaning Bin Folder..."
try {
    Remove-Item -Path "./bin" -Recurse -Force
}
catch {
    Write-Host "Failed To Clean Bin Folder"
    exit
}

Write-Host "Cleaning Output Folder..."
try {
    Remove-Item -Path $output -Recurse -Force
}
catch {
    Write-Host "Failed To Clean Output Folder"
    exit
}

Write-Host "Finished Setup Process..." -ForegroundColor Green

# -------------------------------------------
# Build Framework Dependant
# -------------------------------------------

Write-Host "Starting Framework Dependant Build Process..."
Start-Sleep -Seconds 1

dotnet publish -c Release -r win-x64 --no-self-contained --nologo --output "./bin/framework-dependant"

# Check For Errors
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An Error Occurred While Building" -ForegroundColor Red
    exit
}

Write-Host "Finished Framework Dependant Build Process..." -ForegroundColor Green
Start-Sleep -Seconds 1

# -------------------------------------------
# Build Self Contained
# -------------------------------------------

Write-Host "Starting Self Contained Build Process..."
Start-Sleep -Seconds 1

dotnet publish -c Release -r win-x64 --self-contained --nologo --output "./bin/self-contained"

# Check For Errors
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An Error Occurred While Building" -ForegroundColor Red
    exit
}

Write-Host "Finished Self Contained Build Process..." -ForegroundColor Green
Start-Sleep -Seconds 1

# -------------------------------------------
# Installer
# -------------------------------------------

Write-Host "Starting Installer Process..."
Start-Sleep -Seconds 1

iscc /Qp /O"$output" /F"BeatSpy_Installer_x64" "../Scripts/beatspy-installer.iss" /DAppVersion=$version

# Check For Errors
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An Error Occurred While Creating Installer" -ForegroundColor Red
    exit
}

Write-Host "Finished Installer" -ForegroundColor Green

# -------------------------------------------
# Zipping
# -------------------------------------------

Write-Host "Starting Self Contained Zip Process..."
Start-Sleep -Seconds 1

$selfcontained = Join-Path -Path $output -ChildPath "BeatSpy_SelfContained_x64.zip"

7z a -bsp2 -r $selfcontained "./bin/self-contained/*"

# Check For Errors
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An Error Occurred While Zipping" -ForegroundColor Red
    exit
}

Write-Host "Finished Self Contained Zip Process..." -ForegroundColor Green
Start-Sleep -Seconds 1

Write-Host "Starting Framework Dependent Zip Process..."
Start-Sleep -Seconds 1

$framework = Join-Path -Path $output -ChildPath "BeatSpy_FrameworkDependent_x64.zip"

7z a -bsp2 -r $framework "./bin/framework-dependant/*"

# Check For Errors
if ($LASTEXITCODE -ne 0)
{
    Write-Host "An Error Occurred While Zipping" -ForegroundColor Red
    exit
}

Write-Host "Finished Framework Dependent Zip Process..." -ForegroundColor Green
Start-Sleep -Seconds 1

Write-Host "Publish Completed!" -ForegroundColor Green
Start-Sleep -Seconds 2

Set-Location "../"