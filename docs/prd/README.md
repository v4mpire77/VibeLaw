# Vibe Law Product Requirements Document (PRD)

## 1. Goals and Background Context

### 1.1 Goals
- Create a successful indie game in the cozy life-sim genre with a unique professional theme.
- Ensure players feel a tangible sense of "cozy progression" as their law office grows and improves.
- Encourage investment in building the player’s professional network and relationship with their spouse/assistant.
- Deliver strategic legal gameplay that is accessible, engaging, and a fun translation of the genre inspiration.

### 1.2 Background Context
The cozy life-simulation genre is saturated with farming and crafting experiences. **Vibe Law** adapts the genre’s satisfying progression and relationship mechanics to the novel setting of a small, independent law office. Players fulfill a new fantasy: building a respected and successful business from scratch in a low-stress, rewarding environment.

### 1.3 Change Log
| Date       | Version | Description        | Author |
|------------|---------|--------------------|--------|
| 2025-09-24 | 1.0     | Initial PRD Draft  | John (PM) |

## 2. Requirements

### 2.1 Functional Requirements
- **FR1**: The system shall allow players to select and accept legal cases from a central Case Board.
- **FR2**: The player shall have a finite energy resource that is consumed by performing work-related tasks.
- **FR3**: Making progress on cases shall consume the player's energy.
- **FR4**: Successfully resolving a case shall reward the player with in-game currency (Fees) and experience points (Reputation).
- **FR5**: The player's office environment shall be upgradable using in-game currency.
- **FR6**: The system shall feature a playable character who can move around and interact with the game world.

### 2.2 Non-Functional Requirements
- **NFR1**: The game must maintain a consistent 8-bit pixel art aesthetic.
- **NFR2**: The core gameplay loop must feel cozy and low-stress, avoiding high-pressure timers or fail states in the main management loop.
- **NFR3**: The game's architecture shall be modular to support adding major gameplay systems (Litigation, Networking) post-MVP.
- **NFR4**: The game must be playable on PC (Steam).

## 3. User Interface Design Goals

### 3.1 Overall UX Vision
Create a clear, intuitive interface evoking a classic 8-bit RPG. Menus should be accessible and easy to navigate, enabling efficient management of cases, relationships, and office upgrades.

### 3.2 Key Interaction Paradigms
- Direct character control (top-down view) for exploring the office and city.
- Cursor/controller-based menu interface for management tasks (e.g., interacting with the Case Board).

### 3.3 Core Screens and Views
1. Main Menu / Character Selection
2. The Office (main gameplay hub)
3. Case Board UI
4. The City (exploration hub)
5. Pause / Inventory Menu

### 3.4 Accessibility and Branding
- Target WCAG AA compliance: text contrast, clear navigation, remappable controls where feasible.
- Maintain a clean, friendly 8-bit pixel art style throughout.

### 3.5 Target Device and Platforms
- Primary: PC (Steam).
- Secondary: Design responsively to enable future console adaptations (especially Nintendo Switch).

## 4. Technical Assumptions
- **Repository Structure**: Polyrepo. The game client resides in its own repository; future online services (if any) will live separately.
- **Service Architecture**: Monolith single executable—standard for indie games at this scope.
- **Testing Requirements**: Combination of unit and integration tests for core logic (case resolution, rewards calculation) plus manual playtesting for UX and feel.
- **Multiplayer**: Single-player only for MVP and initial post-MVP phases.
- **Engine**: Final engine choice (MonoGame, Godot, GameMaker, etc.) to be determined by the Architect, but modularity must be preserved regardless.

## 5. Epic List

### Epic 1 – Foundation & Core Case Loop
Deliver the MVP experience: start the game, choose a character, accept, work on, and resolve cases for rewards.

#### Story 1.1 – Project Initialization
**As a** developer
**I want** to set up the initial project in the chosen engine
**So that** we have a version-controlled foundation.
- Acceptance Criteria:
  1. Empty, runnable project created in the chosen engine.
  2. Project committed to Git repository.
  3. Basic folder structure for scenes, scripts, and assets established.

#### Story 1.2 – Character Selection & Movement
**As a** player
**I want** to choose between Omar and Maham and control them in a test scene
**So that** I can interact with the fundamental game world.
- Acceptance Criteria:
  1. Start screen allows selection of Omar or Maham.
  2. Chosen character appears in a simple scene.
  3. Player can move in four directions with keyboard controls.
  4. Character has simple 8-bit walking animations per direction.

#### Story 1.3 – The Office & Case Board UI
**As a** player
**I want** to see my starting office and interact with a Case Board UI
**So that** I can view available cases.
- Acceptance Criteria:
  1. Player starts in the main office scene.
  2. Office contains a static Case Board object.
  3. Interacting with the Case Board opens a UI menu.
  4. Menu lists mock available cases with title and fee amount.

#### Story 1.4 – Accepting & Working on Cases
**As a** player
**I want** to accept a case and spend energy working on it
**So that** I can make progress toward completion.
- Acceptance Criteria:
  1. Player can accept a case from the Case Board UI.
  2. Accepted cases move to an "Active Cases" section.
  3. Visible energy bar tracks remaining energy.
  4. "Work on Case" action decreases energy and increases case progress.
  5. Action disabled when energy is depleted.

#### Story 1.5 – Case Resolution & Rewards
**As a** player
**I want** completed cases to grant money and reputation
**So that** progression feels rewarding.
- Acceptance Criteria:
  1. Full progress marks the case complete.
  2. Completion summary displayed (e.g., "Case Closed! +$500").
  3. Money and reputation totals update visibly.
  4. Completed case removed from Active list.
  5. Energy replenishes at the start of a new day (e.g., via "Sleep").

### Epic 2 – The Litigation Labyrinth
Introduce courthouse location and turn-based legal "combat" against obstacles, with rewards for success.

### Epic 3 – The Living City
Implement networking and intel systems: explorable city hub, professional NPC cast, relationship mechanics.

### Epic 4 – Progression & Polish
Expand long-term progression: office upgrades, work-life balance mechanics, polish (audio, UI, settings).

## 6. Next Steps for Downstream Agents
- **UX Expert Prompt**: "Based on this PRD, create a detailed UI/UX specification for 'Vibe Law', focusing on the core screens (Main Menu, Office) and the Case Board interaction flow for the MVP."
- **Architect Prompt**: "Based on this PRD, create a full-stack architecture document for 'Vibe Law', honoring the technical assumptions (engine TBD, monolith, polyrepo) and modular design requirements for post-MVP epics."

