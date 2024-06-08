using s24422_kolos.Dto;

namespace s24422_kolos.Service;

public interface IClientService
{
    Task<ClientSubscriptionsDto?> GetClientWithSubscriptionsAsync(int id);
}