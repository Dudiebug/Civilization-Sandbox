from __future__ import annotations

import json
import sys
import tempfile
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / "Build"))

from validate_docs import validate_repository  # noqa: E402


class DocumentationValidatorTests(unittest.TestCase):
    def make_repo(self) -> tuple[tempfile.TemporaryDirectory[str], Path]:
        temporary = tempfile.TemporaryDirectory()
        root = Path(temporary.name)
        (root / "Config").mkdir()
        (root / "docs/decisions").mkdir(parents=True)
        (root / "README.md").write_text("# Readme\n\n[Index](docs/DOCUMENT_INDEX.md)\n", encoding="utf-8")
        (root / "START_HERE.md").write_text("# Start\n\n[Index](docs/DOCUMENT_INDEX.md)\n", encoding="utf-8")
        (root / "codex.md").write_text("# Instructions\n\n[Index](docs/DOCUMENT_INDEX.md)\n", encoding="utf-8")
        (root / "docs/spec.md").write_text("# Specification\n\n## Stable section\n", encoding="utf-8")
        (root / "docs/DOCUMENT_INDEX.md").write_text(
            "# Index\n\n"
            "[Readme](../README.md)\n"
            "[Start](../START_HERE.md)\n"
            "[Instructions](../codex.md)\n"
            "[Specification](spec.md#stable-section)\n",
            encoding="utf-8",
        )
        (root / "docs/decisions/README.md").write_text("# Decisions\n", encoding="utf-8")
        documents = []
        for doc_id, path, authority in (
            ("README", "README.md", "instruction"),
            ("START", "START_HERE.md", "instruction"),
            ("INSTRUCTIONS", "codex.md", "instruction"),
            ("INDEX", "docs/DOCUMENT_INDEX.md", "index"),
            ("SPEC", "docs/spec.md", "specification"),
        ):
            documents.append(
                {
                    "id": doc_id,
                    "path": path,
                    "title": doc_id.title(),
                    "authority": authority,
                    "owner": "Test owner",
                    "status": "Current",
                    "release_scope": "Repository",
                    "required": True,
                    "source_refs": ["TEST"],
                }
            )
        registry = {
            "schema_version": 1,
            "entrypoints": ["README.md", "START_HERE.md", "codex.md"],
            "documents": documents,
        }
        (root / "Config/document-registry.json").write_text(json.dumps(registry, indent=2), encoding="utf-8")
        return temporary, root

    def assert_code(self, errors: list[str], code: str) -> None:
        self.assertTrue(any(error.startswith(code) for error in errors), errors)

    def test_valid_repository_passes(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            self.assertEqual([], validate_repository(root))

    def test_broken_internal_reference_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            (root / "docs/DOCUMENT_INDEX.md").write_text("# Index\n\n[Missing](missing.md)\n", encoding="utf-8")
            errors = validate_repository(root)
            self.assert_code(errors, "BROKEN_LINK")
            self.assertIn(
                "BROKEN_LINK docs/DOCUMENT_INDEX.md:3 target does not exist: missing.md",
                errors,
            )

    def test_missing_anchor_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            index = root / "docs/DOCUMENT_INDEX.md"
            index.write_text(index.read_text(encoding="utf-8").replace("#stable-section", "#missing"), encoding="utf-8")
            self.assert_code(validate_repository(root), "MISSING_ANCHOR")

    def test_unreachable_required_document_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            hidden = root / "docs/hidden.md"
            hidden.write_text("# Hidden\n", encoding="utf-8")
            registry_path = root / "Config/document-registry.json"
            registry = json.loads(registry_path.read_text(encoding="utf-8"))
            registry["documents"].append(
                {
                    "id": "HIDDEN",
                    "path": "docs/hidden.md",
                    "title": "Hidden",
                    "authority": "specification",
                    "owner": "Test owner",
                    "status": "Current",
                    "release_scope": "Repository",
                    "required": True,
                    "source_refs": ["TEST"],
                }
            )
            registry_path.write_text(json.dumps(registry), encoding="utf-8")
            self.assert_code(validate_repository(root), "UNREACHABLE_DOCUMENT")

    def test_unregistered_index_document_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            extra = root / "docs/extra.md"
            extra.write_text("# Extra\n", encoding="utf-8")
            index = root / "docs/DOCUMENT_INDEX.md"
            index.write_text(index.read_text(encoding="utf-8") + "\n[Extra](extra.md)\n", encoding="utf-8")
            self.assert_code(validate_repository(root), "UNREGISTERED_INDEX_DOCUMENT")

    def test_duplicate_adr_number_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            for name in ("ADR-001_ONE.md", "ADR-001_TWO.md"):
                (root / "docs/decisions" / name).write_text("# Decision\n\n**Status:** Proposed\n", encoding="utf-8")
            self.assert_code(validate_repository(root), "DUPLICATE_ADR")

    def test_path_case_mismatch_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            index = root / "docs/DOCUMENT_INDEX.md"
            index.write_text(index.read_text(encoding="utf-8").replace("spec.md", "Spec.md"), encoding="utf-8")
            self.assert_code(validate_repository(root), "PATH_CASE")

    def test_agents_file_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            (root / "AGENTS.md").write_text("# Forbidden\n", encoding="utf-8")
            self.assert_code(validate_repository(root), "FORBIDDEN_AGENTS")

    def test_project_codex_config_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            (root / ".codex").mkdir()
            (root / ".codex/config.toml").write_text("project_doc_fallback_filenames = ['codex.md']\n", encoding="utf-8")
            self.assert_code(validate_repository(root), "FORBIDDEN_CODEX_CONFIG")

    def test_stale_manifest_fails(self) -> None:
        temporary, root = self.make_repo()
        with temporary:
            (root / "FILE_MANIFEST.md").write_text("# Manifest\n\n- `README.md`\n", encoding="utf-8")
            self.assert_code(validate_repository(root), "STALE_MANIFEST")


if __name__ == "__main__":
    unittest.main()
