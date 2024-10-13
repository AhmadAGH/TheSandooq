using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheSandooq.Models
{
    public class Income : ITransaction
    {
        public int id { get; set; }

        [Display(Name = "المبلغ")]
        [Required(ErrorMessage = "يجب ادخال المبلغ")]
        [DataType(DataType.Currency, ErrorMessage = "يجب ادخال رقم")]
        public double amount { get; set; }
        [NotMapped]
        public double CalculatedAmount { get => amount; set => amount = value; }
        [Display(Name = "الفئة")]
        [Required(ErrorMessage = "يجب اختيار الفئة")]
        public virtual Category Category { get; set; }
        [Display(Name = "العضو")]
        [Required(ErrorMessage = "يجب اختيار العضو")]
        public virtual ApplicationUser member { get; set; }
        public DateTime TransactionDate { get; set; }
        public int SandooqId { get; set; }
        public int CategoryId { get; set; }

        public virtual Sandooq sandooq { get; set; }

    }

}