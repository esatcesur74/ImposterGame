using ImposterGame.DAL;
using Microsoft.AspNetCore.Mvc;
using ImposterGame.Models;
using Microsoft.AspNetCore.DataProtection;
using System.Linq;
using ImposterGame.ViewModels;
using Microsoft.VisualBasic;
using ImposterGame.Services;



namespace ImposterGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRepository _repo;
        private readonly INetworkService _networkService;  // Add this line

        public GameController(IGameRepository repo, INetworkService networkService)
        {
            _repo = repo;
            _networkService = networkService;
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            var game = _repo.CreateGame();
            game.Mode = GameMode.Local;
            _repo.Save(game);
            return RedirectToAction(nameof(Lobby), new { id = game.Id });
        }
        [HttpGet]
        public IActionResult Lobby(Guid id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();

            // If game has ended and we're returning to lobby, reset to Lobby status
            if (game.Status == GameStatus.Ended)
            {
                game.Status = GameStatus.Lobby;
                _repo.Save(game);
            }

            var localIP = _networkService.GetLocalIPAddress();
            var port = HttpContext.Request.Host.Port ?? 5000;
            var sharableLink = $"http://{localIP}:{port}/Game/Join/{game.Id}";
            bool isHost = game.Mode == GameMode.Local;
            if (game.Mode == GameMode.Remote)
            {
                if (Request.Cookies.TryGetValue($"Player_{id}", out string? playerIdStr)
                    && Guid.TryParse(playerIdStr, out Guid playerId))
                {
                    isHost = playerId == game.HostPlayerId;
                }
                else
                {
                    // If no cookie, default to false (non-host)
                    isHost = false;
                }
            }

            var vm = new LobbyViewModel
            {
                GameId = game.Id,
                Status = game.Status,
                Players = game.Players,
                SharableLink = sharableLink,
                Mode = game.Mode,
                IsHost = isHost,
                ShortCode = game.ShortCode,
                SelectedTopic = game.SelectedTopic,
                VariantMode = game.VariantMode
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPlayer(Guid id, string displayName)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();

            displayName = (displayName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(displayName))
            {
                TempData["Error"] = "Name cannot be empty.";
                return RedirectToAction(nameof(Lobby), new { id });
            }
            if (game.Players.Count >= 8)
            {
                TempData["Error"] = "Max 8 players.";
                return RedirectToAction(nameof(Lobby), new { id });
            }
            if (!game.TryAddPlayer(displayName))
            {
                TempData["Error"] = "That nickname is already taken.";
                return RedirectToAction(nameof(Lobby), new { id });
            }
            _repo.Save(game);

            return RedirectToAction(nameof(Lobby), new { id });



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePlayer(Guid id, Guid playerId)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null) return RedirectToAction(nameof(Lobby), new { id });
            game.Players.Remove(player);
            _repo.Save(game);
            return RedirectToAction(nameof(Lobby), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTopic(Guid id, string topic)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();


            bool isHost = game.Mode == GameMode.Local;
            if (game.Mode == GameMode.Remote)
            {
                if (Request.Cookies.TryGetValue($"Player_{id}", out string? playerIdStr)
                    && Guid.TryParse(playerIdStr, out Guid playerId))
                {
                    isHost = playerId == game.HostPlayerId;
                }
            }

            if (!isHost)
            {
                TempData["Error"] = "Only the host can change game settings!";
                return RedirectToAction(nameof(Lobby), new { id });
            }


            if (game.Status != GameStatus.Lobby)
            {
                TempData["Error"] = "Cannot change settings after game has started!";
                return RedirectToAction(nameof(Lobby), new { id });
            }


            try
            {
                game.SelectedTopic = Enum.Parse<Topic>(topic);
                _repo.Save(game);
            }
            catch
            {
                TempData["Error"] = "Invalid topic selected!";
            }

            return RedirectToAction(nameof(Lobby), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartGame(Guid id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();

            if (game.Players.Count < 3)
            {
                TempData["Error"] = "Need at least 3 players to start!";
                return RedirectToAction(nameof(Lobby), new { id });
            }

            if (game.Status == GameStatus.Started)
            {
                TempData["Error"] = "Already started";
                return RedirectToAction(nameof(Lobby), new { id });
            }
            else
            {
                game.Status = GameStatus.Started;
                var rnd = new Random();
                game.ImposterPlayerId = game.Players[rnd.Next(game.Players.Count)].Id;
                string[] footballers = {
    "Lionel Messi", "Cristiano Ronaldo", "Kylian Mbappe", "Neymar Jr", "Erling Haaland",
    "Kevin De Bruyne", "Mohamed Salah", "Robert Lewandowski", "Karim Benzema", "Harry Kane",
    "Vinicius Jr", "Jude Bellingham", "Luka Modric", "Antoine Griezmann", "Pedri",
    "Sadio Mane", "Marcus Rashford", "Bukayo Saka", "Paulo Dybala", "Riyad Mahrez",
    "Son Heung-min", "Bruno Fernandes", "Jack Grealish", "Phil Foden", "Frenkie de Jong",
    "Gavi", "Virgil van Dijk", "Thiago Silva", "Casemiro", "Olivier Giroud", "Declan Rice",
    "Rodri", "Bernardo Silva", "Joshua Kimmich", "Thibaut Courtois", "Marc-Andre ter Stegen",
    "Alisson Becker", "Ederson", "Achraf Hakimi", "Jamal Musiala", "Lautaro Martinez",
    "Pelé", "Diego Maradona", "Zinedine Zidane", "Ronaldinho", "Ronaldo Nazário",
    "David Beckham", "Thierry Henry", "Xavi Hernandez", "Andres Iniesta", "Francesco Totti",
    "Paolo Maldini", "Andrea Pirlo", "Kaka", "Wayne Rooney", "Didier Drogba",
    "Samuel Eto'o", "Iker Casillas", "Sergio Ramos", "Fernando Torres", "Carlos Puyol",
    "Eden Hazard", "Angel Di Maria", "Karim Adeyemi", "Joao Felix", "Kai Havertz",
    "Toni Kroos", "Leroy Sane", "Ilkay Gundogan", "Rafael Leao", "Gabriel Jesus",
    "Gabriel Martinelli", "Rodrygo", "Federico Valverde", "Aurelien Tchouameni", "Eduardo Camavinga",
    "Raphael Varane", "Kalidou Koulibaly", "Trent Alexander-Arnold", "Andrew Robertson", "Reece James",
    "Jadon Sancho", "Mason Mount", "Declan Gallagher", "N’Golo Kante", "Paul Pogba",
    "Gerard Pique", "Carlos Tevez", "Luis Suarez", "Edinson Cavani", "Gonzalo Higuain",
    "David Villa", "Arjen Robben", "Frank Lampard", "Steven Gerrard", "Patrick Vieira",
    "Clarence Seedorf", "Ruud van Nistelrooy", "George Best", "Eric Cantona", "Roberto Carlos"
};
                string[] jobs ={

                    "Doctor", "Teacher", "Engineer", "Chef", "Artist", "Pilot", "Nurse", "Lawyer", "Police Officer", "Firefighter",
    "Scientist", "Dentist", "Architect", "Programmer", "Pharmacist", "Accountant", "Electrician", "Plumber", "Mechanic", "Journalist",
    "Designer", "Photographer", "Psychologist", "Veterinarian", "Farmer", "Real Estate Agent", "Entrepreneur", "Translator", "Barista", "Athlete"
                };
                string[] countries =
                {
                        "United States", "United Kingdom", "Germany", "France", "Italy", "Spain", "Portugal", "Brazil", "Argentina", "Mexico",
    "Canada", "Australia", "Japan", "South Korea", "China", "India", "Norway", "Sweden", "Denmark", "Netherlands",
    "Switzerland", "Belgium", "Poland", "Greece", "Turkey", "Saudi Arabia", "United Arab Emirates", "South Africa", "Egypt", "Thailand",
    "Indonesia", "Vietnam", "Philippines", "Malaysia", "Singapore", "New Zealand", "Russia", "Ukraine", "Czech Republic", "Austria",
    "Hungary", "Finland", "Ireland", "Scotland", "Wales", "Croatia", "Serbia", "Romania", "Bulgaria", "Morocco",
    "Tunisia", "Nigeria", "Ghana", "Kenya", "Chile", "Colombia", "Peru", "Venezuela", "Uruguay", "Paraguay"
                };

                string[] selectedArray = footballers;
                switch (game.SelectedTopic)
                {
                    case Topic.FootballPlayers:
                        selectedArray = footballers;
                        break;
                    case Topic.Jobs:
                        selectedArray = jobs;
                        break;
                    case Topic.Countries:
                        selectedArray = countries;
                        break;
                }


                game.SecretPlayerName = selectedArray[rnd.Next(selectedArray.Length)];

                game.Status = GameStatus.Started;

                _repo.Save(game);

                return RedirectToAction(nameof(Play), new { id });
            }
        }

        [HttpGet]

        public IActionResult Play(Guid id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();
            if (game.Status != GameStatus.Started)
            {
                TempData["Error"] = "Start the game first.";
                return RedirectToAction(nameof(Lobby), new { id });
            }

            // For Remote mode, only show the current player's card
            List<Player> playersToShow = game.Players;

            if (game.Mode == GameMode.Remote)
            {
                if (Request.Cookies.TryGetValue($"Player_{id}", out string? playerIdStr)
                    && Guid.TryParse(playerIdStr, out Guid playerId))
                {
                    var currentPlayer = game.Players.FirstOrDefault(p => p.Id == playerId);
                    if (currentPlayer != null)
                    {
                        playersToShow = new List<Player> { currentPlayer };
                    }
                }
            }

            var vm = new PlayViewModel
            {
                GameId = game.Id,
                Status = game.Status,
                Mode = game.Mode,
                Players = playersToShow,  
                EndGame = game.isEnded,
                ImposterName = game.ImposterName,
                ImposterPlayerId = game.ImposterPlayerId,
                Secret = game.SecretPlayerName
            };
            return View(vm);
        }

        [HttpGet]

        public IActionResult Role(Guid id, Guid playerId)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();

            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null) return RedirectToAction(nameof(Play), new { id });

            var isImposter = game.ImposterPlayerId == playerId;
            var vm = new RoleViewModel
            {
                GameId = id,
                PlayerName = player.DisplayName,
                IsImposter = isImposter,
                Secret = isImposter ? null : game.SecretPlayerName
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EndGame(Guid id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();
            game.Status = GameStatus.Ended;

            _repo.Save(game);
            return RedirectToAction(nameof(GameOver), new { id });
        }

        [HttpGet]
        public IActionResult GameOver(Guid id)
        {
            var game = _repo.Get(id);
            if (game == null) return NotFound();

            // Make sure the game has actually ended
            if (game.Status != GameStatus.Ended)
            {
                return RedirectToAction(nameof(Play), new { id });
            }

            // Get the imposter player
            var imposter = game.Players.FirstOrDefault(p => p.Id == game.ImposterPlayerId);
            if (imposter == null)
            {
                TempData["Error"] = "Could not find imposter data!";
                return RedirectToAction(nameof(Lobby), new { id });
            }

            var vm = new EndGameViewModel
            {
                GameId = game.Id,
                ImposterName = imposter.DisplayName,
                ImposterPlayerId = imposter.Id,
                SecretPlayerName = game.SecretPlayerName,
                AllPlayers = game.Players,
                SelectedTopic = game.SelectedTopic
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRemote(string hostName)
        {
            var game = _repo.CreateGame();
            if (!game.TryAddPlayer(hostName))
            {
                TempData["Error"] = "Failed to add host player.";
                return RedirectToAction(nameof(New));
            }
            game.Mode = GameMode.Remote;
            game.HostPlayerId = game.Players[0].Id;
            _repo.Save(game);

            // Store the host player ID in a cookie
            Response.Cookies.Append($"Player_{game.Id}", game.Players[0].Id.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(1)
            });

            return RedirectToAction(nameof(Lobby), new { id = game.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult JoinRemote(Guid id, string playerName)
        {
            // Validate player name
            if (string.IsNullOrWhiteSpace(playerName))
            {
                TempData["Error"] = "Player name cannot be empty.";
                return RedirectToAction(nameof(Join), new { id });
            }

            playerName = playerName.Trim();

            // Get the game
            var game = _repo.Get(id);
            if (game == null)
            {
                TempData["Error"] = "Game not found.";
                return RedirectToAction(nameof(Join), new { id });
            }

            // Check if game has already started
            if (game.Status == GameStatus.Started)
            {
                TempData["Error"] = "This game has already started.";
                return RedirectToAction(nameof(Join), new { id });
            }

            // Check player count
            if (game.Players.Count >= 8)
            {
                TempData["Error"] = "Game is full (maximum 8 players).";
                return RedirectToAction(nameof(Join), new { id });
            }

            // Try to add player
            if (!game.TryAddPlayer(playerName))
            {
                TempData["Error"] = "That nickname is already taken in this game.";
                return RedirectToAction(nameof(Join), new { id });
            }

            // Store the player ID in a cookie
            var newPlayer = game.Players.Last(); // Get the player we just added
            Response.Cookies.Append($"Player_{id}", newPlayer.Id.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(1)
            });

            _repo.Save(game);
            return RedirectToAction(nameof(Lobby), new { id });
        }
        private string GenerateMockMatchId()
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpGet]
        public IActionResult DebugLobby()
        {
            return Json(new
            {
                matchId = "DEBUG1234",
                status = "active",
                players = new[] { "player1" },
                minPlayers = 3,
                maxPlayers = 5
            });
        }

        [HttpGet]
        public IActionResult Join(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                var errorVm = new JoinViewModel
                {
                    GameId = Guid.Empty,
                    ErrorMessage = "No game ID provided."
                };
                return View(errorVm);
            }

            Game? game = null;


            if (Guid.TryParse(id, out Guid gameId))
            {
                game = _repo.Get(gameId);
            }
            else
            {

                game = _repo.GetByShortCode(id.ToUpper());
            }


            if (game == null)
            {
                var vm = new JoinViewModel
                {
                    GameId = Guid.Empty,
                    ErrorMessage = "Game not found. The game may have expired or the link is invalid."
                };
                return View(vm);
            }


            if (game.Status == GameStatus.Started)
            {
                var vm = new JoinViewModel
                {
                    GameId = game.Id,
                    ErrorMessage = "This game has already started. You cannot join now"
                };
                return View(vm);
            }


            if (game.Players.Count >= 8)
            {
                var vm = new JoinViewModel
                {
                    GameId = game.Id,
                    ErrorMessage = "Game is full."
                };
                return View(vm);
            }


            var viewModel = new JoinViewModel
            {
                GameId = game.Id,
                ErrorMessage = null
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GetLobbyData(Guid id)
        {
            var game = _repo.Get(id);
            if (game == null)
            {
                return Json(new { sucess = false, message = "Game not found" });
            }
            bool isHost = game.Mode == GameMode.Local;
            if (game.Mode == GameMode.Remote)
            {
                if (Request.Cookies.TryGetValue($"Player_{id}", out string? playerIdStr)
                && Guid.TryParse(playerIdStr, out Guid playerId))
                {
                    isHost = playerId == game.HostPlayerId;
                }

            }
            return Json(new
            {
                success = true,
                players = game.Players.Select(p => new
                {
                    id = p.Id,
                    displayName = p.DisplayName
                }),
                playerCount = game.Players.Count,
                status = game.Status.ToString(),
                isHost = isHost,
                mode = game.Mode.ToString()

            });
        }




    }



}