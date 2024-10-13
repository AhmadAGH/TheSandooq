namespace TheSandooq.Models
{
    public class MemberLoungeViewModel
    {
        public string SandooqName { get; set; }
        public string MemberName { get; set; }
        public double MemberTotalIncomes { get; set; }
        public double MemberTotalExpenses { get; set; }
        public double MemberRepaymentRate { get; set; }
        public double MemberAvailableBalance { get; set; }
        public List<ITransaction> MemberTransactions { get; set; } = new List<ITransaction>();
    }
}
