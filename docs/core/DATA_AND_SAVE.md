# Data, Identity, History, and Saves

## Identity
Use stable nonreused domain IDs. ECS entities, scene objects, and presentation handles are transient projections.

## Save format
A save is an atomic container containing a human-readable manifest plus versioned binary domain chunks, checksums, history, interventions, landmark state, and preview metadata. Never serialize raw ECS memory.

## Save sequence
Quiescent fixed-tick barrier → staged domain snapshot → worker serialization → structural and cross-reference validation → temporary write → flush → atomic replace → retain previous-good backup.

## Migration
Sequential pure migrations with fixtures, reserved removed keys, rollback notes, and pre-migration backup. Unsupported future versions fail clearly.

## Historical retention
Canonical founding, war, treaty, government change, invention, extinction, disaster, world-defining ruin, and player intervention events are permanent. Routine detail compacts into bounded summaries. Landmark geometry and causal references are not silently erased.
