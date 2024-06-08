namespace s24422_kolos.Dto;

public class ClientSubscriptionsDto
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public List<SubscriptionDTO> subscriptions { get; set; }
}