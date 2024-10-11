namespace TheSandooq.Models
{
    public class SandooqMemberDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double TotalIncomes { get; set; }
        public double TotalExpenses { get; set; }
        public double AvailableBalance { get; set; }
        public double RepaymentRate { get; set; }
        public bool IsVerified { get; set; }

        public List<CategoryAmount> CategoryAmounts { get; set; }

    }

}