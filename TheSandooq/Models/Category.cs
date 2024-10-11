using System.ComponentModel.DataAnnotations;

namespace TheSandooq.Models
{
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

}