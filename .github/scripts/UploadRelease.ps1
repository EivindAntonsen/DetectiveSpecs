param(
    [String]$accessToken,
    [String]$workspace,
    [String]$repository,
    [String]$releaseId
)

Start-Sleep -Seconds 10

$dirPath = "${workspace}/Staging/Build/"
$zipFilePath = "${workspace}/Staging/Build/DetectiveSpecs.exe"

[System.IO.Compression.ZipFile]::CreateFromDirectory($dirPath, $zipFilePath)

$assetFileName = [System.IO.Path]::GetFileName($zipFilePath)

$uri = "https://uploads.github.com/repos/${repository}/releases/${releaseId}/assets?name=${assetFileName}"

$headers = @{
    "Accept" = "application/vnd.github+json"
    "Authorization" = "Bearer $accessToken"
    "X-GitHub-Api-Version" = "2022-11-28"
    "Content-Type" = "application/octet-stream"
}

$fileBytes = [System.IO.File]::ReadAllBytes($assetFilePath)

Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $fileBytes