using ImposterGame.Models;

namespace ImposterGame.ViewModels
{
    public class LobbyViewModel
    {
        public Guid GameId { get; set; }
        public GameStatus Status { get; set; }
        public List<Player> Players { get; set; } = new();
        public string? SharableLink { get; set; }
        public GameMode Mode { get; set; }
        public bool IsHost { get; set; }
        public string? ShortCode { get; set; }
        public Topic SelectedTopic { get; set; }
        public GameModeType VariantMode { get; set; }
    }
}