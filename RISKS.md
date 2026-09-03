# Risk Register

| Risk | Impact | Required mitigation |
|---|---|---|
| Chat/session loss | Product meaning or priority drifts | Repository-first resume sequence + current manifest/state/docs |
| Local data corruption | User loses notes | Versioned schema, atomic temp-file replace, quarantine invalid data, export backup, tests |
| Destructive reset/delete | Accidental data loss | Explicit confirmation naming consequence; no hidden destructive shortcuts |
| Malicious/oversized import | Memory/resource abuse or invalid state | File-size cap, strict validation, bounded strings/items, no execution/evaluation |
| UI design drift | App looks inconsistent across chats | `DESIGN_SYSTEM.md` + canonical XAML tokens + visual review |
| Accessibility regression | Keyboard/screen-reader users cannot use app | Standard WPF controls, AutomationProperties, UI Automation journeys, focus/contrast review |
| Installer requests elevation | Consumer-hostile/security regression | Per-user LocalAppData install; no admin rights; lifecycle test |
| Uninstall deletes user notes unexpectedly | Irreversible user-data loss | Preserve note data by default; explicit in-app Reset owns deletion |
| Runtime prerequisite leaks to user | Install fails on clean PC | Self-contained win-x64 publish; installed-artifact lifecycle test |
| Dependency/supply-chain compromise | Build/release compromise | Minimal dependencies, lockfiles, pinned Actions, vulnerability/secret scans, verified tooling downloads |
| Release artifact differs from tested revision | Unverified binary shipped | v0.1 was built after merge from exact source, checksum-matched to the uploaded asset and lifecycle-tested; repeat this release gate for every version |
| Unsigned installer trust warning | Windows may show Unknown publisher/SmartScreen friction | State limitation clearly; never imply verified publisher; signing requires a future authorized identity |
| Server-side branch rules are weakened/removed | Direct main change can bypass PR/CI policy | `main` protection requires PR + `template-integrity`/`quality`, enforces admins and blocks force-push/deletion; verify protection before every release |
| Template/docs drift from live implementation | Replacement chat trusts stale assumptions | Source precedence + context-integrity validation + reconciliation before release |
| v0.2 schema migration damages v0.1 state | Upgrade can lose or strand released user data | Treat v0.1 as the migration fixture; require pre-migration backup, regression tests, install-over upgrade and rollback-safe failure handling before v0.2 release |

High/critical security findings, unexplained data-loss risk and failed required release gates block release.
