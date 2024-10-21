using LotteryResources.Models.Tickets;

namespace LotteryResources.Services.Interfaces
{
    public interface ITicketService
    {
        void DisplayTicketPrice();
        TicketRecieptModel ProcessNumberOfPlayerTickets(decimal playerBalance);
        List<Guid> GenerateTickets(int numberOfTickets);
        decimal CalculateTicketCost(int numberOfTickets);
        int MaxNumberOfTicketsForBalance(decimal playerBalance);
        decimal GetTicketRevenue();
    }
}