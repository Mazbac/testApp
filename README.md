# Autonomous ChatGPT Project Template

A stack-neutral source-of-truth repository for building software with normal ChatGPT + GitHub + Desktop Commander, with the user acting as product owner rather than technical intermediary.

## Core guarantee target
The template is designed for **session invariance**: a replacement ChatGPT session should reconstruct substantially the same durable product meaning and next engineering priority from the repository and current evidence without needing the previous transcript.

## What this template provides
- Durable project memory, deterministic resume and context-reconciliation rules.
- Product discovery from raw natural-language intent, including incomplete brand/design input.
- Architecture and decision records before irreversible complexity.
- Professional Product Experience requirements across first use, daily use, recovery and applicable install/update/uninstall lifecycle.
- Product-design methodology with reference-class/platform research, project design systems, tokens and component strategy without forcing one UI library.
- Secure-development requirements and explicit threat modelling.
- Risk-based unit/integration/E2E/security/accessibility/visual/golden-journey testing.
- Schema-driven machine manifest, lifecycle/context-integrity gates and adversarial template canaries.
- Git/PR/CI quality gates, reproducible release/recovery rules and a hard Definition of Done.

## Start
Read `START_HERE.md`, then `AGENTS.md`. On a local clone, ChatGPT activates repository hooks with `.automation/bootstrap.mjs`, then runs the doctor, validator and context-integrity gate before changing project state.

## Design philosophy
The template owns the design process; each project owns its design language. Reuse proven interaction behavior, own the product design, respect platform conventions and treat setup/error/update/uninstall experience as part of release quality.

## Git safety
This private repository's current GitHub plan does not expose server-side rulesets or classic branch protection. A versioned pre-push hook blocks accidental local direct pushes to `main`/`master`; PR + green independent CI remains mandatory policy. Enable server-side protection when available.

The stack never chooses the product. Technology, component libraries and distribution mechanisms are selected after product constraints are understood.