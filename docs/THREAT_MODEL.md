# QuickShelf Threat Model

## Assets
- User-authored snippets and local settings.
- Integrity of installed QuickShelf binaries and release artifacts.
- User-selected import/export files.

## Actors / entry points
- Local interactive user.
- Malformed or hostile JSON selected through Import.
- Corrupt/truncated local state caused by crash/disk failure/manual modification.
- Tampered build/dependency/installer inputs during development or distribution.

## Trust boundaries
- UI text -> application state.
- Local/imported JSON -> deserializer and validation boundary.
- App state -> `%LOCALAPPDATA%\QuickShelf` filesystem.
- Build tooling/dependencies -> compiled application and installer.

## Abuse cases and mitigations
- Oversized import exhausts memory: reject imports above a bounded size before parsing.
- Invalid schema destroys good data: validate complete candidate state before replacement; keep backup.
- Interrupted save truncates data: write temp file, flush, then atomic replacement/move.
- Snippet text triggers code/markup execution: store/render as plain text; never evaluate content.
- Path injection writes arbitrary files: app-owned persistence path is fixed; export path comes only from Windows file dialog.
- Uninstall deletes notes unexpectedly: installer owns program directory only; user data is preserved.
- Supply-chain tampering: minimal dependencies, official NuGet source, vulnerability checks, pinned GitHub Actions and verified installer tooling.

## Security posture
No network listener, remote service, account, secret, authentication system, plugin loader, script execution or administrator requirement exists in v0.1. Revisit this threat model before adding any such capability.
