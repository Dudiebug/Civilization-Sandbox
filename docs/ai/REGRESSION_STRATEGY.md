# AI Regression Strategy

## Comparison levels
1. Exact deterministic fixture output.
2. Canonical trace/state-diff comparison.
3. Property and metamorphic invariants.
4. Multi-seed distribution envelopes.
5. Long-run system outcomes and performance.

## Change protocol
- State the intended behavioral change before editing.
- Add or update the contract and fixtures first.
- Capture baseline traces/distributions.
- Implement the smallest change.
- Compare intended and unrelated decisions.
- Treat unrelated behavioral movement as a regression unless explicitly approved.
- Add every confirmed bug as a permanent fixture.

## High-risk review questions
Did the change add hidden knowledge, bypass an authority layer, enlarge candidates, introduce order-dependent RNG, alter save semantics, create camera dependence, or move cost into another system?

## Ledger rule
Confirmed defects receive a never-reused `REG-NNN` identity, failing-then-passing fixture, affected contract/version, root cause, and resolution evidence. Closing or superseding a record never deletes it.
