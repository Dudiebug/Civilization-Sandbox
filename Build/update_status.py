#!/usr/bin/env python3
"""Regenerate the milestone-aware status board from task-contract status fields."""
from __future__ import annotations

import json
import re
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
REGISTRY_DOC = json.loads((ROOT / "Config" / "task-registry.json").read_text(encoding="utf-8"))
TASKS = REGISTRY_DOC.get("tasks", [])
MILESTONES = REGISTRY_DOC.get("milestones", [])


def read_field(path: Path, field: str, default: str = "Missing") -> str:
    text = path.read_text(encoding="utf-8")
    match = re.search(rf"^\*\*{re.escape(field)}:\*\*\s*(.+?)\s*$", text, re.MULTILINE)
    return match.group(1).strip() if match else default


rows: list[str] = []
counts: dict[str, Counter] = {m["id"]: Counter() for m in MILESTONES}
for task in TASKS:
    path = ROOT / task["path"]
    status = read_field(path, "Status")
    task["status"] = status
    counts.setdefault(task["milestone"], Counter())[status] += 1
    label = f"**{task['id']}** — {task['title']}"
    if status == "Done":
        rows.append(f"- [x] ~~{label}~~ — Milestone {task['milestone']}")
    elif status == "In Progress":
        rows.append(f"- [~] {label} — **IN PROGRESS** — Milestone {task['milestone']}")
    elif status == "Blocked":
        rows.append(f"- [!] {label} — **BLOCKED** — Milestone {task['milestone']}")
    elif status == "In Review":
        rows.append(f"- [?] {label} — **IN REVIEW** — Milestone {task['milestone']}")
    else:
        rows.append(f"- [ ] {label} — Milestone {task['milestone']}")

active = next((t for t in TASKS if t["status"] == "In Progress"), None)
if active is None:
    active = next((t for t in TASKS if t["status"] == "In Review"), None)
if active is None:
    active = next((t for t in TASKS if t["status"] not in {"Done", "Blocked"}), None)
if active is None:
    active = next((t for t in TASKS if t["status"] == "Blocked"), None)

done = sum(1 for t in TASKS if t["status"] == "Done")
stamp = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M UTC")
summary_lines: list[str] = []
for milestone in MILESTONES:
    mid = milestone["id"]
    c = counts.get(mid, Counter())
    total = sum(c.values())
    if total == 0:
        contract_state = "Deferred until prior gate"
    elif c.get("Done", 0) == total:
        contract_state = "All contracts done; gate still required"
    elif c.get("In Progress", 0) or c.get("In Review", 0):
        contract_state = "Active"
    elif c.get("Blocked", 0):
        contract_state = "Blocked"
    else:
        contract_state = "Defined; not started"
    summary_lines.append(
        f"| {mid} | {milestone['title']} | {c.get('Done', 0)} | "
        f"{c.get('In Progress', 0)} | {c.get('In Review', 0)} | "
        f"{c.get('Blocked', 0)} | {total} | {contract_state} |"
    )

text = f"""# Status Board

**Generated:** {stamp}
**Roadmap revision:** {REGISTRY_DOC.get('roadmap_revision', 'Unspecified')}
**Active implementation task:** {active['id'] if active else 'None'}
**Completed implementation tasks:** {done} / {len(TASKS)}
**Current milestone:** {active['milestone'] if active else 'None'}

## Repository-control setup
- [x] ~~Blueprint copied into the repository kit unchanged.~~
- [x] ~~Scoped `codex.md` suite created; no `AGENTS.md` file.~~
- [x] ~~Evidence-gated task, review, prompt, and status controls created.~~
- [x] ~~Release plan rebaselined: lean Version 1.0, original complete early-modern target at Version 1.5.~~
- [x] ~~Milestone-timed decision queue created to avoid premature product choices.~~

## Pre-1.0 milestone summary
| Milestone | Working name | Done | Active | Review | Blocked | Contracts | Contract state |
|---:|---|---:|---:|---:|---:|---:|---|
{chr(10).join(summary_lines)}

## Currently defined implementation contracts
The first 25 contracts cover Milestones 0.1-0.3. Milestones 0.4-0.9 receive detailed contracts only after the prior gate and required creator decisions pass.

{chr(10).join(rows)}

## Status notation
- `[ ]` Not started
- `[~]` In progress
- `[?]` In review
- `[!]` Blocked
- `[x] ~~Done~~` only after evidence, independent review, and creator acceptance

## Rule
Run `python Build/validate_plan.py` before committing any Done status. A milestone is not complete merely because all of its task contracts are Done; its integrated gate review must also be approved.
"""
(ROOT / "docs" / "plans" / "STATUS_BOARD.md").write_text(text, encoding="utf-8")
print(
    f"Updated STATUS_BOARD.md; active={active['id'] if active else 'None'}, "
    f"milestone={active['milestone'] if active else 'None'}, done={done}/{len(TASKS)}"
)
