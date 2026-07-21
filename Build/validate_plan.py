#!/usr/bin/env python3
"""Validate the vibe-first full roadmap, prompt registry, and vision package."""
from __future__ import annotations

import hashlib
import json
import re
import sys
import zipfile
from pathlib import Path

from validate_docs import validate_repository

ROOT = Path(__file__).resolve().parents[1]
REGISTRY = ROOT / "Config" / "task-registry.json"
BLUEPRINT = ROOT / "docs" / "blueprint" / "Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf"
PLAN_ROOT = ROOT / "docs" / "plans"
ROADMAP_ZIP = PLAN_ROOT / "Civilization_Sandbox_Vibe_First_Full_Roadmap_Kit.zip"
REQUIRED_PLAN_FILES = [
    "README.md",
    "FULL_ROADMAP.md",
    "VISION_AUTHORITY.md",
    "SCOPE_COVERAGE.md",
    "VIBE_WORKFLOW.md",
    "DECISION_GATES.md",
    "SOURCE_AND_CHANGE_NOTES.md",
    "codex.md",
    "tasks/README.md",
    "tasks/codex.md",
]


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def main() -> int:
    errors = [f"Documentation: {item}" for item in validate_repository(ROOT, check_manifest=False)]

    try:
        registry = json.loads(REGISTRY.read_text(encoding="utf-8"))
    except Exception as exc:
        registry = {}
        errors.append(f"Invalid Config/task-registry.json: {exc}")

    if registry.get("schema_version") != 3:
        errors.append("task-registry schema_version must equal 3")
    if registry.get("roadmap_revision") != "vibe-first-full-roadmap-1":
        errors.append("task-registry roadmap_revision is not the vibe-first full roadmap")

    expected_ids = [f"BUILD-{index:02d}" for index in range(21)]
    builds = registry.get("builds", [])
    actual_ids = [item.get("id") for item in builds if isinstance(item, dict)]
    if actual_ids != expected_ids:
        errors.append(f"Build registry must contain ordered BUILD-00 through BUILD-20; found {actual_ids}")

    prompt_paths: list[Path] = []
    for index, build in enumerate(builds):
        if not isinstance(build, dict):
            errors.append(f"Build entry {index} is not an object")
            continue
        path_value = build.get("path")
        path = ROOT / path_value if isinstance(path_value, str) else ROOT / "__missing__"
        if not path.is_file():
            errors.append(f"{build.get('id', index)} prompt is missing: {path_value}")
            continue
        prompt_paths.append(path)
        text = path.read_text(encoding="utf-8")
        if not re.search(rf"^# Prompt - Build {index:02d}\b", text, re.MULTILINE):
            errors.append(f"{build.get('id')} heading does not identify Build {index:02d}: {path_value}")
        for phrase in ("Player outcome:", "Acceptance:"):
            if phrase.lower() not in text.lower():
                errors.append(f"{build.get('id')} prompt lacks {phrase} {path_value}")

    disk_prompts = sorted((PLAN_ROOT / "tasks").glob("BUILD-*.md"))
    if len(disk_prompts) != 21 or set(disk_prompts) != set(prompt_paths):
        errors.append("Exactly the 21 registered BUILD prompts must exist in docs/plans/tasks")

    for relative in REQUIRED_PLAN_FILES:
        if not (PLAN_ROOT / relative).is_file():
            errors.append(f"Missing roadmap file: docs/plans/{relative}")

    full_roadmap = PLAN_ROOT / "FULL_ROADMAP.md"
    if full_roadmap.is_file():
        text = full_roadmap.read_text(encoding="utf-8")
        for index in range(21):
            if not re.search(rf"^## Build {index:02d}\b", text, re.MULTILINE):
                errors.append(f"FULL_ROADMAP.md is missing Build {index:02d}")

    expected_blueprint_hash = str(registry.get("blueprint_sha256", "")).lower()
    if not BLUEPRINT.is_file():
        errors.append("Blueprint PDF is missing")
    elif sha256(BLUEPRINT) != expected_blueprint_hash:
        errors.append("Blueprint PDF hash differs from the registered immutable source")

    if not ROADMAP_ZIP.is_file():
        errors.append("Downloadable roadmap ZIP is missing")
    else:
        try:
            with zipfile.ZipFile(ROADMAP_ZIP) as archive:
                names = [name.replace("\\", "/") for name in archive.namelist()]
                zip_prompts = [name for name in names if re.search(r"/tasks/BUILD-\d{2}_.+\.md$", name)]
                if len(zip_prompts) != 21:
                    errors.append(f"Roadmap ZIP must contain 21 BUILD prompts; found {len(zip_prompts)}")
                blueprint_names = [name for name in names if name.endswith("/blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf")]
                if len(blueprint_names) != 1:
                    errors.append("Roadmap ZIP must contain exactly one Blueprint PDF")
                elif hashlib.sha256(archive.read(blueprint_names[0])).hexdigest() != expected_blueprint_hash:
                    errors.append("Blueprint inside roadmap ZIP differs from the immutable source")
        except Exception as exc:
            errors.append(f"Roadmap ZIP is unreadable: {exc}")

    print(f"Roadmap revision: {registry.get('roadmap_revision', 'Unspecified')}")
    print(f"Playable build prompts: {len(builds)}")
    if errors:
        print("\nVALIDATION FAILED")
        for error in sorted(set(errors)):
            print(f"- {error}")
        return 1
    print("\nVALIDATION PASSED")
    return 0


if __name__ == "__main__":
    sys.exit(main())
