# Release and Recovery

## Release path
1. Start from a clean committed revision whose current independent CI/gates are green.
2. Reconcile durable status with Git/runtime evidence; stale `GREEN` text is not release evidence.
3. Build/package in a reproducible clean environment when feasible.
4. Run the full applicable verification suite against the release candidate/artifact, including context integrity and golden journeys.
5. Verify install/deploy, data migration/update, rollback, recovery and uninstall/reinstall where applicable.
6. Verify product identity, version and distribution/signing/trust expectations appropriate to the target platform.
7. Create versioned release notes tied to the exact tested revision/artifact.
8. Deploy/install the tested artifact, not an ad-hoc developer workspace.
9. Run production/release-artifact smoke tests and record the result in `STATUS.md`.

## Failure handling
Prefer rollback to emergency unverified edits when a released change causes serious regression. Preserve logs/evidence before cleanup where safe. Fix through a branch with a regression test and the same quality/context/product-experience gates.

## Chat/session recovery
A session may end at any time. Before a logical stopping point, persist durable state. A replacement chat reconstructs from repository/GitHub/working tree rather than guessing from prior conversation summaries, following `docs/CONTEXT_INTEGRITY.md`.

Minimum resume inputs:
- `.project/manifest.json`: machine lifecycle/capability/context state;
- `PROJECT.md`: product intent and experience/brand constraints;
- `STATUS.md`: current claimed verified state;
- `PLAN.md`: ordered work;
- `DECISIONS.md`: durable rationale and supersession history;
- `RISKS.md`: open hazards;
- `ARCHITECTURE.md`: technical boundaries/conventions;
- manifest-referenced design sources when UI exists;
- Git status/branches/open PRs/current CI: actual technical evidence.

Run context integrity and reconcile conflicts before continuing. Never leave the only copy of an important decision, design preference, migration instruction or blocker inside chat history.