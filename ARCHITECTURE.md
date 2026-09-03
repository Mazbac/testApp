# Architecture Record

## Product architecture
QuickShelf is a Windows-only desktop application with no backend, account, telemetry, or network dependency in v0.1.

- UI: WPF on .NET 10 LTS, targeting `net10.0-windows`.
- Application shape: single process; WPF code-behind owns narrow UI event orchestration while models/services own persistence, validation, search and theme behavior that can be tested independently.
- Data: versioned JSON document under `%LOCALAPPDATA%\QuickShelf\quickshelf.json`.
- Persistence: write to a temporary file and atomically replace the durable file to reduce corruption risk.
- Import/export: explicit user-selected JSON files through Windows file dialogs; imported data is validated before replacing local state.
- Theme: System / Light / Dark, stored with app data and applied at startup.
- Distribution: self-contained `win-x64` publish packaged by NSIS as a per-user installer.
- Uninstall: binaries and shortcuts are removed; user note data is preserved by default. In-app Reset permanently removes product-owned data after confirmation.

## Boundaries and trust
- All local note content and imported files are untrusted input and must be validated before use.
- QuickShelf never executes note text, imported content, scripts, commands, or remote code.
- The app requests no administrator privileges and exposes no listening ports or public endpoints.
- File dialogs are the only user-driven filesystem boundary beyond the app-owned LocalAppData directory.
- No secrets or credentials exist in the product design.

## Engineering conventions
1. Keep domain/data behavior independently testable from WPF controls.
2. Avoid external runtime dependencies unless they materially reduce risk.
3. Use nullable reference types and deterministic JSON serialization options.
4. User-facing failures use plain language; diagnostics remain internal/debug-only.
5. New persistent schema versions require a migration path and regression test.
6. Release builds are self-contained so end users do not install the .NET runtime manually.

## Release topology
Developer source -> .NET restore/build/test -> self-contained publish -> NSIS installer -> installed `%LOCALAPPDATA%\Programs\QuickShelf` application. User data lives separately under `%LOCALAPPDATA%\QuickShelf`, allowing reinstall/upgrade without deleting notes.

## Threat model summary
Assets: note content, exported backups, local settings, release integrity. Actors: local user, malicious imported JSON, compromised dependency/build input. Entry points: import file, text fields, installer/update package. Mitigations: strict schema/size validation, no evaluation/execution, least privilege, atomic writes, locked dependencies, pinned CI actions, secret/dependency scans, and hash/revision-matched release artifacts.
