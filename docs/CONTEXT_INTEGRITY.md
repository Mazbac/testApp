# Context Integrity and Session Invariance

The repository must make important project truth reconstructable without prior chat history.

## Primary invariant
Given the same repository revision and available tooling, competent replacement ChatGPT sessions should reconstruct substantially the same product intent, current state, architecture, design language, security boundaries, open risks and next engineering priority.

This is session invariance. Exact wording or implementation choices may differ; durable project meaning must not.

## No hidden project state
Information that can materially change future work must not live only in chat. Persist it in the appropriate repository source: product intent in `PROJECT.md`, technical boundaries in `ARCHITECTURE.md`, durable rationale in `DECISIONS.md`, current verified state in `STATUS.md`, ordered work in `PLAN.md`, hazards in `RISKS.md`, deferred scope in `BACKLOG.md`, and UI design language in the manifest-referenced design system.

## Source precedence
When sources disagree, reconcile in this order:
1. Live technical evidence: working tree, runtime state, CI, tests and deployed/release evidence.
2. Machine-readable owned state: `.project/manifest.json` and project configuration.
3. Durable contracts: `PROJECT.md`, `ARCHITECTURE.md`, `DECISIONS.md`, security/design/experience artifacts.
4. Execution memory: `STATUS.md`, `PLAN.md`, `RISKS.md`, `BACKLOG.md`.
5. Chat history and conversational summaries.

A lower-precedence source must never silently override stronger evidence. Investigate the conflict, repair durable state and continue from the reconciled truth.

## Durable input classification
ChatGPT classifies meaningful user input without asking the user to maintain project records:
- ephemeral instruction: affects only the current action;
- durable project decision: changes future product, architecture, security, operations or release behavior and must be persisted;
- durable product/design preference: changes future user experience or visual/interaction direction and must be persisted in the appropriate design/product source.

## Decision continuity
Accepted durable decisions remain authoritative until evidence or requirements justify change. Do not silently rewrite history. Supersede a decision with a new decision that identifies what changed and why.

## Resume reconciliation
Every replacement session must read the manifest and durable state, inspect Git/branches/open PRs/current CI, run repository validation when a working tree is available, reconcile contradictions, and only then continue from the highest-priority unfinished verified step.

## Checkpoint rule
Before ending meaningful work, persist changes that future sessions need. A chat summary is never the only copy of a decision, blocker, migration instruction, design preference or verified state.

## Machine enforcement
`node .automation/context-integrity.mjs` validates machine-checkable invariants. It cannot prove semantic correctness, so ChatGPT still performs reconciliation and review. Context-integrity failures block lifecycle advancement and release.