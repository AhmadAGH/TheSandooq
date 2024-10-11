using Sanooqna.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Sandooqna.Models
{
    public class AddMemberViewModel
    {
        [Required(ErrorMessage ="الاسم مطلوب")]
        [Display(Name = "الاسم الثلاثي")]
        public string MemberFullName { get; set; }

        [Required(ErrorMessage = "الايميل مطلوب")]
        [Display(Name = "الايميل")]
        public string MemberEmail { get; set; }

        [Required]
        public int SandooqId { get; set; }
        
    }
    public class IndexSandooqsViewModel
    {
        public List<Sandooq> sandooqs { get; set; }
    }
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "يجب ادخال اسم الفئة")]
        [Display(Name = "اسم الفئة")]
        public string name { get; set; }

        [Display(Name = "تتطلب عضو ؟")]
        public bool isRequireMember { get; set; }

        [Display(Name = "فئة دخل ؟")]
        public bool isIncome { get; set; }
        public virtual Sandooq sandooq { get; set; }
    }
    public class AddIncomeViewModel
    {


        [Display(Name = "المبلغ")]
        [Required(ErrorMessage = "يجب ادخال المبلغ")]
        [DataType(DataType.Currency, ErrorMessage = "يجب ادخال رقم")]
        [ValueValidation(val: 1, isMax: false, ErrorMessage = "يجب ان يكون المبلغ اكبر من او يساوي 1")]
        public double amount { get; set; }
        [Display(Name = "الفئة")]
        [Required(ErrorMessage = "يجب اختيار الفئة")]
        public int categoryID { get; set; }
        [Display(Name = "العضو")]
   
        public string memberID { get; set; }
        public int sandooqID { get; set; }
        
        public List<Category> sandooqCategories { get; set; }
        public List<ApplicationUser> sandooqMembers { get; set; }
    }
 
    public class AddExpenseViewModel
    {


        [Display(Name = "المبلغ")]
        [Required(ErrorMessage = "يجب ادخال المبلغ")]
        [DataType(DataType.Currency, ErrorMessage = "يجب ادخال رقم")]
        [ValueValidation(val: 1, isMax: false, ErrorMessage = "يجب ان يكون المبلغ اكبر من او يساوي 1")]
        public double amount { get; set; }
        [Display(Name = "الفئة")]
        [Required(ErrorMessage = "يجب اختيار الفئة")]
        public int categoryID { get; set; }
        [Display(Name = "العضو")]
        
        public string memberID { get; set; }
        public int sandooqID { get; set; }
        public Sandooq sandooq { get; set; }
        public List<Category> sandooqCategories { get; set; }
        public List<ApplicationUser> sandooqMembers { get; set; }
    }



}