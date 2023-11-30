Param(
    # Target directory for build files.
    [Parameter(Mandatory, HelpMessage = "Provide the folder path for the release build")]
    [string]
    $BuildDirectory,

    # # Target directory for installer file.
    [Parameter(Mandatory, HelpMessage = "Provide the file path for the INNO installer")]
    [string]
    $InstallerPath,

    # Name of the output zip file
    [Parameter(Mandatory, HelpMessage = "Provide a output name for the zip file")]
    [string]
    $OutputFileName,

    # Should the script skip installer
    [switch] $SkipInstaller = $false,

    # Target directory for output.
    [Parameter(Mandatory, HelpMessage = "Provide a output directory")]
    [string]
    $OutputDirectory
)

#Try and compress the contents of the build folder 
#and save it as a zip in the output directory.
try {
    $CompressPath = $BuildDirectory + "\*"
    $DestonationPath = $OutputDirectory + "\$OutputFileName.zip"
    
    Write-Host "Attempting to compress build.."
    Compress-Archive -Path $CompressPath -DestinationPath $DestonationPath -ErrorAction Stop
    Write-Host "Successfully compressed build at the following location $OutputDirectory"
} 
catch {
    Write-Host "Failed to compress build. Error: $($_.Exception.Message)"
}


if(!$SkipInstaller)
{
    try {
        Write-Host "Attempting to run installer command.."
        $Command = "iscc /O'$OutputDirectory' '$InstallerPath'"
        Invoke-Expression $Command
    }
    catch {
        Write-Host "Failed to run installer command. Error: $($_.Exception.Message)"
    }
}