# QuickShelf Delivery Plan

## DISCOVER / DEFINE / DESIGN
- [x] Bound v0.1 to a small local snippet manager that exercises the full Windows app lifecycle.
- [x] Choose .NET 10 WPF, versioned local JSON storage and NSIS packaging.
- [x] Define data ownership, recovery, uninstall behavior, golden journeys and non-goals.
- [x] Establish Windows-native design direction, tokens/component strategy and accessibility target.

## BUILD
- [x] Implement versioned state model, atomic persistence, corruption recovery and search.
- [x] Implement two-pane snippet UI with create/edit/favorite/search/delete and deliberate empty/error states.
- [x] Implement System/Light/Dark theme preference with local-culture formatting and high-contrast fallback.
- [x] Implement export, import and destructive reset with validation/confirmation/feedback.
- [x] Add unit/integration tests, security scanning and deterministic aggregate verification.
- [x] Add NSIS per-user installer with Start Menu integration and data-preserving uninstall.
- [x] Add project CI with repository/context gates, build, test and vulnerability review.

## VERIFY
- [x] Pass restore/format/build/test/security/context gates locally.
- [x] Exercise all five manifest golden journeys through the real product surface.
- [x] Inspect keyboard/UI Automation semantics, focus treatment, contrast and representative dark/light screenshots.
- [x] Verify final local installer install/use/uninstall/reinstall and preserved user data.
- [x] Reconcile implementation, manifest, architecture, design, risks and current status.

## RELEASE â€” current
- [x] Prepare and locally verify the complete candidate revision on `feat/quickshelf-v0.1`.
- [ ] Push the candidate, open PR, obtain required `template-integrity` and `quality` checks, and review the exact diff/security/product maturity.
- [ ] Merge through protected `main` only after required gates are green.
- [ ] Rebuild/retest the exact merged revision, create v0.1.0 release notes and publish the installer/checksum as GitHub Release assets.
- [ ] Record exact release revision/run/artifact evidence and advance lifecycle to RELEASED.

## Follow-on template stress test
- [ ] v0.2: add a deliberate state-schema migration and prove install-over upgrade + rollback handling.
