using TheSandooq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSandooq.Models
{
    public class MemberSandooqDetailsViewModel
    {
        public string sandooqName { get; set; }
        public int sandooqID { get; set; }
        public string memberFullName { get; set; }
        public string memberID { get; set; }
        public ICollection<Expense> memberExpenses { get; set; }
        public ICollection<Income> memberIncomes { get; set; }
    }

    public class MemberLoungViewModel
    {
        public string sandooqName { get; set; }
        public int sandooqID { get; set; }
        public string memberFullName { get; set; }
        public string memberID { get; set; }
        public ICollection<Expense> memberExpenses { get; set; }
        public ICollection<Income> memberIncomes { get; set; }

        public Sandooq Sandooq { get; set; }
    }
}