# Test Scope

Tests are first-class product assets.

- Use deterministic fixtures with stable seeds and explicit clocks.
- Separate pure unit, scenario, property, metamorphic, statistical, migration, replay, benchmark, soak, and presentation tests.
- A bug fix adds a failing fixture before or with the fix.
- Never weaken an assertion or widen a tolerance merely to pass without documented evidence.
- Test code may inspect debug interfaces but must not become a second implementation of gameplay rules.
- Keep performance tests isolated from ordinary unit-test timing noise and report reference hardware.
