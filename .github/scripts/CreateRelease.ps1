param(
    [String]$accessToken,
    [String]$version
)

$body = @{
    tag_name = "v" + $version;
    target_commitish = "master";
    draft = $false;
    prerelease = $false;
    generate_release_notes = $true;
    make_latest = $true
} | ConvertTo-Json

$uri = "https://api.github.com/repos/EivindAntonsen/DetectiveSpecs/releases"
$accept = "application/vnd.github+json"

$headers = @{
    "Accept" = $accept; 
    "Authorization" = "Bearer $accessToken"; 
    "X-GitHub-Api-Version" = "2022-11-28"
}

Write-Host $uri
Write-Host $body
Write-Host $headers

Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body