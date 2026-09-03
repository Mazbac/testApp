# Current State

- Lifecycle: RELEASED
- Product: QuickShelf v0.1.0
- Platform: Windows x64 desktop
- Default branch: protected `main`
- Release tag: `v0.1.0`
- Release source: `02f57d48c5c3ab82c40edca4ac23bed04e23e081`
- Release page: `https://github.com/Mazbac/testApp/releases/tag/v0.1.0`
- Installer: `QuickShelf-Setup-0.1.0.exe`
- Installer SHA-256: `3968e7880a6bbeea9fdecee880cf75a872deab7ac8ea6e90a6f9e5c6c6389f1c`

## Release evidence
- Protected PR #1 merged only after required `template-integrity` and `quality` checks passed on its exact head.
- The squash merge tree exactly matched the reviewed PR head tree.
- Exact release-source `main` CI passed: Template Integrity run `33763069424`; Quality run `33763069524`.
- Exact release source passed repository adversarial self-tests, locked restore, context validation, formatting, Release build, 8/8 tests, security scanning and semantic UI persistence E2E.
- The exact release-source installer was rebuilt after merge and passed install -> launch/use -> uninstall -> preserved data -> reinstall -> restored data -> uninstall.
- Pre-existing local QuickShelf state was isolated during lifecycle testing and restored byte-for-byte afterward.
- GitHub release asset metadata reports the same installer SHA-256 as the locally tested artifact.

## Product/release posture
- v0.1.0 is a self-contained per-user Windows release with no account, cloud backend, telemetry, listener or production network dependency.
- Uninstall preserves user-authored snippets; explicit in-app Reset owns destructive data cleanup.
- v0.1.0 is unsigned because no authorized code-signing identity is available. Windows may show Unknown publisher and/or SmartScreen reputation warnings; QuickShelf does not claim verified publisher identity.

## Current priority
Use v0.1.0 as the baseline for the next template stress test. The highest-priority follow-on is v0.2: introduce a deliberate state-schema migration and prove install-over upgrade, migration backup and rollback-safe behavior without regressing v0.1 data.

## Known blockers
None for v0.1.0. Code signing remains an accepted distribution-trust limitation until a real signing identity is explicitly authorized.
