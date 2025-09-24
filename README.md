# VibeLaw
A Stardew Valley inspired game built with MonoGame and .NET 8.

## Getting Started

### Prerequisites
- .NET 8 SDK

### Running the Game
```bash
dotnet run
```

### Building the Game
```bash
dotnet build
```

## Project Structure

```
VibeLaw/
├── Content/                 # Game assets
│   ├── sprites/            # Character and object sprites
│   ├── tilesets/           # Tileset images
│   ├── audio/              # Sound effects and music
│   ├── maps/               # Level maps
│   └── data/               # Game data files
├── Game/                    # Game code organization
│   ├── Core/               # Core game systems
│   ├── World/              # World and environment
│   ├── Entities/           # Game entities (player, NPCs)
│   ├── Items/              # Item system
│   ├── UI/                 # User interface
│   └── Save/               # Save/load system
├── Game1.cs                # Main game class
└── Program.cs              # Entry point
```

## Features
- Basic game loop with Update and Draw methods
- Player sprite rendering at position (100, 100)
- Content pipeline setup for sprites, tilesets, audio, and maps
- Organized folder structure for future development
