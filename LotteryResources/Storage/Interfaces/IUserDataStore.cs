using LotteryResources.Models.Configuration;

namespace LotteryResources.Storage.Interfaces
{
    public interface IUserDataStore
    {
        decimal UserBalance { get; }
        string PlayerName { get; }

        void UpdateUserBalance(bool increment, decimal value);
        PlayerConfiguration GetPlayerData();
    }
}