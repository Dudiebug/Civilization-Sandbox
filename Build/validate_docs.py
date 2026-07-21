#!/usr/bin/env python3
"""Validate repository documentation metadata, links, reachability, and policy."""
from __future__ import annotations

import argparse
import json
import os
import re
from collections import defaultdict, deque
from pathlib import Path
from urllib.parse import unquote, urlsplit

DEFAULT_ROOT = Path(__file__).resolve().parents[1]
REGISTRY_REL = Path("Config/document-registry.json")
INDEX_REL = Path("docs/DOCUMENT_INDEX.md")
ALLOWED_AUTHORITIES = {
    "blueprint",
    "decision",
    "evidence",
    "index",
    "instruction",
    "plan",
    "specification",
    "task-contract",
}
ALLOWED_STATUSES = {"Accepted", "Active", "Current", "Immutable", "Template"}
REQUIRED_FIELDS = {
    "id",
    "path",
    "title",
    "authority",
    "owner",
    "status",
    "release_scope",
    "required",
    "source_refs",
}
IGNORED_TOP_LEVEL = {
    ".git",
    ".idea",
    ".vs",
    "Artifacts",
    "BuildOutput",
    "Builds",
    "CoverageResults",
    "Library",
    "Logs",
    "MemoryCaptures",
    "Obj",
    "Recordings",
    "Temp",
    "TestResults",
    "UserSettings",
}
IGNORED_SUFFIXES = {
    ".booproj",
    ".csproj",
    ".mdb",
    ".opendb",
    ".pdb",
    ".pidb",
    ".pyc",
    ".sln",
    ".suo",
    ".svd",
    ".user",
    ".userprefs",
}
LINK_RE = re.compile(r"!?\[[^\]]*\]\(([^)\n]+)\)")
HEADING_RE = re.compile(r"^#{1,6}\s+(.+?)\s*#*\s*$")
ADR_FILE_RE = re.compile(r"^ADR-(\d{3})(?:[-_].*)?\.md$", re.IGNORECASE)
STATUS_RE = re.compile(r"^\*\*Status:\*\*\s*(.+?)\s*$", re.MULTILINE)


def _repo_files(root: Path) -> list[str]:
    files: list[str] = []
    for path in root.rglob("*"):
        if not path.is_file():
            continue
        rel = path.relative_to(root)
        if rel.parts and rel.parts[0] in IGNORED_TOP_LEVEL:
            continue
        if "__pycache__" in rel.parts or path.suffix.lower() in IGNORED_SUFFIXES:
            continue
        if path.name in {".DS_Store", "Thumbs.db"} or path.name.startswith(".env"):
            continue
        if path.name.endswith(".local.toml"):
            continue
        if rel.parts[:1] == (".vscode",) and path.suffix.lower() == ".log":
            continue
        files.append(rel.as_posix())
    return sorted(files)


def _markdown_files(root: Path) -> list[Path]:
    result: list[Path] = []
    for path in root.rglob("*.md"):
        rel = path.relative_to(root)
        if rel.parts and rel.parts[0] in IGNORED_TOP_LEVEL:
            continue
        if "__pycache__" not in rel.parts:
            result.append(path)
    return sorted(result)


def _link_target(raw: str) -> str:
    value = raw.strip()
    if value.startswith("<") and ">" in value:
        return value[1 : value.index(">")]
    return value.split(maxsplit=1)[0] if value else ""


def _slug(text: str) -> str:
    text = re.sub(r"<[^>]+>", "", text)
    text = re.sub(r"[`*_~]", "", text).strip().lower()
    text = re.sub(r"[^\w\- ]", "", text)
    return re.sub(r"[\s-]+", "-", text).strip("-")


