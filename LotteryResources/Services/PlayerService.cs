using LotteryResources.Models.Players;
using LotteryResources.Services.Interfaces;
using LotteryResources.Storage.Interfaces;

namespace LotteryResources.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUserDataStore _userDataStore;
        public PlayerService(IUserDataStore userDataStore)
        {
            _userDataStore = userDataStore;
        }

        public void DisplayWelcome()
        {
            Console.WriteLine($"Welcome to the Bede Lottery, {_userDataStore.PlayerName}");
            Console.WriteLine();
            Console.WriteLine($"* Your digital balance is ${_userDataStore.UserBalance}");
        }

        public decimal PromptTicketPurchase()
        {
            Console.WriteLine($"How many tickets do you want to buy, {_userDataStore.PlayerName}?");
            return _userDataStore.UserBalance;
        }

        public void UpdateBalance(bool increment, decimal value)
        {
            _userDataStore.UpdateUserBalance(increment, value);
        }

        public PlayerModel GeneratePlayerForGame(List<Guid> tickets, decimal ticketCost)
        {
            return new PlayerModel
            {
                Name = _userDataStore.PlayerName,
                Tickets = tickets,
                IsCpu = false,
                Spent = ticketCost
            };
        }
    }
}
