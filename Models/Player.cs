namespace ImposterGame.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GameId { get; set; }
        public string DisplayName { get; set; } = "";
    }
}