def _anchors(path: Path) -> set[str]:
    anchors: set[str] = set()
    counts: dict[str, int] = defaultdict(int)
    in_fence = False
    for line in path.read_text(encoding="utf-8").splitlines():
        if line.lstrip().startswith("```"):
            in_fence = not in_fence
            continue
        if in_fence:
            continue
        match = HEADING_RE.match(line)
        if not match:
            continue
        base = _slug(match.group(1))
        if not base:
            continue
        count = counts[base]
        anchors.add(base if count == 0 else f"{base}-{count}")
        counts[base] += 1
    return anchors


def _exact_path(root: Path, relative: Path) -> tuple[Path | None, str | None]:
    current = root
    for part in relative.parts:
        if part in {"", "."}:
            continue
        if part == "..":
            return None, "outside"
        if not current.is_dir():
            return None, "missing"
        names = {child.name for child in current.iterdir()}
        if part not in names:
            if part.lower() in {name.lower() for name in names}:
                return None, "case"
            return None, "missing"
        current = current / part
    return current, None


def _resolve_link(root: Path, source: Path, target: str) -> tuple[Path | None, str, str | None]:
    parsed = urlsplit(target)
    if parsed.scheme or target.startswith("//"):
        return None, "", "external"
    fragment = unquote(parsed.fragment)
    raw_path = unquote(parsed.path)
    if not raw_path:
        return source, fragment, None
    if raw_path.startswith(("/", "\\")):
        return None, fragment, "outside"
    source_parent = source.relative_to(root).parent
    relative = Path(os.path.normpath(source_parent / Path(raw_path.replace("\\", "/"))))
    if relative.is_absolute() or relative.parts[:1] == ("..",):
        return None, fragment, "outside"
    exact, issue = _exact_path(root, relative)
    return exact, fragment, issue


def _collect_links(root: Path, errors: list[str]) -> tuple[dict[str, set[str]], dict[str, set[str]]]:
    graph: dict[str, set[str]] = defaultdict(set)
    direct: dict[str, set[str]] = defaultdict(set)
    anchor_cache: dict[Path, set[str]] = {}
    for source in _markdown_files(root):
        source_rel = source.relative_to(root).as_posix()
        text = source.read_text(encoding="utf-8")
        for match in LINK_RE.finditer(text):
            line = text.count("\n", 0, match.start()) + 1
            target = _link_target(match.group(1))
            if not target:
                errors.append(f"EMPTY_LINK {source_rel}:{line} has an empty Markdown target")
                continue
            resolved, fragment, issue = _resolve_link(root, source, target)
            if issue == "external":
                continue
            if issue == "outside":
                errors.append(f"OUTSIDE_LINK {source_rel}:{line} points outside the repository: {target}")
                continue
            if issue == "case":
                errors.append(f"PATH_CASE {source_rel}:{line} uses incorrect path casing: {target}")
                continue
            if issue == "missing" or resolved is None or not resolved.exists():
                errors.append(f"BROKEN_LINK {source_rel}:{line} target does not exist: {target}")
                continue
            target_rel = resolved.relative_to(root).as_posix()
            direct[source_rel].add(target_rel)
            graph[source_rel].add(target_rel)
            if fragment and resolved.suffix.lower() == ".md":
                available = anchor_cache.setdefault(resolved, _anchors(resolved))
                if fragment.lower() not in available:
                    errors.append(f"MISSING_ANCHOR {source_rel}:{line} target anchor does not exist: {target}")
    return graph, direct


def _load_registry(root: Path, errors: list[str]) -> dict:
    path = root / REGISTRY_REL
    if not path.exists():
        errors.append(f"MISSING_REGISTRY {REGISTRY_REL.as_posix()} does not exist")
        return {"entrypoints": [], "documents": []}
    try:
        registry = json.loads(path.read_text(encoding="utf-8"))
    except Exception as exc:
        errors.append(f"INVALID_REGISTRY {REGISTRY_REL.as_posix()} is not valid JSON: {exc}")
        return {"entrypoints": [], "documents": []}
    if registry.get("schema_version") != 1:
        errors.append("REGISTRY_SCHEMA document registry schema_version must equal 1")
    if not isinstance(registry.get("entrypoints"), list):
        errors.append("REGISTRY_ENTRYPOINTS entrypoints must be a list")
        registry["entrypoints"] = []
    if not isinstance(registry.get("documents"), list):
        errors.append("REGISTRY_DOCUMENTS documents must be a list")
        registry["documents"] = []
    return registry


