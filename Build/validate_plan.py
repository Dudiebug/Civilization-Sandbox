#!/usr/bin/env python3
"""Validate milestone-aware roadmap structure, evidence-gated task state, and kit invariants."""
from __future__ import annotations

import hashlib
import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
REGISTRY_PATH = ROOT / "Config" / "task-registry.json"
VALID_STATUSES = {"Not Started", "In Progress", "Blocked", "In Review", "Done"}
EXPECTED_MILESTONES = [f"0.{i}" for i in range(1, 10)]
BLUEPRINT_SHA256 = "6c71f41ad6f4cecbfd1aaa0c3c0474d6b4be4d63dd9a418a28174d9f2fa0ac6e"
CHECKOUT_ACTION = "actions/checkout@08c6903cd8c0fde910a37f88322edcfb5dd907a8"
POLICY_JOB = "repository-policy"
REQUIRED_EVIDENCE = {
    "Build": {"PASS", "N/A"},
    "Tests": {"PASS", "N/A"},
    "Replay/state diff": {"PASS", "N/A"},
    "Persistence/migration": {"PASS", "N/A"},
    "Performance": {"PASS", "N/A"},
    "Documentation": {"PASS"},
    "Independent review": {"PASS"},
}
IGNORED_TOP_LEVEL = {
    ".git", ".idea", ".vs", "Artifacts", "BuildOutput", "Builds", "CoverageResults",
    "Library", "Logs", "MemoryCaptures", "Obj", "Recordings", "Temp", "TestResults", "UserSettings",
}
IGNORED_SUFFIXES = {".booproj", ".csproj", ".mdb", ".opendb", ".pdb", ".pidb", ".pyc", ".sln", ".suo", ".svd", ".user", ".userprefs"}


def field_from(path: Path, field: str) -> str | None:
    text = path.read_text(encoding="utf-8")
    match = re.search(rf"^\*\*{re.escape(field)}:\*\*\s*(.+?)\s*$", text, re.MULTILINE)
    return match.group(1).strip() if match else None


def heading_from(path: Path) -> str | None:
    text = path.read_text(encoding="utf-8")
    match = re.search(r"^#\s+(.+?)\s*$", text, re.MULTILINE)
    return match.group(1).strip() if match else None


def evidence_value(text: str, label: str) -> str | None:
    pattern = rf"^\s*[-*]?\s*{re.escape(label)}:\s*(PASS|FAIL|N/A|CHANGES REQUIRED|PENDING)\b"
    match = re.search(pattern, text, re.IGNORECASE | re.MULTILINE)
    return match.group(1).upper() if match else None


