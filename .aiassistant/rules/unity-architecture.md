---
apply: always
---

# Unity project architecture rules

## Project intent
This repository is a showcase-quality Unity pet project.
The code must look production-ready, reviewer-friendly, scalable, and maintainable.

## Core stack
- Unity C#
- Zenject
- UniTask
- DOTween
- Odin Inspector

## Architectural defaults
- Follow SOLID
- Prefer composition over inheritance
- Keep MonoBehaviours thin
- Keep business and gameplay logic outside views
- Views should render, play feedback, and forward events
- Prefer plain C# services, presenters, controllers, and use cases for logic
- Installers are for composition only
- Avoid service locator usage outside composition root
- Prefer explicit dependencies
- Prefer small focused classes

## Layering rules
- Domain and gameplay rules must not depend on Unity view details unless clearly justified
- Animation belongs to presentation/view layer unless there is a strong reason otherwise
- Systems such as inventory, resource logic, progression, quests, and economy should remain testable without scene objects
- Do not place business rules directly into view classes

## Zenject rules
- Prefer constructor injection in non-MonoBehaviour classes
- Use factories for dynamic object creation
- Evaluate pools for frequently spawned objects, temporary effects, and drops
- Use signals only when decoupling is actually needed
- Avoid using DiContainer directly outside installers and dedicated factories

## DOTween rules
- Prefer Sequence for authored gameplay feedback
- Link or kill tweens safely according to object lifetime
- Avoid orphan tweens
- Expose timings, amplitudes, punch strengths, offsets, and durations as serialized settings or config
- Avoid hidden looping tweens unless intentionally designed

## Code style requirements

### Naming
- Use PascalCase for classes, interfaces, methods, properties, delegates, and events
- All interfaces must start with `I`
- Use camelCase for non-static fields and method parameters
- Use PascalCase for static fields
- Treat abbreviations as normal words:
  - `XmlHttpRequest`
  - `url`
  - `FindPostById`
- Method names must be verbs
- Boolean fields, properties, and methods should answer a question and use names like `Is`, `Has`, `Can`
- Use meaningful full names, do not shorten names unnecessarily
- In compound container names, use plural first word:
  - `ObjectsPool`
  - `NodesList`

### Files and class layout
- Each new class should normally be placed in its own file
- Exception: a very small helper class tightly coupled to the main class may stay in the same file
- Member order inside a class:
  1. constants
  2. fields
  3. properties
  4. constructors or initialization methods
  5. methods
- Inside each category use this order:
  1. public
  2. protected
  3. private
- Static members go before instance members inside the same visibility group
- A paired private field and public property may stay close together when it improves readability

### Fields and properties
- Prefer private serialized fields for Inspector-only exposure:
  - `[SerializeField] private float moveSpeed;`
- Each variable declaration must be on its own line
- Always write explicit access modifiers

### Formatting
- Use Allman style braces
- Every opening brace must be on a new line
- Always use braces for conditionals, even for single-line bodies
- Exception: a single-line `return` may stay without braces if it improves readability and remains consistent
- Keep exactly one empty line between methods
- Use internal blank lines only to separate logical sections inside a method
- If a method needs too many visual sections, refactor it into smaller methods

### Switch statements
- Always include a functional `default` branch
- At minimum, log or otherwise handle unexpected state explicitly

### Attributes
- Attributes for fields and properties stay on the same line
- Attributes for classes and methods go on the line above

### Comments and documentation
- Write comments in English only
- Place comments above the code they describe, never inline at the end of a line
- Public classes, public methods, and public enums should use XML documentation comments
- Write comments only when they add information that is not obvious from good naming

### Constants and literals
- Avoid magic constants
- Numeric literals in code should normally be limited to:
  - `0`
  - `1`
  - `-1`
  - `2`
  - `0.5f` when calculating center or average
- In other cases, extract a named constant or config value
- Prefer serialized config/settings over hardcoded tuning numbers

### Code hygiene
- Remove dead code
- Do not keep large commented-out code blocks
- Do not leave temporary hacks without an explicit reason
- Keep methods focused and short
- Prefer readonly fields where possible
- Minimize allocations in gameplay hot paths
- Match existing folder and naming conventions

## Testing rules
- Non-trivial pure logic should have unit tests
- DI wiring and MonoBehaviour-driven feature behavior should have integration tests where useful
- Startup-critical scenes/features should have smoke coverage when justified

## Forbidden patterns
- No god classes
- No hidden singleton dependencies
- No business logic dumped into animation classes
- No broad unrelated refactors just to solve a local task
- No architecture shortcuts that reduce long-term readability

## Expected answer style
When proposing code changes:
1. Briefly explain the design choice
2. List files to create or modify
3. Provide implementation
4. Mention tests
5. Mention risks or follow-ups