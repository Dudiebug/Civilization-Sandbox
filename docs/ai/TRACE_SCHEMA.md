# Decision Trace Schema

A consequential trace contains:

- trace ID, contract ID/version, world tick, entity stable ID, authority layer;
- observer perspective, knowledge records, confidence, and data age;
- trigger, current commitment, and invalidation reason;
- candidate count and candidate IDs;
- rejected preconditions with source evidence;
- normalized score terms, units, curves, modifiers, and policy IDs;
- trait, fast-state, attitude, social, and higher-layer contributions;
- hysteresis, cooldown, commitment, and change penalties;
- RNG stream address, bounded adjustment, and stable tie-break;
- selected action, reservations, expected effect, and next review;
- evaluation duration, allocations, query counts, and truncation flags;
- player-intervention ancestry where relevant.

Retail storage is bounded. Test builds may retain full traces. Major causes are promoted to the historical event graph before compaction.

TASK-015 owns the machine-readable schema and versioning rules. Individual contracts declare which optional terms apply; they may not omit authority, knowledge provenance, selected/rejected preconditions, deterministic selection evidence, or budget counters needed to prove their behavior.
