Param(
    # Root folder directory
    [Parameter(Mandatory, HelpMessage = "The folder that holds the solution file.")]
    [string]
    $slnDirectory,

    # Output folder directory 
    [Parameter(Mandatory, HelpMessage = "The location to place the finished files.")]
    [string]
    $outputDirectory,

    # Output file name
    [Parameter(Mandatory, HelpMessage = "This string will be used as the file name for both zip & installer.")]
    [string]
    $outputFileName
)

#Check if a ".iss" file exists inside of the "Scripts" folder
#If it doesn't write to the console and break
$installerScriptPath = Join-Path -Path $slnDirectory -ChildPath "Scripts/*.iss"
$installerScript = Get-Item -Path $installerScriptPath
if(!(Test-Path -Path $installerScriptPath))
{
    Write-Host "Scripts folder does not exist"
    break
}

#Check if the "Publish" folder exists inside of the bin folder
#If it doesn't write to the console and break
$publishPath = Join-Path -Path $slnDirectory -ChildPath "BeatSpy/bin/Publish/*"
if(!(Test-Path -Path $publishPath))
{
    Write-Host "Publish folder does not exist"
    break
}

#Check if there is previous content inside of the output directory
#Delete the contents if there is
$outputDirectoryContents = Join-Path -Path $outputDirectory -ChildPath "*"
if(Test-Path -Path $outputDirectoryContents)
{
    Remove-Item -Path $outputDirectoryContents
    Write-Host "Found and deleted previous build"
    Start-Sleep -Seconds 2
}

#Try and compress the contents of the "Publish" folder to the output
#directory that was specified
try 
{
    #Join both the output directory and the output file name
    $outputDirectoryWithFinalName = Join-Path -Path $outputDirectory -ChildPath "$outputFileName.zip"

    Write-Host "Starting compression stage"
    Compress-Archive -Path $publishPath -DestinationPath $outputDirectoryWithFinalName -Force -ErrorAction Stop
    Write-Host "Finished compression stage"
}
catch 
{
    Write-Host "Failed to compress build contents: $($_.Exception.Message)"
    break
}

#Try and run the inno setup installer command
try {
    Write-Host "Starting installer stage"
    Invoke-Expression "iscc /Qp /O'$outputDirectory' /F'$outputFileName' '$installerScript'"
    Write-Host "Finished installer stage"
}
catch {
    Write-Host "Failed to start inno set up installer: $($_.Exception.Message)"
    break
}

Write-Host "Build finished"