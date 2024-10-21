using LotteryResources.Models.Players;

namespace LotteryResources.Services.Interfaces
{
    public interface IGameService
    {
        void ListCpuPlayers();
        void ProcessResults(decimal ticketRevenue);
        void AddPlayers(List<PlayerModel> players);
    }
}