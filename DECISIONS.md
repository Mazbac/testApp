# Decision Log

Record only durable decisions that future chats must understand. Do not use this as a transcript.

## D-001 — Repository is the source of truth
Status: accepted
Decision: Chat history is disposable; project state, constraints and durable decisions live in Git.
Reason: normal ChatGPT conversations can end or be replaced without warning.

## D-002 — User is product owner, not technical intermediary
Status: accepted
Decision: ChatGPT owns technical execution and uses available tools before requesting user action.
Reason: manual relay work is slow, error-prone and defeats autonomous delivery.

## D-003 — Template is stack-neutral
Status: accepted
Decision: choose technology after product discovery instead of embedding one app stack in the template.
Reason: one reusable template must support different software classes without inherited technical debt.

## D-004 — Independent gates over self-assessment
Status: accepted
Decision: CI, tests, scans and reproducible checks decide readiness; ChatGPT's confidence does not.
Reason: deterministic evidence is more reliable than conversational claims.

## D-005 — Free private-repo main guard
Status: accepted
Decision: activate a versioned local pre-push hook that rejects direct pushes to `main`/`master`; continue using PR + green CI by policy.
Reason: GitHub returned HTTP 403 for both rulesets and classic branch protection on this private repository, requiring GitHub Pro or public visibility. Repository visibility will not be changed implicitly.
Limitation: local hooks are defense-in-depth, not a substitute for server-side enforcement; a capable actor/tool can bypass them. If server-side protection becomes available, require PRs and the stable CI checks there.


## D-006 — Session invariance is the primary continuity invariant
Status: accepted
Decision: replacement chats must reconstruct substantially the same durable project meaning and next priority from repository state without prior transcript dependency.
Reason: repo-first memory is only useful if different sessions converge on the same project interpretation.

## D-007 — Context has explicit source precedence
Status: accepted
Decision: live technical evidence outranks machine-readable state, which outranks durable contracts, execution memory and chat history. Conflicts are reconciled and persisted.
Reason: stale documentation or chat must not override reality silently.

## D-008 — Product experience is a release discipline
Status: accepted
Decision: install/setup/onboarding/daily use/errors/recovery/update/uninstall and other applicable lifecycle surfaces are product quality, not optional polish.
Reason: technically correct software can still be consumer-hostile or visibly immature.

## D-009 — The template owns the design process, projects own their design language
Status: accepted
Decision: require reference-class/platform research, design direction, tokens, component strategy and durable project design sources without imposing one universal component library or visual style.
Reason: cross-chat consistency requires a stable design source, while different product classes and platforms require different design languages.
