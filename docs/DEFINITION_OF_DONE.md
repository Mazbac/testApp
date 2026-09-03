# Definition of Done

A feature/release is done only when every applicable item below is satisfied with evidence. Not-applicable items require an explicit reason.

## Product
- Accepted user outcome works end-to-end, including meaningful edge/error states.
- Scope/non-goals and changed assumptions are reflected in durable repository state.
- Applicable golden journeys succeed through the real product surface.

## Context integrity
- Manifest/schema/lifecycle checks pass.
- `STATUS.md` and durable project sources reflect the current project meaning and do not contradict stronger technical evidence.
- Material architecture, product, security and design decisions/preferences needed by replacement chats are persisted.

## Engineering
- Clean checkout/bootstrap works from documented prerequisites.
- Formatting/lint/type/schema/config checks pass where applicable.
- Unit/integration tests pass; critical journeys have E2E coverage proportional to risk.
- Build/package succeeds without relying on untracked local files.
- No unexplained runtime, browser console or relevant network errors.

## Security
- Threat-model impact reviewed.
- Authorization/input/data handling are tested at relevant trust boundaries.
- Applicable dependency, secret and static/security scans are green.
- No unresolved high/critical security finding.

## Design / UX
- Project design system and component strategy are respected; no unexplained one-off visual/interaction patterns.
- Responsive/adaptive and accessibility checks pass for supported surfaces.
- Loading/empty/error/disabled/destructive states are deliberate and recoverable where applicable.
- ChatGPT has visually inspected representative final UI and checked maturity against the project reference class/platform, not only DOM/test results.

## Professional product experience
- First-use path reaches the intended first useful outcome without unnecessary technical configuration.
- Defaults, permissions, onboarding, terminology, feedback, progress and recovery are appropriate for the target audience.
- Avoidable developer/internal implementation details are not exposed to ordinary users.
- Performance on critical journeys is acceptable for the product context.
- Applicable install, update/migration, repair/reset, uninstall/data handling and reinstall journeys behave deliberately and professionally.

## Release
- Clean/reproducible install or deployment is verified.
- Upgrade/migration and rollback are verified when data/runtime changes require them.
- Version/release notes and artifact/source revision match.
- Release artifact/distribution passes representative golden-journey smoke tests.
- Current independent CI/gates are green for the revision being released; stale textual claims are not release evidence.