$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
python (Join-Path $PSScriptRoot 'validate_plan.py')
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
