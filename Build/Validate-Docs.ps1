$ErrorActionPreference = 'Stop'
python (Join-Path $PSScriptRoot 'validate_docs.py')
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
