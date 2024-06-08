namespace s24422_kolos.Model;

public class Sale
{
    public int IdSale { get; set; }
    public int IdClient { get; set; }
    public int IdSubscription { get; set; }
    public DateTime CreatedAt { get; set; }

    public Client Client { get; set; }
    public Subscription Subscription { get; set; }
}