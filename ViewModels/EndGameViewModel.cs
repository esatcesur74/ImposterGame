using ImposterGame.Models;

namespace ImposterGame.ViewModels
{
    public class EndGameViewModel
    {
        public Guid GameId { get; set; }
        public string ImposterName { get; set; } = "";
        public Guid ImposterPlayerId { get; set; }
        public string SecretPlayerName { get; set; } = "";
        public List<Player> AllPlayers { get; set; } = new();
        public Topic SelectedTopic { get; set; }
    }
}