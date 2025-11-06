using System.Collections.Concurrent;
using ImposterGame.DAL;
using ImposterGame.Models;

namespace ImposterGame.DAL
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly ConcurrentDictionary<Guid, Game> _store = new();

        public Game CreateGame()
        {
            var game = new Game();
            game.ShortCode = GenerateShortCode();
            _store[game.Id] = game;
            return game;
        }

        public Game? Get(Guid id)
        {
            _store.TryGetValue(id, out var game);
            return game;
        }

        public void Save(Game game)
        {
            _store[game.Id] = game;
        }

        public Game? GetByShortCode(string shortCode)
        {
            return _store.Values.FirstOrDefault(g => g.ShortCode == shortCode);
        }

        private string GenerateShortCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            int length = random.Next(4, 7); 
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}