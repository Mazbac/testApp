# Risk Register

| Risk | Impact | Required mitigation |
|---|---|---|
| Chat context loss | Work is repeated or assumptions drift | Repo-first deterministic resume procedure + current durable state |
| Cross-chat semantic drift | Different sessions build different interpretations of the same product | Session-invariance contract, durable design/product sources, context-integrity tests |
| Docs/machine/code divergence | New session trusts stale or contradictory state | Source precedence, reconciliation, manifest/schema/context-integrity gates |
| Premature "done" claim | Broken or immature release reaches user | Hard Definition of Done + independent CI + professional experience review |
| UI/design-direction drift | Product becomes visually inconsistent or changes personality across chats | Manifest-referenced design system, tokens, product component layer, visual regression/review |
| Generic AI UI | Coherent but visibly less mature than professional reference products | Reference-class/platform research and explicit maturity review before release |
| Component-library leakage/lock-in | Vendor styling/API defines product identity and makes replacement costly | Explicit component strategy; product abstractions over proven primitives where practical |
| Consumer lifecycle regression | Install/setup/update/uninstall works technically but becomes confusing or fragile | Product Experience Standard + golden lifecycle journeys |
| Security added late | Structural vulnerabilities | Threat model during DEFINE; security tests during every slice |
| Tool-driven accidental change | Data/code loss | Branches, small reversible changes, backups/migrations, rollback |
| Dependency/supply-chain compromise | Build/runtime compromise | Minimal dependencies, lockfiles, scanning, pinned CI actions |
| Local-only success | Release fails on another machine | Clean-install/reproducible build test |
| Over-automation via pixel clicking | Fragile tests/actions | API/CLI/semantic UI automation before coordinate input |
| Scope creep | Endless project / fragile architecture | First useful release + explicit backlog + impact analysis |
| User becomes relay operator | Slow/error-prone development | Exhaust ChatGPT-accessible tools before escalation |
| Private repo lacks server-side branch protection on current GitHub plan | Direct main updates cannot be blocked by GitHub itself | Versioned local pre-push guard + PR/CI policy; enable server-side rules if later supported |

New material risks must be added when discovered and closed only with evidence.