# QuickShelf

QuickShelf is a small local Windows snippet manager built as the first end-to-end trial of this repository's autonomous delivery contract.

## v0.1 scope
- Create, edit, favorite, search and delete short snippets.
- Autosave to versioned local JSON with atomic writes and corruption recovery.
- System, Light and Dark appearance preferences.
- JSON backup/export, validated import/restore and explicit destructive reset.
- Self-contained Windows 11 x64 installer; no account, cloud service, telemetry or separate .NET runtime.
- Uninstall removes the app but preserves user-authored snippets for reinstall.

User data is stored under `%LOCALAPPDATA%\QuickShelf`. The installed app lives under `%LOCALAPPDATA%\Programs\QuickShelf`.

## Development
Prerequisites: Git, Node.js 24, .NET SDK 10.0.400 and PowerShell. NSIS 3.12 is required only to build the installer.

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tools/bootstrap.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/verify.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/build.ps1
```

`AGENTS.md`, `.project/manifest.json` and the repository operating documents remain the source of truth for project state and release rules.

## Release posture
v0.1 is still in verification until the protected-branch PR and independent GitHub Actions gates are green. The test release is unsigned unless a real code-signing identity is later authorized, so Windows may report an unknown publisher or apply SmartScreen reputation checks.
