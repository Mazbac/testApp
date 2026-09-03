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

## Release
QuickShelf v0.1.0 is released at `https://github.com/Mazbac/testApp/releases/tag/v0.1.0` from source revision `02f57d48c5c3ab82c40edca4ac23bed04e23e081`.

Installer SHA-256: `3968e7880a6bbeea9fdecee880cf75a872deab7ac8ea6e90a6f9e5c6c6389f1c`.

The v0.1.0 test release is unsigned because no authorized code-signing identity is available. Windows may report an unknown publisher or apply SmartScreen reputation checks; QuickShelf does not claim a verified publisher identity.
