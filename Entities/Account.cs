using ATMSimulator.Entities.Enums;

namespace ATMSimulator.Entities
{
    public class Account
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public int Pin { get; set; }
    }
}
