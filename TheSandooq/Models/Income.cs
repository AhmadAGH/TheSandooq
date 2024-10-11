using System.ComponentModel.DataAnnotations;

namespace TheSandooq.Models
{
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

}