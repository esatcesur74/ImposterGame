using ImposterGame.Models;

namespace ImposterGame.ViewModels
{
    public class PlayViewModel
    {
        public Guid GameId { get; set; }
        public GameStatus Status { get; set; }
        public GameMode Mode { get; set; }
        public List<Player> Players { get; set; } = new();
        public bool EndGame { get; set; }
        public string? ImposterName { get; set; }

        public Guid? ImposterPlayerId { get; set; }
        public string? Secret { get; set; }
    }
}