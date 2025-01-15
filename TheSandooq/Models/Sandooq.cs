using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sanooqna.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheSandooq.Data;

namespace TheSandooq.Models
{
    public class Sandooq
    {
        public int id { get; set; }
        public string creatorID { get; set; }

        [Display(Name = "المدير")]
        public virtual ApplicationUser creator { get; set; }
        public virtual ICollection<ApplicationUser> members { get; set; }

        [Display(Name = "اسم الصندوق")]
        [Required(ErrorMessage = "يجب ادخال اسم للصندوق")]
        public string name { get; set; }
        [Display(Name = "القسط الشهري")]
        [Required(ErrorMessage = "يجب ادخال قيمة القسط الشهري")]
        public double monthlyPayment { get; set; }
        public double maxInvestmentPercentage { get; set; }
        public virtual ICollection<Income> incomes { get; set; }
        public virtual ICollection<Expense> expenses { get; set; }
        public virtual ICollection<Category> categories { get; set; }

        public double GetTotalIncomes(string userID = "")
        {

            var sandooq = this;

            double totalIncomes = 0;
            if (sandooq.incomes == null || sandooq.incomes.Count == 0 || (!string.IsNullOrEmpty(userID) && sandooq.incomes.Where(i => i.member.Id.Equals(userID)).Count() == 0))
            {
                return totalIncomes;
            }
            if (!string.IsNullOrEmpty(userID))
            {
                foreach (Income i in sandooq.incomes.Where(i => i.member.Id.Equals(userID)))
                {
                    totalIncomes += i.amount;
                }
            }
            else
            {
                foreach (Income i in sandooq.incomes)
                {
                    totalIncomes += i.amount;
                }
            }

            return totalIncomes;


        }
        public double GetTotalExpenses(string userID = "")
        {

            var sandooq = this;
            double totalExpenses = 0;
            if (sandooq.expenses == null || sandooq.expenses.Count == 0 || (!string.IsNullOrEmpty(userID) && sandooq.expenses.Where(i => i.member.Id.Equals(userID)).Count() == 0))
            {
                return totalExpenses;
            }
            if (!string.IsNullOrEmpty(userID))
            {
                foreach (Expense e in sandooq.expenses.Where(i => i.member.Id.Equals(userID)))
                {
                    totalExpenses += e.amount;
                }
            }
            else
            {
                foreach (Expense e in sandooq.expenses)
                {
                    totalExpenses += e.amount;
                }
            }

            return totalExpenses;


        }
        public double GetBalance(string userID = "")
        {
            return this.GetTotalIncomes(userID) - this.GetTotalExpenses(userID);
        }
        public double GetAvailableBalance(string userID = "")
        {


            var sandooq = this;
            double availableBalance = 
                sandooq.incomes.Sum(i => i.amount) 
                - 
                sandooq.expenses.Sum(e => e.amount);
            if (string.IsNullOrEmpty(userID))
            {
                foreach (Income i in sandooq.incomes)
                {
                    if (i.Category.mainCategoryType.HasValue && i.Category.mainCategoryType == MainCategories.DPI)
                    {
                        availableBalance -= i.amount;
                    }
                }
                return availableBalance;
            }

            foreach (Income i in sandooq.incomes)
            {
                if (i.Category.mainCategoryType.HasValue && i.Category.mainCategoryType == MainCategories.DPI && !i.member.Id.Equals(userID))
                {
                    availableBalance -= i.amount;
                }
            }
            return availableBalance;


        }

