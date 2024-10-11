namespace TheSandooq.Models
{
    public class CategoryAmount
    {
        public string CategoryName { get; set; }
        public double Amount { get; set; }
        public bool IsIncome { get; set; }
        public bool IsMainCategory { get; set; }
        public int CategoryId { get; set; }

    }

}