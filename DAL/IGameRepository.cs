using ImposterGame.Models;

namespace ImposterGame.DAL
{
    public interface IGameRepository
    {
        Game CreateGame();
        Game? Get(Guid id);
        Game? GetByShortCode(string shortCode);
        void Save(Game game);
    }
}