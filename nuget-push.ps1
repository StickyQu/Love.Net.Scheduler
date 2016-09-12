dotnet restore

dotnet build .\src\Scheduler

#dotnet test .\CronExpressionDescriptor.Test

dotnet pack .\src\Scheduler

$project = Get-Content ".\src\Scheduler\project.json" | ConvertFrom-Json
$version = $project.version.Trim("-*")
nuget push .\src\Scheduler\bin\Debug\Love.Net.Scheduler.$version.nupkg -source nuget -apikey $env:NUGET_API_KEY
