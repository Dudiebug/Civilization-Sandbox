# Build and Automation Scope

Scripts must be deterministic, idempotent where appropriate, noninteractive in CI, explicit about prerequisites, and fail with actionable diagnostics. Do not hide failures, mutate developer machines outside declared locations, download unpinned dependencies, or delete user data. Destructive operations require explicit paths and confirmation/CI-safe guards.
