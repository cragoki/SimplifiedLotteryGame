using LotteryResources.Models.Players;

namespace LotteryResources.Storage.Interfaces
{
    public interface IGameDataStore
    {
        List<PlayerModel> Players { get; }
        void ResetGame();
        void AddPlayers(List<PlayerModel> players);
        PlayerModel GenerateGrandPrizeWinner();
        List<PlayerModel> GenerateSecondTierWinners();
        List<PlayerModel> GenerateThirdTierWinners();
        decimal CalculateGrandPrize(decimal ticketRevenue);
        decimal CalculateSecondTier(decimal ticketRevenue);
        decimal CalculateThirdTier(decimal ticketRevenue);
        Guid SelectRandomTicket();
    }
}