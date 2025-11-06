using System;
using System.Collections.Generic;

namespace ImposterGame.Models
{
    public enum GameStatus { Lobby, Started, Ended }
     public enum GameModeType { Vanilla, Question }
    public enum Topic { FootballPlayers, Jobs, Countries }

    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string JoinCode { get; set; } = "";
        public GameStatus Status { get; set; } = GameStatus.Lobby;

        public string PlayerName { get; set; } = "";

        public Guid? ImposterPlayerId { get; set; }

        public List<Player> Players { get; set; } = new();

        public string SecretPlayerName { get; set; } = "";

        public bool isEnded => Status == GameStatus.Ended;

        public string? ImposterName => Players.FirstOrDefault(p => p.Id == ImposterPlayerId
        )?.DisplayName;

        public bool TryAddPlayer(string name)
        {
            if (Players.Any(p => p.DisplayName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return false;


            Players.Add(new Player { DisplayName = name, GameId = Id });
            return true;
        }

        public string ShortCode { get; set; } = string.Empty;

        public GameMode Mode { get; set; }= GameMode.Local;
        public Guid? HostPlayerId { get; set; }

        public Topic SelectedTopic { get; set; } = Topic.FootballPlayers;
        public GameModeType VariantMode { get; set; } = GameModeType.Vanilla;




    }
}

