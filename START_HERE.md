# Start Here

Use this repository for one project only. Do not import assumptions or rules from other repositories unless the user explicitly asks.

## New project prompt
`Use this repository as the source of truth. Read AGENTS.md and the repository operating documents first. I will describe what I want in natural language. Own the technical work end-to-end through a secure, professional, consumer-ready, tested release. Preserve cross-chat consistency and do not make me the technical intermediary.`

## Deterministic first-run / resume sequence
1. Read `AGENTS.md` and `.project/manifest.json`.
2. Read `PROJECT.md`, `STATUS.md`, `PLAN.md`, `DECISIONS.md`, `RISKS.md`, `ARCHITECTURE.md` and applicable manifest-referenced design/experience sources.
3. When local Git is available, run `node .automation/bootstrap.mjs` once for the clone.
4. Run `node .automation/doctor.mjs`, `node .automation/validate.mjs` and `node .automation/context-integrity.mjs`.
5. Inspect Git status, branches, open PRs and current CI before trusting status claims.
6. Reconcile contradictions using `docs/CONTEXT_INTEGRITY.md` before making changes.
7. If manifest mode is `template`, enter discovery and turn raw natural-language intent into a bounded first useful release.
8. Choose architecture, stack, product-experience strategy and UI design approach only after product constraints are understood.
9. Activate project mode using `docs/PROJECT_ACTIVATION.md`; project mode must become machine-valid before substantial implementation.
10. Work through `docs/SDLC.md` and keep durable state current.

## Resume prompt
`Resume this repository. Reconstruct the real state from the manifest, durable project sources, GitHub, the working tree and current CI. Reconcile contradictions, preserve accepted product/design decisions and continue autonomously from the highest-priority unfinished verified step.`

A replacement chat must not need the previous transcript. If prior conversation contains durable information not yet persisted, persist it at the next safe checkpoint.