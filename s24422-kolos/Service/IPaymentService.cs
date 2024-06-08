namespace s24422_kolos.Service;

public interface IPaymentService
{
    Task<int?> AddPaymentAsync(int idClient, int idSubscription, decimal paymentAmount);
}