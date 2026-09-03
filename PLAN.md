# QuickShelf Delivery Plan

## v0.1.0 — RELEASED
- [x] Bound v0.1 to a small local snippet manager that exercises the full Windows app lifecycle.
- [x] Define product, architecture, data ownership, recovery, security boundaries, design system and non-goals.
- [x] Implement snippets, search, favorites, autosave, recovery, System/Light/Dark themes, export/import and destructive reset.
- [x] Add unit/integration tests, semantic Windows UI Automation E2E, security scanning and adversarial repository canaries.
- [x] Build a self-contained per-user NSIS installer with Start Menu integration and data-preserving uninstall.
- [x] Exercise all five manifest golden journeys and inspect representative Light/Dark final surfaces.
- [x] Protect `main` server-side and require `template-integrity` + `quality` through pull requests.
- [x] Fix reproducibility defects discovered by independent CI without weakening gates.
- [x] Merge PR #1 and prove the merge tree matches the reviewed candidate tree.
- [x] Rebuild/retest exact release source `02f57d48c5c3ab82c40edca4ac23bed04e23e081`.
- [x] Pass exact-source main CI runs `33763069424` and `33763069524`.
- [x] Publish GitHub Release `v0.1.0` with installer and checksum.

## Release artifact
- Installer: `QuickShelf-Setup-0.1.0.exe`
- SHA-256: `3968e7880a6bbeea9fdecee880cf75a872deab7ac8ea6e90a6f9e5c6c6389f1c`
- Release: `https://github.com/Mazbac/testApp/releases/tag/v0.1.0`
- Trust limitation: unsigned test release; no verified publisher identity is claimed.

## v0.2 — next template stress test
- [ ] Define a small user-visible field that requires schema v1 -> v2 migration without inflating product scope.
- [ ] Create a pre-migration backup and explicit migration/rollback contract before changing persisted state.
- [ ] Add migration regression tests using real v0.1 state fixtures.
- [ ] Build v0.2 and verify install-over upgrade preserves every supported v0.1 datum.
- [ ] Verify failed/unsupported migration does not silently destroy or overwrite recoverable v0.1 data.
- [ ] Repeat protected PR, independent CI, exact-revision installer lifecycle and release evidence.

No v0.2 implementation has started. v0.1.0 remains the stable released baseline.
