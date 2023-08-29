using ATMSimulator.Entities.Enums;

namespace ATMSimulator.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }

        public string AccountId { get; set; }
    }
}
