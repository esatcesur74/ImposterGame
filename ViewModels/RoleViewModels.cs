namespace ImposterGame.ViewModels
{
    public class RoleViewModel
    {
        public Guid GameId { get; set; }

        public string PlayerName { get; set; } = "";

        public bool IsImposter { get; set; }

        public string? Secret { get; set; }
    }
}
