#!/usr/bin/env python3
"""Regenerate the concise status board from task-contract status fields."""
from __future__ import annotations

import json
import re
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
REGISTRY = json.loads((ROOT / "Config" / "task-registry.json").read_text(encoding="utf-8"))["tasks"]


def read_status(path: Path) -> str:
    text = path.read_text(encoding="utf-8")
    match = re.search(r"^\*\*Status:\*\*\s*(.+?)\s*$", text, re.MULTILINE)
    return match.group(1).strip() if match else "Missing"


rows = []
phase_counts: dict[int, Counter] = {}
for task in REGISTRY:
    status = read_status(ROOT / task["path"])
    task["status"] = status
    phase_counts.setdefault(task["phase"], Counter())[status] += 1
    label = f"**{task['id']}** — {task['title']}"
    if status == "Done":
        rows.append(f"- [x] ~~{label}~~")
    elif status == "In Progress":
        rows.append(f"- [~] {label} — **IN PROGRESS**")
    elif status == "Blocked":
        rows.append(f"- [!] {label} — **BLOCKED**")
    elif status == "In Review":
        rows.append(f"- [?] {label} — **IN REVIEW**")
    else:
        rows.append(f"- [ ] {label}")

active = next((t for t in REGISTRY if t["status"] == "In Progress"), None)
if active is None:
    active = next((t for t in REGISTRY if t["status"] == "In Review"), None)
if active is None:
    active = next((t for t in REGISTRY if t["status"] not in {"Done", "Blocked"}), None)
if active is None:
    active = next((t for t in REGISTRY if t["status"] == "Blocked"), None)

done = sum(1 for t in REGISTRY if t["status"] == "Done")
stamp = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M UTC")
phase_lines = []
for phase in sorted(phase_counts):
    c = phase_counts[phase]
    phase_lines.append(
        f"| {phase} | {c.get('Done', 0)} | {c.get('In Progress', 0)} | "
        f"{c.get('In Review', 0)} | {c.get('Blocked', 0)} | {sum(c.values())} |"
    )

text = f"""# Status Board

**Generated:** {stamp}
**Active:** {active['id'] if active else 'None'}
**Completed game tasks:** {done} / {len(REGISTRY)}

## Repository-control setup
- [x] ~~Blueprint copied into repository kit.~~
- [x] ~~Scoped `codex.md` suite created; no `AGENTS.md` file.~~
- [x] ~~Master/phase/task plans, prompts, templates, and model guide created.~~

## Phase summary
| Phase | Done | Active | Review | Blocked | Total |
|---:|---:|---:|---:|---:|---:|
{chr(10).join(phase_lines)}

## First 25 implementation tasks
{chr(10).join(rows)}

## Rule
A line changes to `[x] ~~...~~` only after a complete evidence pack, independent review, and creator acceptance. Run `python Build/validate_plan.py` before committing any Done status.
"""
(ROOT / "docs" / "plans" / "STATUS_BOARD.md").write_text(text, encoding="utf-8")
print(f"Updated STATUS_BOARD.md; active={active['id'] if active else 'None'}, done={done}/{len(REGISTRY)}")
