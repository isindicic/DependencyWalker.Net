$errorActionPreference = 'Stop'

Get-ChildItem $PSScriptRoot -Recurse -Force
| Where-Object { $_.FullName -like "*\bin\*" }
| Remove-Item -Recurse -Force

Get-Item -ErrorAction Ignore $PSScriptRoot\publish
| Remove-Item -Recurse -Force


dotnet publish ./DependencyWalker --framework net7.0-windows --configuration Release --self-contained false
if (0 -ne $LASTEXITCODE) { throw "Publish failed." }

dotnet publish ./DependencyWalker.Net.Cli --framework net7.0 --configuration Release --self-contained false
if (0 -ne $LASTEXITCODE) { throw "Publish failed." }

dotnet publish ./DependencyWalker --framework net472 --configuration Release /p:GenerateResourceUsePreserializedResources=true
if (0 -ne $LASTEXITCODE) { throw "Publish failed." }

dotnet publish ./DependencyWalker.Net.Cli --framework net472 --configuration Release
if (0 -ne $LASTEXITCODE) { throw "Publish failed." }

# Include CLI in the app package, so user gets both.
Copy-Item $PSScriptRoot\DependencyWalker.Net.Cli\bin\Release\net7.0\win10-x64\publish\ $PSScriptRoot\DependencyWalker\bin\Release\net7.0-windows\win10-x64\publish\
Copy-Item $PSScriptRoot\DependencyWalker.Net.Cli\bin\Release\net472\win10-x64\publish\ $PSScriptRoot\DependencyWalker\bin\Release\net472\win10-x64\publish\

New-Item -ItemType Directory -Path $PSScriptRoot\publish\ | Out-Null
Compress-Archive -Path $PSScriptRoot\DependencyWalker\bin\Release\net7.0-windows\win10-x64\publish\* -DestinationPath $PSScriptRoot\publish\DependencyWalker.NetCore.zip
Compress-Archive -Path $PSScriptRoot\DependencyWalker.Net.Cli\bin\Release\net7.0\win10-x64\publish\* -DestinationPath $PSScriptRoot\publish\DependencyWalker.Cli.NetCore.zip
Compress-Archive -Path $PSScriptRoot\DependencyWalker\bin\Release\net472\win10-x64\publish\* -DestinationPath $PSScriptRoot\publish\DependencyWalker.zip
Compress-Archive -Path $PSScriptRoot\DependencyWalker.Net.Cli\bin\Release\net472\win10-x64\publish\* -DestinationPath $PSScriptRoot\publish\DependencyWalker.Cli.zip