def _validate_registry(root: Path, registry: dict, graph: dict[str, set[str]], direct: dict[str, set[str]], errors: list[str]) -> None:
    seen_ids: set[str] = set()
    seen_paths: set[str] = set()
    documents = registry.get("documents", [])
    entrypoints = registry.get("entrypoints", [])
    for entry in entrypoints:
        if not isinstance(entry, str):
            errors.append(f"ENTRYPOINT_TYPE entrypoint must be a string: {entry!r}")
            continue
        exact, issue = _exact_path(root, Path(entry))
        if issue or exact is None or not exact.is_file():
            errors.append(f"MISSING_ENTRYPOINT entrypoint does not exist with exact casing: {entry}")

    for index, document in enumerate(documents):
        label = document.get("id", f"documents[{index}]") if isinstance(document, dict) else f"documents[{index}]"
        if not isinstance(document, dict):
            errors.append(f"DOCUMENT_TYPE {label} must be an object")
            continue
        missing = sorted(REQUIRED_FIELDS - set(document))
        if missing:
            errors.append(f"DOCUMENT_FIELDS {label} is missing fields: {missing}")
            continue
        doc_id = document["id"]
        path_value = document["path"]
        if not all(isinstance(document[field], str) and document[field].strip() for field in ("id", "path", "title", "owner", "release_scope")):
            errors.append(f"DOCUMENT_TEXT {label} has an empty or non-string identity field")
        if doc_id in seen_ids:
            errors.append(f"DUPLICATE_DOCUMENT_ID duplicate document id: {doc_id}")
        seen_ids.add(doc_id)
        if path_value in seen_paths:
            errors.append(f"DUPLICATE_DOCUMENT_PATH duplicate document path: {path_value}")
        seen_paths.add(path_value)
        if document["authority"] not in ALLOWED_AUTHORITIES:
            errors.append(f"DOCUMENT_AUTHORITY {doc_id} has invalid authority: {document['authority']!r}")
        if document["status"] not in ALLOWED_STATUSES:
            errors.append(f"DOCUMENT_STATUS {doc_id} has invalid status: {document['status']!r}")
        if not isinstance(document["required"], bool):
            errors.append(f"DOCUMENT_REQUIRED {doc_id} required must be boolean")
        if not isinstance(document["source_refs"], list) or not document["source_refs"] or not all(
            isinstance(value, str) and value.strip() for value in document["source_refs"]
        ):
            errors.append(f"DOCUMENT_SOURCES {doc_id} source_refs must be a non-empty string list")
        exact, issue = _exact_path(root, Path(path_value))
        if issue == "case":
            errors.append(f"DOCUMENT_PATH_CASE {doc_id} uses incorrect path casing: {path_value}")
        elif issue or exact is None or not exact.is_file():
            errors.append(f"MISSING_DOCUMENT {doc_id} path does not exist: {path_value}")

    reachable: set[str] = set()
    queue: deque[str] = deque(value for value in entrypoints if isinstance(value, str))
    while queue:
        current = queue.popleft()
        if current in reachable:
            continue
        reachable.add(current)
        for target in sorted(graph.get(current, set())):
            if target not in reachable:
                queue.append(target)

    index_links = direct.get(INDEX_REL.as_posix(), set())
    for document in documents:
        if not isinstance(document, dict) or not isinstance(document.get("path"), str):
            continue
        path_value = document["path"]
        if document.get("required") and path_value not in reachable:
            errors.append(f"UNREACHABLE_DOCUMENT {document.get('id')} is not reachable from a registered entrypoint: {path_value}")
        if document.get("required") and path_value != INDEX_REL.as_posix() and path_value not in index_links:
            errors.append(f"UNINDEXED_DOCUMENT {document.get('id')} is not linked from {INDEX_REL.as_posix()}: {path_value}")


