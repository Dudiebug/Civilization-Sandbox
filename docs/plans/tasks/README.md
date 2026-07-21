# Playable Build Prompts

Each file in this folder is a creator-facing prompt for one complete playable build.

These prompts are deliberately larger than a single coding session and smaller than a full release. Codex should read the prompt, inspect the current project, and propose 3-8 implementation slices. It should implement one slice at a time, keep Unity runnable, and use creator play feedback to choose the next slice.

Do not paste all prompts into one session. Start only the current build.

The Blueprint and `../VISION_AUTHORITY.md` remain binding. A build prompt can adjust implementation depth but cannot silently delete preserved scope.
