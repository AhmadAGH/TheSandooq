namespace TheSandooq.Models
{
    public interface ITransaction
    {
        public Category Category { get; set; }
        public double CalculatedAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}