def _validate_adrs(root: Path, direct: dict[str, set[str]], errors: list[str]) -> None:
    decisions = root / "docs/decisions"
    if not decisions.is_dir():
        return
    numbers: dict[str, list[str]] = defaultdict(list)
    accepted: set[str] = set()
    for path in sorted(decisions.glob("ADR-*.md")):
        match = ADR_FILE_RE.match(path.name)
        if not match:
            errors.append(f"ADR_FILENAME invalid ADR filename: {path.relative_to(root).as_posix()}")
            continue
        rel = path.relative_to(root).as_posix()
        numbers[match.group(1)].append(rel)
        status_match = STATUS_RE.search(path.read_text(encoding="utf-8"))
        if status_match and status_match.group(1).strip().lower().startswith("accepted"):
            accepted.add(rel)
    for number, paths in sorted(numbers.items()):
        if len(paths) > 1:
            errors.append(f"DUPLICATE_ADR ADR-{number} is used by: {paths}")
    indexed = direct.get("docs/decisions/README.md", set())
    for rel in sorted(accepted - indexed):
        errors.append(f"UNINDEXED_ADR accepted ADR is not linked from docs/decisions/README.md: {rel}")


def _validate_forbidden_files(root: Path, errors: list[str]) -> None:
    for path in root.rglob("*"):
        if not path.is_file():
            continue
        rel = path.relative_to(root)
        if rel.parts and rel.parts[0] in IGNORED_TOP_LEVEL:
            continue
        if path.name.lower() == "agents.md":
            errors.append(f"FORBIDDEN_AGENTS forbidden instruction file exists: {rel.as_posix()}")
        if path.name.lower() == "config.toml" and path.parent.name.lower() == ".codex":
            errors.append(f"FORBIDDEN_CODEX_CONFIG forbidden project config exists: {rel.as_posix()}")


def _validate_manifest(root: Path, errors: list[str]) -> None:
    manifest = root / "FILE_MANIFEST.md"
    if not manifest.exists():
        return
    declared = sorted(re.findall(r"^- `([^`]+)`$", manifest.read_text(encoding="utf-8"), re.MULTILINE))
    actual = _repo_files(root)
    if declared != actual:
        missing = sorted(set(actual) - set(declared))
        stale = sorted(set(declared) - set(actual))
        errors.append(f"STALE_MANIFEST FILE_MANIFEST.md differs from repository files; missing={missing}, stale={stale}")


def validate_repository(root: Path, *, check_manifest: bool = True) -> list[str]:
    """Return deterministic validation errors; an empty list means success."""
    root = root.resolve()
    errors: list[str] = []
    graph, direct = _collect_links(root, errors)
    registry = _load_registry(root, errors)
    _validate_registry(root, registry, graph, direct, errors)
    _validate_adrs(root, direct, errors)
    _validate_forbidden_files(root, errors)
    if check_manifest:
        _validate_manifest(root, errors)
    return sorted(set(errors))


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--root", type=Path, default=DEFAULT_ROOT, help="repository root (used by tests)")
    args = parser.parse_args()
    errors = validate_repository(args.root)
    registry_path = args.root.resolve() / REGISTRY_REL
    count = 0
    if registry_path.exists():
        try:
            count = len(json.loads(registry_path.read_text(encoding="utf-8")).get("documents", []))
        except Exception:
            pass
    print(f"Registered documents: {count}")
    if errors:
        print("\nDOCUMENTATION VALIDATION FAILED")
        for error in errors:
            print(f"- {error}")
        return 1
    print("\nDOCUMENTATION VALIDATION PASSED")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
