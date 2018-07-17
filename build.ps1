#Requires -Version 3

# Process source files
$srcFiles = Get-ChildItem -Recurse .\src\* -Include *.cs*, *.addin | Where-Object {$_.FullName -NotMatch "\\obj\\"}
foreach ($file in $srcFiles) {
	$outfile = ($file | Resolve-Path -Relative).Replace('\src\', '\dist\')
	New-Item -ItemType Directory -Force -Path (Split-Path -Path $outfile -Parent) | Out-Null
	(Get-Content $file).Replace('RevitAddin', '$safeprojectname$').Replace('00000000-0000-0000-0000-00000000000', '$guid').Replace('</ClientId', '$</ClientId') | Set-Content $outfile
}

# Copy static files
$staticFiles = Get-ChildItem -Recurse .\src\* -Include *.resx*, *.png, *.vstemplate, *.ico, *.json | Where-Object {$_.FullName -NotMatch "\\obj\\"}
foreach ($file in $staticFiles) {
	$outfile = ($file | Resolve-Path -Relative).Replace('\src\', '\dist\')
	New-Item -ItemType Directory -Force -Path (Split-Path -Path $outfile -Parent) | Out-Null
	Copy-Item -Path $file -Destination $outfile
}
