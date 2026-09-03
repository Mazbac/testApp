# Activating a Derived Project

Do this after product discovery and before substantial implementation.

1. Replace template placeholders in `PROJECT.md` with actual problem, users, first release, product-experience constraints, brand/design inputs and explicit non-goals.
2. Set `.project/manifest.json` to `mode: "project"` and lifecycle `DEFINE` (or the earliest accurate non-template phase).
3. Fill every manifest `UNSET`; use explicit values such as `none` or `not-applicable` when a capability is intentionally absent.
4. Record chosen stack/runtime/package manager/database, distribution model and rationale in `ARCHITECTURE.md` / `DECISIONS.md`.
5. Define exact reproducible commands in the manifest, including one aggregate `commands.verify` gate.
6. Create `.github/workflows/quality.yml` with stable job names, minimal permissions, stack-appropriate build/test/security checks and full-SHA Action pinning.
7. Define applicable golden journeys and lifecycle experience in `experience` before broad implementation.
8. If a UI exists, perform reference-class/platform research and create a concrete project design source from `docs/DESIGN_SYSTEM_TEMPLATE.md`.
9. Point `ui.designSystem` and `ui.designTokens` at real tracked sources; record component strategy and accessibility target.
10. For web UI, configure project-local Playwright and set `ui.playwright: true` plus `ui.devUrl`.
11. Update `STATUS.md` lifecycle to exactly match the manifest and remove stale template-state claims.
12. Run `node .automation/validate.mjs` and `node .automation/context-integrity.mjs`; project mode intentionally fails until its required project-specific sources are concrete.
13. Update `PLAN.md`, `RISKS.md`, relevant decisions and open the first implementation PR.

The activation commit is the point where the repository stops being a generic template and becomes one concrete product. Do not satisfy gates with placeholder artifacts.