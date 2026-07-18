#!/usr/bin/env python3
"""Validate evidence-gated task state and repository-control invariants."""
from __future__ import annotations

import json
import re
import sys
import tomllib
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
REGISTRY_PATH = ROOT / "Config" / "task-registry.json"
VALID_STATUSES = {"Not Started", "In Progress", "Blocked", "In Review", "Done"}
REQUIRED_EVIDENCE = {
    "Build": {"PASS", "N/A"},
    "Tests": {"PASS", "N/A"},
    "Replay/state diff": {"PASS", "N/A"},
    "Persistence/migration": {"PASS", "N/A"},
    "Performance": {"PASS", "N/A"},
    "Documentation": {"PASS"},
    "Independent review": {"PASS"},
}


def status_from(path: Path) -> str | None:
    text = path.read_text(encoding="utf-8")
    match = re.search(r"^\*\*Status:\*\*\s*(.+?)\s*$", text, re.MULTILINE)
    return match.group(1).strip() if match else None


def evidence_value(text: str, label: str) -> str | None:
    pattern = rf"^\s*[-*]?\s*{re.escape(label)}:\s*(PASS|FAIL|N/A|CHANGES REQUIRED|PENDING)\b"
    match = re.search(pattern, text, re.IGNORECASE | re.MULTILINE)
    return match.group(1).upper() if match else None


def main() -> int:
    errors: list[str] = []
    warnings: list[str] = []

    if not REGISTRY_PATH.exists():
        errors.append("Missing Config/task-registry.json")
        registry = {"tasks": []}
    else:
        registry = json.loads(REGISTRY_PATH.read_text(encoding="utf-8"))

    tasks = registry.get("tasks", [])
    if len(tasks) != 25:
        errors.append(f"Expected 25 registered first tasks; found {len(tasks)}")

    ids = [t.get("id") for t in tasks]
    expected_ids = [f"TASK-{i:03d}" for i in range(1, 26)]
    if ids != expected_ids:
        errors.append("Task registry IDs/order do not equal TASK-001 through TASK-025")

    status_map: dict[str, str] = {}
    for task in tasks:
        tid = task["id"]
        path = ROOT / task["path"]
        if not path.exists():
            errors.append(f"{tid}: missing task file {task['path']}")
            continue
        status = status_from(path)
        if status is None:
            errors.append(f"{tid}: missing **Status:** line")
            continue
        status_map[tid] = status
        if status not in VALID_STATUSES:
            errors.append(f"{tid}: invalid status {status!r}")

    for task in tasks:
        tid = task["id"]
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
        if not re.search(r"^\s*\*\*?Decision:\*\*?\s*APPROVE\b|^\s*Decision:\s*APPROVE\b", acceptance, re.IGNORECASE | re.MULTILINE):
            errors.append(f"{tid}: creator acceptance does not contain Decision: APPROVE")

    # User explicitly requested no AGENTS.md. Check case-insensitively.
    forbidden = [p for p in ROOT.rglob("*") if p.is_file() and p.name.lower() == "agents.md"]
    if forbidden:
        errors.extend(f"Forbidden file exists: {p.relative_to(ROOT)}" for p in forbidden)

    codex_files = sorted(ROOT.rglob("codex.md"))
    if len(codex_files) < 20:
        errors.append(f"Expected a scoped codex.md suite; found only {len(codex_files)}")

    config_path = ROOT / ".codex" / "config.toml"
    if not config_path.exists():
        errors.append("Missing .codex/config.toml")
    else:
        try:
            config = tomllib.loads(config_path.read_text(encoding="utf-8"))
        except Exception as exc:  # pragma: no cover - diagnostic path
            errors.append(f"Invalid TOML in .codex/config.toml: {exc}")
        else:
            if "codex.md" not in config.get("project_doc_fallback_filenames", []):
                errors.append(".codex/config.toml does not set codex.md as a project-document fallback")
            if config.get("sandbox_mode") != "workspace-write":
                warnings.append("sandbox_mode is not workspace-write")
            if config.get("sandbox_workspace_write", {}).get("network_access") is not False:
                warnings.append("workspace-write network access is not explicitly false")

    required_paths = [
        "START_HERE.md",
        "codex.md",
        "docs/blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf",
        "docs/creator/MODEL_AND_PROMPT_GUIDE.md",
        "docs/plans/MASTER_PLAN.md",
        "docs/plans/STATUS_BOARD.md",
        "docs/plans/CURRENT_STEP.md",
        "docs/core/TECHNICAL_ARCHITECTURE.md",
        "docs/ai/DECISION_LAB.md",
        "docs/templates/EVIDENCE_TEMPLATE.md",
    ]
    for rel in required_paths:
        if not (ROOT / rel).exists():
            errors.append(f"Missing required file: {rel}")

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
