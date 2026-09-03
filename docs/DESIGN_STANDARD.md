# Product Design and UI/UX Standard

Applies to any project with a user interface. The template defines a professional design process; each derived project owns its own design language.

## Required design sequence
Do not begin broad screen styling from an unconstrained model preference. Work in this order:
1. Product intent and primary user tasks.
2. Current reference-class and target-platform research.
3. Brand/creative inputs and explicit user preferences.
4. Information architecture and critical journeys.
5. Product design direction and interaction principles.
6. Design tokens and component architecture.
7. Reusable UX patterns.
8. Screens/flows and responsive behavior.
9. Visual, accessibility and maturity verification.

## Reference-class research
Identify credible professional products that solve comparable problems and authoritative platform guidance. Study navigation, hierarchy, density, terminology, setup, errors, settings, keyboard/touch patterns and lifecycle experience. Extract principles; never copy protected brand expression or assume competitor behavior is correct without evaluating product fit.

## Brand and creative input
The user may provide a complete brand system, only a logo/color, screenshots, websites, imagery, likes/dislikes or raw natural-language impressions. Treat these as design signals. Translate them into a coherent software design language instead of literally reproducing references or applying a brand color everywhere.

## Project design source of truth
Before broad UI implementation, create the manifest-referenced project design system (normally `DESIGN_SYSTEM.md`). It records product character, brand inputs, reference-class conclusions, platform conventions, density, typography, color, spacing, radius/elevation, iconography, motion, component strategy, recurring UX patterns, accessibility expectations and durable explicit preferences.

A new session reads this source before changing UI. It may not introduce a conflicting design direction merely because another style appears attractive. Material design changes update the design source and rationale first.

## Design tokens
Use a canonical token source appropriate to the stack for color, typography, spacing, radius, elevation, motion and breakpoints where applicable. Feature code should consume tokens rather than inventing one-off values. New tokens require a system-level reason, not local convenience.

## Component strategy
Do not hard-code one component library into this template. Select foundations per project using platform fit, accessibility maturity, maintenance, dependency risk, component coverage, customization, performance and brand flexibility.

Prefer the principle: **reuse proven interaction behavior; own the product design**. When practical, wrap third-party primitives behind product components so feature code depends on the product design system rather than a vendor's visual API.

Do not rebuild difficult interaction primitives from scratch without reason, but do not let a third-party library define the product's identity by accident.

## Product patterns
Components alone do not create coherent UX. Define reusable patterns for recurring flows such as forms, save/autosave, destructive actions, undo, search/filtering, permissions, connection setup, import/export, progress, empty states, recovery, settings, onboarding, updates and unsaved changes when applicable.

## Required states and platform fit
Every interactive flow deliberately handles normal, hover/focus/disabled, loading, empty, validation, success, error, timeout/offline and destructive states when applicable. Follow target-platform conventions for windowing, navigation, input, menus, permissions, notifications and system integration unless a documented product reason justifies deviation.

## Accessibility and responsive quality
Keyboard navigation, visible focus, semantic structure, contrast, target sizes and reduced-motion behavior are first-class requirements. Web products target WCAG 2.2 AA unless stricter requirements apply. Responsive/adaptive behavior is designed deliberately rather than patched after desktop completion.

## Visual verification
For web UI, use Playwright semantic locators to exercise critical flows and capture representative viewport screenshots. Inspect browser console/network failures and maintain visual regression baselines once stable. Other platforms use the strongest available semantic/test interface plus screenshots or equivalent visual evidence.

## Professional maturity review
ChatGPT must visually inspect representative final surfaces and ask whether hierarchy, density, alignment, typography, component consistency, brand integration, platform fit, microcopy and interaction polish show a material maturity gap versus the chosen reference class. Fix unexplained gaps before release.

A UI feature is not done merely because it functions, passes DOM tests or looks internally consistent. It must preserve the project design system and meet the professional product experience standard.