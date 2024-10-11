using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace TheSandooq.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "الاسم")]
        public string FullName { get; set; }
        public virtual ICollection<Sandooq> CreatedSandooqs { get; set; } = new List<Sandooq>();
        public virtual ICollection<Sandooq> Sandooqs { get; set; } = new List<Sandooq>();
        public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
       
    }

    
}