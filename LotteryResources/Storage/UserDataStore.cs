using LotteryResources.Models.Configuration;
using LotteryResources.Storage.Interfaces;
using Microsoft.Extensions.Options;

namespace LotteryResources.Storage
{
    public class UserDataStore : IUserDataStore
    {
        private readonly PlayerConfiguration _playerConfiguration;
        public decimal UserBalance { get; private set; }
        public string PlayerName { get; private set; }

        public UserDataStore(IOptions<PlayerConfiguration> playerConfiguration)
        {
            _playerConfiguration = playerConfiguration.Value;

            // Get the initial user balance from appsettings.json
            if (_playerConfiguration != null)
            {
                UserBalance = _playerConfiguration.DefaultPlayerBalance;
                PlayerName = _playerConfiguration.DefaultPlayerName;
            }
            else
            {
                //Could possibly throw a setup exception here, but for simplicity I'll hard code it in the case where there is something wrong with appsettings
                UserBalance = 10;
                PlayerName = "Player 1";
            }

        }

        public void UpdateUserBalance(bool increment, decimal value)
        {
            //Update our 'stored' user balance
            if (increment)
                UserBalance += value;
            else
                UserBalance -= value;
        }

        public PlayerConfiguration GetPlayerData() 
        {
            return _playerConfiguration;
        }
    }
}
