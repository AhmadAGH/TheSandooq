using Sanooqna.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sandooqna.Models
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



    }
    public class Income
    {
        public int id { get; set; }
        public string type { get; set; }

        [Display(Name = "المبلغ")]
        [Required(ErrorMessage = "يجب ادخال المبلغ")]
        [DataType(DataType.Currency, ErrorMessage = "يجب ادخال رقم")]
        public double amount { get; set; }
        [Display(Name = "الفئة")]
        [Required(ErrorMessage = "يجب اختيار الفئة")]
        public virtual Category category { get; set; }
        [Display(Name = "العضو")]
        [Required(ErrorMessage = "يجب اختيار العضو")]
        public virtual ApplicationUser member { get; set; }
        public int SandooqId { get; set; }
        public int CategoryId { get; set; }
        public virtual Sandooq sandooq { get; set; }

    }
    public class Category
    {
        public int id { get; set; }

        [Required(ErrorMessage = "يجب ادخال اسم الفئة")]
        [Display(Name = "اسم الفئة")]
        public string name { get; set; }

        [Display(Name = "تتطلب عضو ؟")]
        public bool isRequireMember { get; set; }

        [Display(Name = "فئة دخل ؟")]
        public bool isIncome { get; set; }
        public int SandooqId { get; set; }


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
        public MainCategories? mainCategoryType { get; set; }
        public virtual Sandooq sandooq { get; set; }

    }
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

    public class CategoryAmount
    {
        public string CategoryName { get; set; }
        public double Amount { get; set; }
        public bool IsIncome { get; set; }
        public bool IsMainCategory { get; set; }
        public int CategoryId { get; set; }

    }
    public class Expense
    {
        public int id { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        public int SandooqId { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
        public virtual ApplicationUser member { get; set; }

        public virtual Sandooq sandooq { get; set; }
    }

}