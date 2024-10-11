namespace TheSandooq.Models
{
    public class DetailsSandooqViewModel
    {
        public Sandooq Sandooq { get; set; }
        public double TotalIncomes { get; set; }
        public double TotalExpenses { get; set; }
        public double AvailableBalance { get; set; }
        public double RepaymentRate { get; set; }
        public List<SandooqMemberDetails> SandooqMembersDetails { get; set; }
        public List<CategoryAmount> CategoryAmounts { get; set; }
    }

}