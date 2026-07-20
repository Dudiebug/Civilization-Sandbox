# Current Step

**Active task:** TASK-001 — Repository governance and reproducible Windows bootstrap  
**Status:** In Progress
**Phase:** 0  
**Model:** Implementation and verification role

## One action
Run `gh auth login`. After the unrelated Windows Installer session is clear, rerun `powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1 -InstallPrerequisites`; then resume the Linux build and live governance audit. Keep acceptance and recovery gates pending until independently observed.

## Do not do yet
Do not create terrain, citizens, buildings, gameplay AI, art, persistence, replay, or player-facing APIs.

## Success for this step
Local governance and Unity checks pass with retained evidence; remote protection, clean-profile reproduction, independent review, and creator acceptance are explicitly recorded.
