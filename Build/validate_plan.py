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
REQUIRED_EVIDENCE = {
    "Build": {"PASS", "N/A"},
    "Tests": {"PASS", "N/A"},
    "Replay/state diff": {"PASS", "N/A"},
    "Persistence/migration": {"PASS", "N/A"},
    "Performance": {"PASS", "N/A"},
    "Documentation": {"PASS"},
    "Independent review": {"PASS"},
}


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

    config_path = ROOT / ".codex" / "config.toml"
    if config_path.exists():
        errors.append(".codex/config.toml is deferred and must remain absent")

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
        "docs/templates/MILESTONE_GATE_TEMPLATE.md",
        "docs/prompts/10_PLAN_MILESTONE.md",
        "docs/evidence/ROADMAP-REBASELINE/EVIDENCE.md",
    ]
    for rel in required_paths:
        if not (ROOT / rel).exists():
            errors.append(f"Missing required file: {rel}")

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
