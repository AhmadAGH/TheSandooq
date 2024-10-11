using System.ComponentModel.DataAnnotations;

namespace TheSandooq.Models
{
    public class CreateSandooqViewModel
    {

        [Display(Name = "اسم الصندوق")]
        [Required(ErrorMessage = "يجب ادخال اسم للصندوق")]
        public string Name { get; set; }
        [Display(Name = "القسط الشهري")]
        [Required(ErrorMessage = "يجب ادخال قيمة القسط الشهري")]
        public double MonthlyPayment { get; set; }

    }
}
