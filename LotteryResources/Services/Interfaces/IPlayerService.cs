using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Models.Tickets;

namespace LotteryResources.Services.Interfaces
{
    public interface IPlayerService
    {
        void DisplayWelcome();
        decimal PromptTicketPurchase();
        void UpdateBalance(bool increment, decimal value);
        PlayerModel GeneratePlayerForGame(List<Guid> tickets, decimal ticketCost);
    }
}