def repository_files() -> list[str]:
    files: list[str] = []
    for path in ROOT.rglob("*"):
        if not path.is_file():
            continue
        relative = path.relative_to(ROOT)
        if relative.parts[0] in IGNORED_TOP_LEVEL:
            continue
        if "__pycache__" in relative.parts:
            continue
        if path.suffix.lower() in IGNORED_SUFFIXES:
            continue
        if path.name in {".DS_Store", "Thumbs.db"} or path.name.startswith(".env") or path.name.endswith(".local.toml"):
            continue
        if relative.parts[:1] == (".vscode",) and path.suffix.lower() == ".log":
            continue
        files.append(relative.as_posix())
    return sorted(files)


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as fh:
        for chunk in iter(lambda: fh.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def main() -> int:
    errors: list[str] = []
    warnings: list[str] = []

    if not REGISTRY_PATH.exists():
        errors.append("Missing Config/task-registry.json")
        registry: dict = {"tasks": [], "milestones": []}
    else:
        try:
            registry = json.loads(REGISTRY_PATH.read_text(encoding="utf-8"))
        except Exception as exc:
            errors.append(f"Invalid JSON in Config/task-registry.json: {exc}")
            registry = {"tasks": [], "milestones": []}

    if registry.get("schema_version", 0) < 2:
        errors.append("task-registry schema_version must be at least 2")
    if not registry.get("roadmap_revision"):
        errors.append("task-registry must declare roadmap_revision")

    milestones = registry.get("milestones", [])
    milestone_ids = [m.get("id") for m in milestones]
    if milestone_ids != EXPECTED_MILESTONES:
        errors.append(f"Milestone IDs/order must be {EXPECTED_MILESTONES}; found {milestone_ids}")
    if len(set(milestone_ids)) != len(milestone_ids):
        errors.append("Duplicate milestone IDs in task registry")
    milestone_map = {m.get("id"): m for m in milestones}
    for milestone in milestones:
        mid = milestone.get("id")
        plan_rel = milestone.get("plan")
        if not plan_rel:
            errors.append(f"Milestone {mid}: missing plan path")
            continue
        plan = ROOT / plan_rel
        if not plan.exists():
            errors.append(f"Milestone {mid}: missing plan {plan_rel}")
            continue
        heading = heading_from(plan) or ""
        if f"Milestone {mid}" not in heading:
            errors.append(f"Milestone {mid}: plan heading does not identify the milestone: {heading!r}")

    tasks = registry.get("tasks", [])
    if len(tasks) != 25:
        errors.append(f"Expected 25 registered first tasks; found {len(tasks)}")

    ids = [t.get("id") for t in tasks]
    expected_ids = [f"TASK-{i:03d}" for i in range(1, 26)]
    if ids != expected_ids:
        errors.append("Task registry IDs/order do not equal TASK-001 through TASK-025")

    status_map: dict[str, str] = {}
    for index, task in enumerate(tasks, start=1):
        tid = task.get("id", f"TASK-?{index}")
        path_rel = task.get("path")
        if not path_rel:
            errors.append(f"{tid}: missing task path")
            continue
        path = ROOT / path_rel
        if not path.exists():
            errors.append(f"{tid}: missing task file {path_rel}")
            continue

        expected_milestone = "0.1" if index <= 10 else "0.2" if index <= 20 else "0.3"
        milestone = task.get("milestone")
        if milestone != expected_milestone:
            errors.append(f"{tid}: expected milestone {expected_milestone}; found {milestone!r}")
        if milestone not in milestone_map:
            errors.append(f"{tid}: unknown milestone {milestone!r}")

        expected_legacy_phase = int(expected_milestone[-1]) - 1
        if task.get("phase") != expected_legacy_phase:
            warnings.append(
                f"{tid}: legacy phase field {task.get('phase')!r} does not match expected {expected_legacy_phase}"
            )

        heading = heading_from(path)
        expected_heading = f"{tid} - {task.get('title')}"
        if heading != expected_heading:
            errors.append(f"{tid}: heading {heading!r} does not match registry {expected_heading!r}")

        status = field_from(path, "Status")
        if status is None:
            errors.append(f"{tid}: missing **Status:** line")
        else:
            status_map[tid] = status
            if status not in VALID_STATUSES:
                errors.append(f"{tid}: invalid status {status!r}")

        milestone_field = field_from(path, "Milestone")
        if milestone_field is None or not milestone_field.startswith(f"{milestone} "):
            errors.append(f"{tid}: **Milestone:** must begin with {milestone!r}; found {milestone_field!r}")
        if field_from(path, "Release horizon") is None:
            errors.append(f"{tid}: missing **Release horizon:** line")
        if field_from(path, "Decision dependencies") is None:
            errors.append(f"{tid}: missing **Decision dependencies:** line")
        if re.search(r"^\*\*Phase:\*\*", path.read_text(encoding="utf-8"), re.MULTILINE):
            errors.append(f"{tid}: obsolete **Phase:** metadata remains")

        for dep in task.get("depends_on", []):
            if dep not in expected_ids:
                errors.append(f"{tid}: unknown dependency {dep}")
            elif expected_ids.index(dep) >= expected_ids.index(tid):
                errors.append(f"{tid}: dependency {dep} is not earlier in the ordered first-task sequence")

    for task in tasks:
        tid = task.get("id")
        status = status_map.get(tid)
        if status != "Done":
            continue

        incomplete_deps = [d for d in task.get("depends_on", []) if status_map.get(d) != "Done"]
        if incomplete_deps:
            errors.append(f"{tid}: marked Done before dependencies: {', '.join(incomplete_deps)}")

        evidence_dir = ROOT / "docs" / "evidence" / tid
        evidence_path = evidence_dir / "EVIDENCE.md"
        acceptance_path = evidence_dir / "ACCEPTANCE.md"
        if not evidence_path.exists():
            errors.append(f"{tid}: Done but missing {evidence_path.relative_to(ROOT)}")
            continue
        if not acceptance_path.exists():
            errors.append(f"{tid}: Done but missing {acceptance_path.relative_to(ROOT)}")
            continue

        evidence = evidence_path.read_text(encoding="utf-8")
        for label, allowed in REQUIRED_EVIDENCE.items():
            value = evidence_value(evidence, label)
            if value not in allowed:
                errors.append(f"{tid}: evidence {label!r} must be one of {sorted(allowed)}; found {value!r}")

        acceptance = acceptance_path.read_text(encoding="utf-8")
        if not re.search(
            r"^\s*\*\*?Decision:\*\*?\s*APPROVE\b|^\s*Decision:\s*APPROVE\b",
            acceptance,
            re.IGNORECASE | re.MULTILINE,
        ):
            errors.append(f"{tid}: creator acceptance does not contain Decision: APPROVE")

    # User explicitly requested no AGENTS.md. Check case-insensitively.
    forbidden = [p for p in ROOT.rglob("*") if p.is_file() and p.name.lower() == "agents.md"]
    if forbidden:
        errors.extend(f"Forbidden file exists: {p.relative_to(ROOT)}" for p in forbidden)

    codex_files = sorted(ROOT.rglob("codex.md"))
    if len(codex_files) < 20:
        errors.append(f"Expected a scoped codex.md suite; found only {len(codex_files)}")

    if (ROOT / ".codex" / "config.toml").exists():
        errors.append(".codex/config.toml must remain absent; scoped codex.md files are the instruction authority")

    manifest_path = ROOT / "FILE_MANIFEST.md"
    if not manifest_path.exists():
        errors.append("Missing FILE_MANIFEST.md")
    else:
        declared = sorted(re.findall(r"^- `([^`]+)`$", manifest_path.read_text(encoding="utf-8"), re.MULTILINE))
        actual = repository_files()
        if declared != actual:
            missing = sorted(set(actual) - set(declared))
            stale = sorted(set(declared) - set(actual))
            errors.append(f"FILE_MANIFEST.md is stale; missing={missing}, stale={stale}")

    required_paths = [
        "README.md",
        "START_HERE.md",
        "REVISION_SUMMARY.md",
        "codex.md",
        "docs/blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf",
        "docs/creator/MODEL_AND_PROMPT_GUIDE.md",
        "docs/decisions/ADR-001_RELEASE_SCOPE_REBASELINE.md",
        "docs/plans/MASTER_PLAN.md",
        "docs/plans/PRE_1_0_ROADMAP.md",
        "docs/plans/RELEASE_LADDER.md",
        "docs/plans/DECISION_QUEUE.md",
        "docs/plans/IDENTITY_GUARDRAILS.md",
        "docs/plans/STATUS_BOARD.md",
        "docs/plans/CURRENT_STEP.md",
        "docs/plans/PHASE_8.md",
        "docs/plans/VERSION_1_PRODUCTION.md",
        "docs/plans/VERSION_1_5_PRODUCTION.md",
        "docs/core/TECHNICAL_ARCHITECTURE.md",
        "docs/ai/DECISION_LAB.md",
        "docs/templates/EVIDENCE_TEMPLATE.md",
        "Config/repository-governance.json",
        "Config/toolchain.json",
        "docs/core/REPOSITORY_GOVERNANCE.md",
        "docs/templates/MILESTONE_GATE_TEMPLATE.md",
        "docs/prompts/10_PLAN_MILESTONE.md",
        "docs/evidence/ROADMAP-REBASELINE/EVIDENCE.md",
    ]
    for rel in required_paths:
        if not (ROOT / rel).exists():
            errors.append(f"Missing required file: {rel}")

    toolchain_path = ROOT / "Config" / "toolchain.json"
    package_manifest_path = ROOT / "Packages" / "manifest.json"
    package_lock_path = ROOT / "Packages" / "packages-lock.json"
    try:
        toolchain = json.loads(toolchain_path.read_text(encoding="utf-8"))
        package_manifest = json.loads(package_manifest_path.read_text(encoding="utf-8"))
        package_lock = json.loads(package_lock_path.read_text(encoding="utf-8"))
        expected_packages = toolchain.get("packages", {})
        actual_packages = package_manifest.get("dependencies", {})
        if actual_packages != expected_packages:
            errors.append("Packages/manifest.json dependencies do not exactly match Config/toolchain.json packages")
        if package_manifest.get("testables") != ["com.civsandbox.tooling"]:
            errors.append("Packages/manifest.json testables must contain only com.civsandbox.tooling")
        locked = package_lock.get("dependencies", {})
        for package_name, expected_version in expected_packages.items():
            record = locked.get(package_name)
            if not isinstance(record, dict):
                errors.append(f"Packages/packages-lock.json is missing direct package {package_name}")
                continue
            if record.get("depth") != 0 or record.get("version") != expected_version:
                errors.append(
                    f"Packages/packages-lock.json direct package {package_name} must be depth 0 at {expected_version}"
                )
        expected_lock_hash = str(toolchain.get("unity", {}).get("packageLockSha256", "")).lower()
        if not re.fullmatch(r"[0-9a-f]{64}", expected_lock_hash) or sha256(package_lock_path) != expected_lock_hash:
            errors.append("Packages/packages-lock.json SHA-256 does not match Config/toolchain.json")
    except Exception as exc:
        errors.append(f"Toolchain/package contract is unreadable: {exc}")

    governance_path = ROOT / "Config" / "repository-governance.json"
    workflow_path = ROOT / ".github" / "workflows" / "repository-policy.yml"
    try:
        governance = json.loads(governance_path.read_text(encoding="utf-8"))
        workflow = governance.get("workflow", {})
        protection = governance.get("protection", {})
        if governance.get("defaultBranch") != "main" or governance.get("visibility") not in {"private", "public"}:
            errors.append("Repository governance must declare supported visibility and main as default")
        if not protection.get("requirePullRequest") or not protection.get("enforceAdmins"):
            errors.append("Repository governance must require pull requests and administrator enforcement")
        if protection.get("allowForcePushes") or protection.get("allowDeletions"):
            errors.append("Repository governance must prohibit force-pushes and branch deletion")
        if not protection.get("requireBranchesUpToDate") or protection.get("requireStatusCheckAfterFirstRun") != POLICY_JOB:
            errors.append("Repository governance must require strict repository-policy status checks")
        if workflow.get("path") != ".github/workflows/repository-policy.yml":
            errors.append("Repository governance workflow path changed")
        if workflow.get("jobName") != POLICY_JOB or workflow.get("checkoutAction") != CHECKOUT_ACTION:
            errors.append("Repository governance workflow job or checkout pin changed")
        workflow_text = workflow_path.read_text(encoding="utf-8")
        if len(re.findall(rf"^\s*uses:\s*{re.escape(CHECKOUT_ACTION)}\s*$", workflow_text, re.MULTILINE)) != 1:
            errors.append("repository-policy workflow must use the immutable checkout SHA exactly once")
        if not re.search(rf"^  {re.escape(POLICY_JOB)}:\s*$", workflow_text, re.MULTILINE):
            errors.append("repository-policy workflow job key changed")
        if not re.search(rf"^    name:\s*{re.escape(POLICY_JOB)}\s*$", workflow_text, re.MULTILINE):
            errors.append("repository-policy workflow check name changed")
        if not re.search(r"^permissions:\s*\n  contents:\s*read\s*$", workflow_text, re.MULTILINE):
            errors.append("repository-policy workflow permissions must remain contents: read")
        if re.search(r"\$\{\{\s*secrets\.", workflow_text, re.IGNORECASE):
            errors.append("repository-policy workflow must not reference secrets")
        jobs_match = re.search(r"^jobs:\s*$", workflow_text, re.MULTILINE)
        job_text = ""
        if not jobs_match:
            errors.append("repository-policy workflow jobs section is absent")
        else:
            jobs_text = workflow_text[jobs_match.end():]
            job_headers = list(re.finditer(r"^  ([A-Za-z0-9_-]+):\s*$", jobs_text, re.MULTILINE))
            if len(job_headers) != 1 or job_headers[0].group(1) != POLICY_JOB:
                errors.append("repository-policy workflow must contain exactly one job named repository-policy")
            else:
                job_text = jobs_text[job_headers[0].start():]
        required_workflow_runs = [
            "python Build/validate_plan.py",
            "powershell -NoProfile -ExecutionPolicy Bypass -File Build/Configure-Repository.ps1 -Offline",
            "powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1 -RepositoryOnly",
            "powershell -NoProfile -ExecutionPolicy Bypass -File Tests/Bootstrap/Task001.Bootstrap.Tests.ps1",
        ]
        for run in required_workflow_runs:
            if len(re.findall(rf"^\s{{8}}run:\s*{re.escape(run)}\s*$", job_text, re.MULTILINE)) != 1:
                errors.append(f"repository-policy workflow command is absent or duplicated: {run}")
        if re.search(r"^\s+(?:continue-on-error|if):", job_text, re.IGNORECASE | re.MULTILINE):
            errors.append("repository-policy job may not contain if or continue-on-error bypasses")
    except Exception as exc:
        errors.append(f"Repository governance/workflow contract is unreadable: {exc}")

    adr = ROOT / "docs" / "decisions" / "ADR-001_RELEASE_SCOPE_REBASELINE.md"
    if adr.exists() and field_from(adr, "Status") != "Accepted":
        errors.append("ADR-001 must be recorded as Accepted")

    decision_queue = ROOT / "docs" / "plans" / "DECISION_QUEUE.md"
    if decision_queue.exists():
        text = decision_queue.read_text(encoding="utf-8")
        decision_ids = re.findall(r"^\|\s*(D\d{2})\s*\|", text, re.MULTILINE)
        expected_decisions = [f"D{i:02d}" for i in range(1, 12)]
        if decision_ids != expected_decisions:
            errors.append(
                f"Decision queue must contain ordered D01-D11; found {decision_ids}"
            )

    blueprint = ROOT / "docs" / "blueprint" / "Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf"
    if blueprint.exists():
        actual_hash = sha256(blueprint)
        if actual_hash != BLUEPRINT_SHA256:
            errors.append(f"Blueprint PDF hash changed: {actual_hash}")

    status_board = ROOT / "docs" / "plans" / "STATUS_BOARD.md"
    if status_board.exists():
        board = status_board.read_text(encoding="utf-8")
        for task in tasks:
            needle = f"**{task['id']}** — {task['title']}"
            if needle not in board:
                errors.append(f"Status board is stale or missing registry title: {needle}")
        for mid in EXPECTED_MILESTONES:
            if f"| {mid} |" not in board:
                errors.append(f"Status board is missing milestone {mid}")

    print(f"Roadmap revision: {registry.get('roadmap_revision', 'Unspecified')}")
    print(f"Milestones registered: {len(milestones)}")
    print(f"Tasks registered: {len(tasks)}")
    print(f"Scoped codex.md files: {len(codex_files)}")
    print(f"Done tasks: {sum(1 for s in status_map.values() if s == 'Done')}")
    for warning in warnings:
        print(f"WARNING: {warning}")

    if errors:
        print("\nVALIDATION FAILED")
        for error in errors:
            print(f"- {error}")
        return 1

    print("\nVALIDATION PASSED")
    return 0


if __name__ == "__main__":
    sys.exit(main())
