# Project Design System

## Product character
- Desired product feeling: calm, immediate, lightweight, trustworthy, native to Windows.
- Primary user/design context: one person quickly capturing and retrieving short text snippets on a desktop PC.
- Information density: compact but breathable; note list and editor visible together on normal desktop widths.
- Interaction principles: instant first value, keyboard-friendly, autosave, obvious recovery, no technical configuration.

## Brand and creative input
- Existing brand/assets: none; QuickShelf is a neutral product identity for this repository trial.
- Brand colors/typography: use Windows system typography and restrained semantic accents rather than a bespoke brand palette.
- Tone/content character: concise, plain English, action labels that describe results.
- User-provided references: no visual reference supplied.
- Explicit durable likes/dislikes: app should be small in scope but feel like a real installable Windows product, not a prototype.

## Reference class
- Products/platform guidance reviewed: Microsoft Sticky Notes and current Windows app design/accessibility guidance.
- Patterns worth adopting: quick new-note creation, immediate search filtering, keyboard shortcuts, remembered state, System/Light/Dark appearance, shallow navigation.
- Patterns deliberately avoided: account/sync prompts, floating individual note windows, rich-text complexity, decorative visual noise.
- Deliberate differentiation: one focused two-pane workspace with local-only ownership and explicit JSON backup/restore.

## Platform conventions
- Target platform(s): Windows 11 x64.
- Native/familiar conventions to preserve: standard window chrome, Windows file dialogs, Ctrl+N/Ctrl+F/Ctrl+S-style expectations, visible keyboard focus, Escape to cancel transient states.
- Intentional deviations and rationale: WPF custom resources provide consistent light/dark surfaces while retaining standard controls and UI Automation semantics.

## Design foundations
- Canonical token source: `src/QuickShelf/Styles/Tokens.xaml`.
- Typography: Segoe UI; 12-14 px body/control text, 20-24 px primary headings, readable line spacing.
- Color/semantic color: neutral Windows-like surfaces; one accent; dedicated success/warning/error tokens; never rely on color alone.
- Spacing/grid: 4 px base scale; common spacing 8/12/16/24; minimum 32 px actionable control height.
- Radius/elevation: modest 6-8 px radii; borders and subtle surface separation over heavy shadows.
- Iconography: Unicode/simple text symbols only where their meaning is obvious; pair ambiguous symbols with accessible labels/tooltips.
- Motion: minimal; no essential information communicated only through animation; honor reduced-motion expectations by avoiding decorative animation.
- Responsive/adaptive rules: minimum 760x520; list pane fixed within a useful range and editor consumes remaining width.

## Component strategy
- Primitive/component foundation: built-in WPF controls and styles only for v0.1.
- Product abstraction boundary: reusable application resources/styles plus narrow code-behind UI handlers; persistence, validation, search and theme behavior live in independently testable services/models.
- Accessibility rationale: standard WPF controls expose established keyboard/UI Automation behavior; explicit AutomationProperties are added where labels are otherwise ambiguous.
- Dependency/customization constraints: no third-party UI library for this small product; avoid bespoke controls unless native primitives cannot meet an accepted journey.

## UX patterns
- Navigation: one main workspace plus a lightweight Settings panel; no nested navigation.
- Forms/validation: autosave valid note text; import validates before mutation; search filters as the user types.
- Feedback/progress/errors: short status text for saved/exported/imported actions; recoverable dialogs for failures; never expose stack traces.
- Destructive/recovery behavior: delete and reset require explicit confirmation; reset names the consequence; import does not partially apply invalid data.
- Settings/advanced disclosure: theme and data actions only; technical paths/schema details stay out of normal UI.
- Onboarding/first value: no wizard. First launch shows a useful empty state with one clear Create first snippet action.

## Accessibility
- Target: WCAG 2.2 AA principles adapted to native Windows desktop plus Windows UI Automation compatibility.
- Input/focus expectations: all primary actions keyboard reachable; logical tab order; visible focus; Ctrl+N creates, Ctrl+F focuses search and destructive deletion remains explicit and confirmed.
- Motion/contrast/zoom expectations: sufficient text/background contrast, no color-only meaning, no required animation, WPF layout remains usable at common Windows display scaling.

## Research sources
- Microsoft Windows app design/navigation/usability guidance, reviewed 2026-09-03.
- Microsoft Sticky Notes support documentation, reviewed 2026-09-03.
- Microsoft WPF/.NET desktop documentation, reviewed 2026-09-03.
