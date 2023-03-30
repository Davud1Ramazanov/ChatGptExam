namespace ChatGptExam.Models
{
    public class ShopSubscription
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public string? SubscriptionImage { get; set; }
        public float SubscriptionPrice { get; set; }

    }
}
