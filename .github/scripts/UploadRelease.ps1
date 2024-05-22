param(
    [String]$accessToken,
    [String]$workspace,
    [String]$releaseId,
    [String]$version
)

$exeFilePath = "${workspace}\Staging\Build\DetectiveSpecs-v${version}.exe"
$zipFilePath = "${workspace}\Staging\Build\DetectiveSpecs.zip"

if (Test-Path $zipFilePath) {
    Remove-Item -Path $zipFilePath
}

Compress-Archive -Path $exeFilePath -DestinationPath $zipFilePath

$assetFileName = [System.IO.Path]::GetFileName($zipFilePath)
$uri = "https://uploads.github.com/repos/EivindAntonsen/DetectiveSpecs/releases/${releaseId}/assets?name=${assetFileName}"

$headers = @{
    "Accept" = "application/vnd.github+json"
    "Authorization" = "Bearer $accessToken"
    "Content-Type" = "application/octet-stream"
}

$fileContent = [System.IO.File]::ReadAllBytes($zipFilePath)

Invoke-RestMethod -Method 'POST' -Uri $uri -Headers $headers -Body $fileContent