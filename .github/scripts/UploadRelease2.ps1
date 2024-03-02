param(
    [String]$accessToken,
    [String]$workspace,
    [String]$releaseId,
    [String]$version
)

# Define paths
$exeFilePath = "${workspace}\Staging\Build\DetectiveSpecs-v${version}.exe"
$zipFilePath = "${workspace}\Staging\Build\DetectiveSpecs-v${version}.zip"

# Remove any existing zip file at the destination path
if (Test-Path $zipFilePath) {
    Remove-Item -Path $zipFilePath
}

# Create new compressed zip file
Compress-Archive -Path $exeFilePath -DestinationPath $zipFilePath

# Define GitHub API url
$assetFileName = [System.IO.Path]::GetFileName($zipFilePath)
$uri = "https://uploads.github.com/repos/EivindAntonsen/DetectiveSpecs/releases/${releaseId}/assets?name=${assetFileName}"

# Define headers for API request
$headers = @{
    "Accept" = "application/vnd.github+json"
    "Authorization" = "Bearer $accessToken"
    "Content-Type" = "application/octet-stream"
}

# Read file data as bytes
$fileContent = [System.IO.File]::ReadAllBytes($zipFilePath)

# Make API request to upload file
Invoke-RestMethod -Method 'POST' -Uri $uri -Headers $headers -Body $fileContent