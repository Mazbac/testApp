# Test Strategy

Testing follows risk, behavior and release experience, not a coverage percentage alone.

## Layers
- Contract/context: manifest schema, lifecycle prerequisites, required durable sources and context-integrity drift checks.
- Static: formatting, linting, type/schema validation and build/config checks.
- Unit: deterministic business rules, transformations and edge cases.
- Integration: database/filesystem/network boundaries, migrations and external adapters with controlled dependencies.
- E2E: critical user journeys through the real application surface.
- Security: authorization boundaries, hostile/malformed inputs and dependency/secret scanning.
- UI: accessibility assertions, responsive/adaptive behavior, console/network cleanliness and visual regression where stable.
- Product experience: setup/onboarding, sane defaults, feedback/errors/recovery and platform conventions.
- Release lifecycle: clean bootstrap/install, upgrade/migration, built-artifact smoke test, rollback, uninstall/reinstall and data cleanup/preservation where applicable.

## Golden journeys
Every project defines a small set of user-value journeys in the manifest before BUILD. Prefer new-user first value, returning-user primary task, representative failure/recovery and lifecycle journeys where applicable. Automate the highest-risk/most-repeatable parts and supplement with direct ChatGPT inspection when automation cannot judge product maturity.

## Regression rule
A reproduced defect gets a failing automated test before or alongside the fix whenever technically reasonable. Product-experience and context-drift defects receive regression coverage too when they can be made deterministic.

## Playwright / UI automation
For web software, use semantic locators (role/label/text/test-id) rather than pixel coordinates. Test additional browser engines when compatibility matters. Capture traces/screenshots on failure and inspect browser console/network errors. Screenshots support visual judgment; they do not replace state assertions.

## Reliability
Tests must be deterministic enough to act as gates. Fix flaky tests or underlying races; do not normalize rerunning until green. Project manifest and CI name the exact verification commands used as release evidence.

## Template adversarial testing
The template itself maintains positive and negative canaries. Changes to governance/enforcement must prove that the valid template passes and representative invalid states are rejected, including context drift and incomplete project/design activation.