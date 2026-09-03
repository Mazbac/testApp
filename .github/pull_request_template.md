## Outcome
Describe the user/product outcome, not only files changed.

## Verification evidence
- [ ] Relevant static/build checks pass.
- [ ] Relevant unit/integration/E2E/golden-journey tests pass.
- [ ] Regression coverage was added for fixed defects where practical.
- [ ] Clean bootstrap/build impact was checked.
- [ ] `node .automation/context-integrity.mjs` passes.

## Context continuity
- [ ] Durable product/architecture/design decisions or preferences were persisted where needed.
- [ ] `STATUS.md`, `PLAN.md`, `RISKS.md`, `DECISIONS.md` and referenced design/experience sources were updated when affected.
- [ ] No stale status claim contradicts stronger Git/CI/runtime evidence.

## Security
- [ ] Threat-model impact reviewed.
- [ ] Authorization/input/secret/dependency risks checked as applicable.
- [ ] No unresolved high/critical security finding.

## Design / product experience (if applicable)
- [ ] Project design system/component strategy and platform conventions were respected.
- [ ] Responsive/accessibility and relevant UI states were checked.
- [ ] Representative UI/journeys were visually inspected; maturity gaps versus the reference class were addressed.
- [ ] Setup/defaults/errors/recovery and applicable install/update/uninstall behavior were reviewed.

## Release / recovery
- [ ] Migration/upgrade and rollback implications reviewed.
- [ ] Applicable installer/updater/uninstaller/reinstall paths were checked.
- [ ] Release evidence applies to the revision/artifact being proposed.

Not-applicable items require a short reason in the PR description.