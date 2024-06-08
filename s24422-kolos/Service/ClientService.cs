using System.Collections;
using Microsoft.EntityFrameworkCore;
using s24422_kolos.Database;
using s24422_kolos.Dto;
using s24422_kolos.Model;

namespace s24422_kolos.Service;

public class ClientService : IClientService
{
    private readonly SubscriptionContext _context;

    public ClientService(SubscriptionContext context)
    {
        _context = context;
    }

    public async Task<ClientSubscriptionsDto?> GetClientWithSubscriptionsAsync(int id)
    {
        var client = await _context.Clients
            .Include(c => c.Sales)
            .ThenInclude(s => s.Subscription)
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.IdClient == id);
        return client == null ? null : MapResultToDto(client);
    }

    private static ClientSubscriptionsDto MapResultToDto(Client client)
    {
        var subscriptionDtos = client.Sales.Select(sale => new SubscriptionDTO()
        {
            IdSubscription = sale.IdSubscription,
            Name = sale.Subscription.Name,
            TotalPaidAmount = client.Payments
                .Where(p => p.IdSubscription == sale.Subscription.IdSubscription)
                .Sum(p => p.Subscription.Price)
        }).ToList();
        return new ClientSubscriptionsDto
        {
            firstName = client.FirstName,
            lastName = client.LastName,
            email = client.Email,
            phone = client.Phone,
            subscriptions = subscriptionDtos
        };
    }
}