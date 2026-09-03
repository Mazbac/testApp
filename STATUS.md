# Current State

- Lifecycle: TEMPLATE
- Foundation status: READY
- Foundation v2 merge: `26951f5b25759ad96bcf5d6c789acf6aaff9badd`
- Manifest schema: v2
- Local validation on merged v2 foundation: GREEN
- Independent PR validation for implementation PR #3: GREEN
- Post-merge `main` GitHub Actions validation: GREEN
- Local direct-default-branch guard: VERIFIED BLOCKING

## Verified foundation
- Session invariance is the primary continuity invariant; chat history is disposable project context.
- Explicit source precedence/reconciliation, durable-input classification and decision supersession.
- Stack-neutral manifest v2 with machine-readable context, UI/design, product-experience and release state.
- Schema-driven repository validation plus a separate context-integrity gate.
- Positive baseline and six adversarial negative canaries run locally and in GitHub Actions.
- Secure SDLC, professional Product Experience Standard, professional design methodology, security standard, test strategy and hard Definition of Done.
- UI projects require durable design-system/token sources, component strategy, accessibility target and golden journeys before lifecycle advancement.
- Derived project CI must run repository validation and context integrity.
- Local bootstrap/doctor checks Git, GitHub CLI, Node, npm, Playwright and versioned Git hooks.
- Full-SHA GitHub Actions pinning remains enforced.

## Platform constraint
GitHub server-side rulesets/classic branch protection remain unavailable for this private repository on the current plan. The versioned local pre-push guard remains defense-in-depth; PR + green CI is mandatory policy. Enable server-side protection if the plan/visibility later supports it.

## Next use
Create/use a repository from this template and give normal ChatGPT the new-project prompt in `START_HERE.md`. ChatGPT should reconstruct context deterministically, discover the product from natural language, activate project mode, establish project-specific design/experience/security/quality sources, and continue autonomously through the SDLC.

## Known blockers
None for normal template use. Server-side branch enforcement remains a GitHub plan/visibility capability constraint, not a product-development blocker.