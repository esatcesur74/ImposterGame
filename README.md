# ImposterGame

# Imposter Game

A social deduction web game built with ASP.NET Core MVC where players try to identify the imposter among them!

## Game Overview

**Imposter Game** is inspired by popular social deduction games. Players are assigned secret identities from a chosen topic (Football Players, Jobs, or Countries), except for one player - the imposter - who doesn't know which identity they're supposed to have. Through discussion and questions, players must work together to identify who the imposter is!

## Features

- **Multiple Game Modes**
  - **Local Play**: Pass-and-play mode for groups playing on the same device
  - **Remote Play**: Online multiplayer with shareable game codes and links

- **Topic Selection**
  - Football Players
  - Jobs
  - Countries

- ** UI**
  - Newspaper-themed lobby design
  - Footballer trading card-style role reveals
  - Retro/cyberpunk aesthetic
  - Responsive design for mobile and desktop

- **Game Flow**
  1. Create a lobby (local or remote)
  2. Add players (3+ required)
  3. Select a topic
  4. Start the game
  5. Each player reveals their role by tapping their card
  6. Players discuss and try to find the imposter
  7. Game ends with imposter reveal

## Tech Stack

- **Backend**: ASP.NET Core 8.0 MVC
- **Frontend**: Razor Views, React (for dynamic components)
- **Styling**: Custom CSS with newspaper/trading card themes
- **Real-time Updates**: Polling-based updates for multiplayer

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- A modern web browser

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/esatcesur74/ImposterGame.git
   cd ImposterGame
   ```

2. Navigate to the project directory:
   ```bash
   cd ImposterGame
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   ```
   https://localhost:5001
   ```

   

## How to Play

### Local Mode
1. Click "LOCAL PLAY" on the home screen
2. Add player names (minimum 3 players)
3. Select a topic from the configuration panel
4. Click "START GAME"
5. Pass the device to each player to reveal their role
6. Players discuss and try to identify the imposter
7. End the game to reveal the truth!

### Remote Mode
1. Click "CREATE REMOTE LOBBY"
2. Enter your nickname
3. Share the game code or link with other players
4. Wait for players to join
5. Select a topic and start the game
6. Each player views their role on their own device
7. Discuss and find the imposter!

For remote mode all the players need to be in same network in order to play.

## Project Structure

```
ImposterGame/
├── Controllers/         # MVC Controllers
│   └── GameController.cs
├── Models/             # Data models
│   ├── Game.cs
│   ├── Player.cs
│   └── Enums (GameStatus, GameMode, Topic)
├── Services/           # Game logic
│   ├── IGameService.cs
│   └── GameService.cs
├── ViewModels/         # View-specific models
├── Views/              # Razor views
│   ├── Game/
│   │   ├── New.cshtml        # Home/create game
│   │   ├── Join.cshtml       # Join remote game
│   │   ├── Lobby.cshtml      # Lobby with player list
│   │   ├── Play.cshtml       # Main game screen
│   │   ├── Role.cshtml       # Individual role reveal
│   │   └── GameOver.cshtml   # End game screen
│   └── Shared/
│       └── _Layout.cshtml
└── wwwroot/            # Static files
    ├── css/
    │   ├── game-additions.css    # Card & component styles
    │   └── retro-lobby.css       # Newspaper theme
    └── js/
```

## Game Mechanics

### Roles
- **Crewmates**: Each crewmate is assigned a secret identity (e.g., "Cristiano Ronaldo", "Teacher", "France")
- **Imposter**: The imposter sees "ELIMINATE" instead of a specific identity

### Winning
- **Crewmates win** by correctly identifying the imposter through discussion
- **Imposter wins** by blending in and avoiding detection

### Topics
Each game uses one of three topics:
- **Football Players**: Famous soccer players from around the world
- **Jobs**: Common professions and occupations
- **Countries**: Countries from different continents

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the MIT License.

## Acknowledgments

- Inspired by social deduction games like Among Us and The Resistance
- Avatar generation powered by [DiceBear Avatars](https://dicebear.com/)
- React for dynamic UI components

## Contact

For questions or feedback, please open an issue on GitHub.

---

**Enjoy playing Imposter Game!** 🎮
