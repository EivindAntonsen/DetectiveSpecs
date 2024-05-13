param (
    [string]$project,
    [string]$version,
    [string]$workspacePath
)

$filename = "DetectiveSpecs-v$version"
$output = "$workspacePath\Staging\Build\$filename"
dotnet publish $project -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true --output $output