        public double GetMemberBalance(string memberId)
        {
            var sandooq = this;
            double availableBalance =
                sandooq.incomes.Where(i => i.member.Id == memberId).Sum(i => i.amount)
                -
                sandooq.expenses.Where(i => i.member.Id == memberId).Sum(e => e.amount);

            return availableBalance;
        }
        public double GetTotalAmountByCategory(int categoryID, string userID = "")
        {

            double totalAmount = 0;
            var sandooq = this;
            Category? category = sandooq.categories.FirstOrDefault(c => c.id == categoryID);
            if (category == null)
            {
                return totalAmount;
            }
            if (category.isIncome)
            {
                if (string.IsNullOrEmpty(userID))
                {
                    foreach (Income i in sandooq.incomes)
                    {
                        if (i.Category.mainCategoryType.HasValue && i.Category.id == categoryID)
                        {
                            totalAmount += i.amount;
                        }
                    }
                }
                else
                {
                    foreach (Income i in sandooq.incomes)
                    {
                        if (i.Category.mainCategoryType.HasValue && i.Category.id == categoryID && i.member.Id.Equals(userID))
                        {
                            totalAmount += i.amount;
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(userID))
                {
                    foreach (Expense e in sandooq.expenses)
                    {
                        if (e.Category.mainCategoryType.HasValue && e.Category.id == categoryID)
                        {
                            totalAmount += e.amount;
                        }
                    }
                }
                else
                {
                    foreach (Expense e in sandooq.expenses)
                    {
                        if (e.Category.mainCategoryType.HasValue && e.Category.id == categoryID && e.member.Id.Equals(userID))
                        {
                            totalAmount += e.amount;
                        }
                    }
                }
            }

            return totalAmount;


        }

        public double GetTotalAmountByMainCategory(MainCategories mainCategory, string userID = "")
        {

            double totalAmount = 0;
            var sandooq = this;
            Category? category = sandooq.categories.FirstOrDefault(c => c.mainCategoryType == mainCategory);
            if (category == null)
            {
                return totalAmount;
            }
            if (category.isIncome)
            {
                if (string.IsNullOrEmpty(userID))
                {
                    foreach (Income i in sandooq.incomes)
                    {
                        if (i.Category.mainCategoryType.HasValue && i.Category.mainCategoryType == mainCategory)
                        {
                            totalAmount += i.amount;
                        }
                    }
                }
                else
                {
                    foreach (Income i in sandooq.incomes)
                    {
                        if (i.Category.mainCategoryType.HasValue && i.Category.mainCategoryType == mainCategory && i.member.Id.Equals(userID))
                        {
                            totalAmount += i.amount;
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(userID))
                {
                    foreach (Expense e in sandooq.expenses)
                    {
                        if (e.Category.mainCategoryType.HasValue && e.Category.mainCategoryType == mainCategory)
                        {
                            totalAmount += e.amount;
                        }
                    }
                }
                else
                {
                    foreach (Expense e in sandooq.expenses)
                    {
                        if (e.Category.mainCategoryType.HasValue && e.Category.mainCategoryType == mainCategory && e.member.Id.Equals(userID))
                        {
                            totalAmount += e.amount;
                        }
                    }
                }
            }

            return totalAmount;


        }
        public double GetRepaymentRate(string userID = "")
        {

            var sandooq = this;
            Category? paymentCat = sandooq.categories.FirstOrDefault(c => c.mainCategoryType == MainCategories.PYM);
            Category? loanCat = sandooq.categories.FirstOrDefault(c => c.mainCategoryType == MainCategories.LON);
            if(paymentCat == null || loanCat == null)
            {
                return 100;
            }
            double totalPayments = this.GetTotalAmountByCategory( paymentCat.id, userID);
            double totalLoans = this.GetTotalAmountByCategory(loanCat.id, userID);
            if (totalLoans == 0)
            {
                return 100;
            }
            else if (totalPayments == 0)
            {
                return 0;
            }
            else
            {
                return totalPayments / totalLoans * 100;
            }



        }

    }

    /// <summary>
    /// Typs Of Main categorys in all sandooqs:
    /// قسط - PYM
    /// وديعة - DPI
    /// سداد - PAY
    /// عائد استثماري - IVI
    /// سلفة - LON
    /// سحب وديعة - DPE
    /// سحب اصول - ALL
    /// استثمار - IVE
    /// زكاة - ZKH
    /// </summary>
    public enum MainCategories
    {

        PYM,
        DPI,
        PAY,
        IVI,
        LON,
        DPE,
        ALL,
        IVE,
        ZKH
    }

}