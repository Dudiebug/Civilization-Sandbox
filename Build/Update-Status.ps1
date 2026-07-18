$ErrorActionPreference = 'Stop'
python (Join-Path $PSScriptRoot 'update_status.py')
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
