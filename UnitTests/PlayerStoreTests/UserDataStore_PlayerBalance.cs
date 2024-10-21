using LotteryResources.Models.Configuration;
using LotteryResources.Storage;
using Microsoft.Extensions.Options;

namespace UnitTests.PlayerStoreTests
{
    [TestFixture]
    public class UserDataStore_PlayerBalance
    {
        private UserDataStore _userDataStore;
        private PlayerConfiguration _playerConfiguration;

        [SetUp]
        public void SetUp()
        {
            _playerConfiguration = new PlayerConfiguration
            {
                DefaultPlayerName = "Player 1",
                DefaultPlayerBalance = 10
            };
            var options = Options.Create(_playerConfiguration);

            _userDataStore = new UserDataStore(options);
        }

        [Test]
        public void ValidateTicketInputAgainstBalance_ReduceBalance()
        {
            decimal value = 5;
            _userDataStore.UpdateUserBalance(false, value);

            Assert.AreEqual(_userDataStore.UserBalance, _playerConfiguration.DefaultPlayerBalance - value);
        }

        [Test]
        public void ValidateTicketInputAgainstBalance_IncreaseBalance()
        {
            decimal value = 5;
            _userDataStore.UpdateUserBalance(true, value);

            Assert.AreEqual(_userDataStore.UserBalance, _playerConfiguration.DefaultPlayerBalance + value);
        }
    }
}
