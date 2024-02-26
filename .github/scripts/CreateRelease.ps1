param(
    [String]$accessToken,
    [String]$version
)

$body = @{
    tag_name = "v" + $version
    target_commitish = "master"
    name = $version
    body = "Latest automated release of Detective Specs"
    draft = $false
    prerelease = $false
    generate_release_notes = $false
} | ConvertTo-Json

$uri = "https://api.github.com/repos/DetectiveSpecs/releases"
$accept = "application/vnd.github+json"

Invoke-RestMethod -Uri $uri -Method Post -Headers @{"Accept" = $accept; "Authorization" = "Bearer $accessToken"; "X-GitHub-Api-Version" = "2022-11-28"} -Body $body