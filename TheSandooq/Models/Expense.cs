using System.ComponentModel.DataAnnotations.Schema;

namespace TheSandooq.Models
{
    public class Expense :ITransaction
    {
        public int id { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        [NotMapped]
        public double CalculatedAmount { get => amount * -1; set => amount = value; }
        public int SandooqId { get; set; }
        public int CategoryId { get; set; }
        public DateTime TransactionDate { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser member { get; set; }

        public virtual Sandooq sandooq { get; set; }

    }

}