namespace LotteryResources.Storage.Interfaces
{
    public interface ITicketDataStore
    {
        decimal TicketPrice { get; }
        int MaxTickets { get; }
        decimal TotalTicketRevenue { get; }
        void TicketPurchased();
    }
}