namespace TheSandooq.Models
{
    public class Expense
    {
        public int id { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        public int SandooqId { get; set; }
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public virtual ApplicationUser member { get; set; }

        public virtual Sandooq sandooq { get; set; }
    }

}