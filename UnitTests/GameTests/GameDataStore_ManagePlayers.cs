using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Services;
using LotteryResources.Storage;
using Microsoft.Extensions.Options;

namespace UnitTests.GameTests
{
    [TestFixture]
    public class GameDataStore_ManagePlayers
    {
        private GameDataStore _gameDataStore;
        private GameConfiguration _gameConfiguration;

        [SetUp]
        public void SetUp()
        {
            _gameConfiguration = new GameConfiguration
            {

            };
            var options = Options.Create(_gameConfiguration);

            _gameDataStore = new GameDataStore(options, new RandomNumberService());
        }

        [Test]
        public void AddPlayer_WithTickets()
        {
            var playerList = new List<PlayerModel>()
            {
            new PlayerModel() { Name = "Player 1", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            new PlayerModel() { Name = "Player 2", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            new PlayerModel() { Name = "Player 3", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            };

            PopulatePlayers(playerList);

            Assert.AreEqual(_gameDataStore.Players.Count(), playerList.Count());
        }

        [Test]
        public void AddPlayer_WithOutTickets()
        {
            var playerList = new List<PlayerModel>()
            {
            new PlayerModel() { Name = "Player 1", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            new PlayerModel() { Name = "Player 2", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            new PlayerModel() { Name = "Player 3", IsCpu = false, Tickets = new List<Guid>(){}},
            };

            PopulatePlayers(playerList);

            Assert.AreEqual(_gameDataStore.Players.Count(), playerList.Where(x => x.Tickets.Count() > 0).Count());
        }


        [Test]
        public void ResetGame()
        {
            var playerList = new List<PlayerModel>()
            {
            new PlayerModel() { Name = "Player 1", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            new PlayerModel() { Name = "Player 2", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
            new PlayerModel() { Name = "Player 3", IsCpu = false, Tickets = new List<Guid>(){}},
            };

            PopulatePlayers(playerList);

            _gameDataStore.ResetGame();

            Assert.AreEqual(_gameDataStore.Players.Count(), 0);
        }

        private void PopulatePlayers(List<PlayerModel> players)
        {
            _gameDataStore.AddPlayers(players);
        }
    }
}
