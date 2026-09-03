# Autonomous Secure Professional SDLC

## DISCOVER
Translate raw natural-language intent into problem, users, product-experience constraints, first useful release, non-goals and measurable acceptance. Capture durable brand/design signals without requiring a formal brief. Research current external facts when they affect decisions. Do not build while core product uncertainty would cause avoidable rework.

## DEFINE
Choose the simplest supportable architecture and stack. Define data ownership, trust boundaries, permissions, external dependencies, deployment/distribution model, install/update/uninstall implications, migrations/rollback and threat model. Record major decisions and project conventions needed for session invariance.

## DESIGN
Map critical and golden user journeys and every important state: obtain/install when applicable, first use, normal, empty, loading, success, validation, failure, recovery, offline/timeout and destructive confirmation. For UI, perform reference-class/platform research, establish the project design source, canonical tokens, component strategy, reusable patterns and accessibility target before broad feature styling.

## BUILD
Implement small vertical slices. A slice is incomplete without tests, validation, authorization, observability/error handling, relevant UI states, product-experience behavior and durable-context impact. Keep the app runnable and keep project design/architecture sources synchronized with material changes.

## VERIFY
Run format/lint/type/schema/config checks, unit/integration/E2E tests, security checks, context-integrity checks, golden journeys, accessibility and visual/maturity review when UI exists, supported platform/browser checks, performance checks where user experience depends on them, clean install/build and upgrade/uninstall/reinstall checks where applicable.

## RELEASE
Build from a clean committed revision whose applicable independent gates are green. Produce the platform-appropriate versioned artifact/distribution. Verify install/deploy, migrations, update/rollback, user-data behavior and uninstall where applicable. Run release-artifact golden-journey smoke tests and record evidence tied to the tested revision/run.

## OPERATE
Treat defects, security advisories, dependency updates, experience regressions and new ideas as controlled changes through the same loop. Preserve accepted product/design meaning unless a recorded decision supersedes it. Never bypass gates for a "small" production fix; scale evidence to risk.

## Evidence and lifecycle rule
A phase advances only when its required repository outputs exist and applicable automated/manual-by-ChatGPT checks have evidence. `node .automation/context-integrity.mjs` enforces machine-checkable lifecycle/context prerequisites. User approval or model confidence cannot substitute for failed technical or experience gates.