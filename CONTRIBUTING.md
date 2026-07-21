# Contributing and Codex Workflow

1. Select one playable build prompt from `docs/plans/tasks/`.
2. Have Codex propose 3-8 playable slices, then approve only the next slice.
3. Work on the published build branch in the authoritative checkout.
4. Read root and scoped `codex.md` files.
5. Update behavior/data contracts before or with implementation.
6. Keep the diff coherent; do not mix cleanup, future-release work, or unrelated tuning.
7. Run the task's required build, tests, replay, persistence, performance, and documentation checks.
8. Run the Unity result and keep a working rollback commit.
9. Use formal evidence and adversarial review at the build/release gate, proportional to risk.
10. Obtain creator-visible acceptance for the playable build.
11. Merge only through the protected process and update the roadmap when play changes priorities.

Branches use `build/BUILD-NN-description` in the single authoritative checkout at `C:\Users\dudie\Projects\Civilization-Sandbox`; switch branches only after preserving and publishing the current branch. Persistent sibling worktrees, secondary clones, and copied Unity project folders are prohibited. `main` requires a pull request and the `repository-policy` check after its first successful run; direct pushes, force-pushes, and deletion are prohibited. See `docs/core/REPOSITORY_GOVERNANCE.md`.

High-risk changes may not be planned, implemented, verified, and approved by one uninterrupted role/session. Agents may not answer milestone product questions on the creator's behalf.

Any change to a locked task interface requires an explicit creator-facing question and recorded approval before the interface or its authority files are edited.
