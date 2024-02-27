param(
    [String]$accessToken,
    [String]$workspace,
    [String]$releaseId,
    [String]$version
)

Add-Type -Assembly System.IO.Compression.FileSystem

$exeFilePath = "${workspace}\Staging\Build\DetectiveSpecs-v${version}.exe"
$zipFilePath = "${workspace}\Staging\Build\DetectiveSpecs-v${version}.zip"
if (Test-Path $zipFilePath){ Remove-Item $zipFilePath }
$compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
$includeBaseDirectory = $false
$zip = [System.IO.Compression.ZipFile]::Open($zipFilePath, 'Update')
$relativePath = [System.IO.Path]::GetRelativePath((Get-Location).Path, $exeFilePath)
$entry = $zip.CreateEntryFromFile($exeFilePath, $relativePath, $compressionLevel)
$zip.Dispose()

$assetFileName = [System.IO.Path]::GetFileName($zipFilePath)
$uri = "https://uploads.github.com/repos/EivindAntonsen/DetectiveSpecs/releases/${releaseId}/assets?name=${assetFileName}"
  
$headers = @{
    "Accept" = "application/vnd.github+json"
    "Authorization" = "Bearer $accessToken"
    "X-GitHub-Api-Version" = "2022-11-28"
    "Content-Type" = "application/octet-stream"
}

$fileBytes = [System.IO.File]::ReadAllBytes($zipFilePath)
Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $fileBytes
