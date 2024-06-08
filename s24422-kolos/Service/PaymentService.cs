using Microsoft.EntityFrameworkCore;
using s24422_kolos.Database;
using s24422_kolos.Model;

namespace s24422_kolos.Service;

public class PaymentService : IPaymentService
{
    private readonly SubscriptionContext _context;

    public PaymentService(SubscriptionContext context)
    {
        _context = context;
    }

    public async Task<int?> AddPaymentAsync(int idClient, int idSubscription, decimal paymentAmount)
    {
        var client = await _context.Clients.FindAsync(idClient);
        if (client == null)
        {
            return null;
        }

        var subscription = await _context.Subscriptions.FindAsync(idSubscription);
        if (subscription == null)
        {
            return null;
        }

        var sales = await _context.Sales
            .Where(s => s.IdClient == idClient && s.IdSubscription == idSubscription)
            .ToListAsync();

        if (!sales.Any())
        {
            return null;
        }

        var latestSale = sales.OrderByDescending(s => s.CreatedAt).First();
        var renewalDate = latestSale.CreatedAt.AddMonths(subscription.RenewalPeriod);
        if (DateTime.Now > renewalDate)
        {
            return null;
        }

        var existingPayment = await _context.Payments
            .FirstOrDefaultAsync(p => p.IdClient == idClient && p.IdSubscription == idSubscription && p.Date >= latestSale.CreatedAt && p.Date < renewalDate);

        if (existingPayment != null)
        {
            return null;
        }

        var discounts = await _context.Discounts
            .Where(d => d.IdSubscription == idSubscription && d.DateFrom <= DateTime.Now && d.DateTo >= DateTime.Now)
            .ToListAsync();

        var maxDiscount = discounts.Any() ? discounts.Max(d => d.Value) : 0;
        var finalAmount = subscription.Price * (1 - (maxDiscount / 100.0m));

        if (paymentAmount != finalAmount)
        {
            return null;
        }

        var payment = new Payment
        {
            IdClient = idClient,
            IdSubscription = idSubscription,
            Date = DateTime.Now
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return payment.IdPayment;
    }
}
