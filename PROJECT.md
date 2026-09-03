# QuickShelf — Project Definition

## Status
Activated Windows desktop project. First useful release: v0.1.0.

## Product intent
- Problem: people need a fast private place for short notes/snippets without accounts or cloud setup.
- Primary user: ordinary Windows users who want lightweight local scratch storage.
- Desired outcome: capture, find, edit, favorite, export and recover snippets with near-zero setup.
- First useful release: installable Windows 11 x64 app with local persistence and a complete lifecycle.
- Explicit non-goals: accounts, cloud sync, collaboration, rich text, plugins, mobile/web clients and telemetry.

## Product experience
- Audience: non-technical consumer; implementation details stay hidden.
- First value: install, launch, create a snippet, close/reopen and see it preserved.
- Distribution: versioned self-contained Windows installer; no separate .NET runtime required.
- Onboarding: no wizard; an empty-state prompt and obvious New snippet action are sufficient.
- Updates: v0.1 supports install-over upgrade; no background updater.
- Recovery: atomic writes + backup; corrupt data is quarantined rather than overwritten.
- Uninstall: removes app binaries/shortcuts but preserves user data; in-app Reset deletes data explicitly.

## Brand / creative input
- Product name: QuickShelf.
- Character: calm, native, compact, trustworthy; Windows Fluent-inspired rather than web-dashboard-like.
- Typography: Segoe UI/system typography.
- Durable preference: keep the app small and the development/release path broad enough to test this repository.

## Acceptance
Release only when install/launch/use/restart/export-reset-import/theme/uninstall-reinstall journeys work through the real product surface and repository Definition of Done is satisfied.
