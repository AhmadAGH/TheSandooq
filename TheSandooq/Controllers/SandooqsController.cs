using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using TheSandooq.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSandooq.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using TheSandooq.Models;

namespace TheSandooq.Controllers
{
    [Authorize]
    public class SandooqsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private string _currentUserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        public SandooqsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        // GET: Sandooqs
        public ActionResult Index(string message, bool? isSuccess)
        {
            if (message != null)
            {
                ViewBag.message = message;
                ViewBag.isSuccess = isSuccess;
            }
            var sandooqs = _dbContext.dbSandooqs.Where(s => s.creator.Email.Equals(User.Identity.Name) || s.members.Any(m => m.Id == _currentUserId)).ToList();
            return View(sandooqs);
        }

        // GET: Sandooqs/Details/5
        public async Task<ActionResult> Details(int? id, string message, bool? isSuccess)
        {
            if (message != null)
            {
                ViewBag.message = message;
                ViewBag.isSuccess = isSuccess;
            }
            if (id == null)
            {
                return new BadRequestResult();
            }
            Sandooq sandooq = await _dbContext.dbSandooqs.FindAsync(id);
            var isCurrentUserCreator = _currentUserId == sandooq.creator.Id;

            if (!isCurrentUserCreator)
            {
                return RedirectToAction("Index", "Sandooqs", new { message = "غير مصرح لك الدخول الى هذا الصندوق", isSuccess = false });
            }
            if (sandooq == null)
            {
                return NotFound();
            }

            var membersDetails = sandooq.members.Select(m => new SandooqMemberDetails
            {
                Id = m.Id,
                AvailableBalance = sandooq.GetAvailableBalance(m.Id),
                RepaymentRate = sandooq.GetRepaymentRate(m.Id),
                TotalExpenses = sandooq.GetTotalExpenses(m.Id),
                TotalIncomes = sandooq.GetTotalIncomes(m.Id),
                IsVerified = !string.IsNullOrEmpty(m.PasswordHash),
                Name = m.FullName,
                CategoryAmounts = sandooq.categories.Select(c => new CategoryAmount
                {
                    CategoryId = c.id,
                    CategoryName = c.name,
                    IsMainCategory = c.mainCategoryType.HasValue,
                    IsIncome = c.isIncome,
                    Amount = sandooq.GetTotalAmountByCategory(c.id, m.Id)

                }).ToList()
            }).ToList();

            var model = new DetailsSandooqViewModel
            {
                Sandooq = sandooq,
                AvailableBalance = sandooq.GetAvailableBalance(),
                RepaymentRate = sandooq.GetRepaymentRate(),
                TotalExpenses = sandooq.GetTotalExpenses(),
                TotalIncomes = sandooq.GetTotalIncomes(),
                SandooqMembersDetails = membersDetails,
                CategoryAmounts = sandooq.categories.Select(c => new CategoryAmount
                {
                    CategoryId = c.id,
                    CategoryName = c.name,
                    IsMainCategory = c.mainCategoryType.HasValue,
                    IsIncome = c.isIncome,
                    Amount = sandooq.GetTotalAmountByCategory(c.id)
                }).ToList()
            };


            return View(model);
        }

        // GET: Sandooqs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sandooqs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateSandooqViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sandooq = new Sandooq();
                sandooq.creatorID = _currentUserId;
                sandooq.monthlyPayment = model.MonthlyPayment;
                sandooq.name = model.Name;
                List<Category> mainCategories = new List<Category>
                {
                new Category{name = "قسط",          isIncome = true,isRequireMember = true,mainCategoryType=MainCategories.PYM,sandooq = sandooq},
                new Category{name = "وديعة",        isIncome = true,isRequireMember = true,mainCategoryType=MainCategories.DPI,sandooq = sandooq},
                new Category{name = "سداد",         isIncome = true,isRequireMember = true,mainCategoryType=MainCategories.PAY,sandooq = sandooq},
                new Category{name = "عائد استثماري",isIncome = true,isRequireMember = false,mainCategoryType=MainCategories.IVI,sandooq = sandooq},
                new Category{name = "سلفة",         isIncome = false,isRequireMember = true,mainCategoryType=MainCategories.LON,sandooq = sandooq},
                new Category{name = "سحب وديعة",    isIncome = false,isRequireMember = true,mainCategoryType=MainCategories.DPE,sandooq = sandooq},
                new Category{name = "سحب اصول",     isIncome = false,isRequireMember = true,mainCategoryType=MainCategories.ALL,sandooq = sandooq},
                new Category{name = "استثمار",      isIncome = false,isRequireMember = false,mainCategoryType=MainCategories.IVE,sandooq = sandooq},
                new Category{name = "زكاة",         isIncome = false,isRequireMember = false,mainCategoryType=MainCategories.ZKH,sandooq = sandooq},
                };
                sandooq.categories = mainCategories;

                _dbContext.dbSandooqs.Add(sandooq);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Sandooqs", new { message = "تمت اضافة الصندوق بنجاح", isSuccess = true });
            }

            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            string errorsString = string.Join(",", errors);
            return RedirectToAction("Index", "Sandooqs", new { message = errorsString, isSuccess = false });
        }

        // GET: Sandooqs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Sandooq sandooq = _dbContext.dbSandooqs.Find(id);
            if (sandooq == null)
            {
                return NotFound();
            }
            return View(sandooq);
        }

        // POST: Sandooqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sandooq sandooq)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(sandooq).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sandooq);
        }

        // GET: Sandooqs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();

            }
            Sandooq sandooq = _dbContext.dbSandooqs.Find(id);
            if (sandooq == null)
            {
                return NotFound();
            }
            return View(sandooq);
        }

        // POST: Sandooqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sandooq sandooq = _dbContext.dbSandooqs.Find(id);
            _dbContext.dbSandooqs.Remove(sandooq);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddMember(int sandooqId)
        {
            return PartialView("AddMemberPartial");
        }
        [HttpPost]
        public async Task<ActionResult> AddMember(AddMemberViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Details", "Sandooqs", new { id = model.SandooqId, message = "فضلا قم بتعبئة جميع الحقول !", isSuccess = false });
                }
                Sandooq sandooq = _dbContext.dbSandooqs.First(s => s.id == model.SandooqId);
                ApplicationUser member = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.MemberEmail);
                var isNewUser = member == null;
                // Check if the member already exists
                if (isNewUser)
                {
                    // Create new user if not exists
                    member = new ApplicationUser
                    {
                        UserName = model.MemberEmail,
                        Email = model.MemberEmail,
                        FullName = model.MemberFullName
                    };

                    var result = await _userManager.CreateAsync(member);
                    if (!result.Succeeded)
                    {
                        // If user creation fails, return an error message
                        return RedirectToAction("Details", "Sandooqs", new { model.SandooqId, message = "تعذر انشاء المستخدم !", isSuccess = false });
                    }
                    // Get the newly created user from the database
                    member = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == member.Email);
                    // Send password reset link to new user
                    SendPasswordLinkToMemberAsync(member.Id);
                }
                else
                {
                    if (sandooq.members.Any(m => m.Id == member.Id))
                    {
                        return RedirectToAction("Details", "Sandooqs", new { model.SandooqId, message = "العضو مضاف لللصندوق بالفعل ! !", isSuccess = false });
                    }
                }

                // Attach the user to the DbContext if it’s not already being tracked**
                if (_dbContext.Entry(member).State == EntityState.Detached)
                {

                    _dbContext.Users.Attach(member);
                }

                // Add member to Sandooq
                sandooq.members.Add(member);
                _dbContext.SaveChanges();

                // Redirect with a success message
                return RedirectToAction("Details", "Sandooqs", new
                {
                    sandooq.id,
                    message = isNewUser ? "تم اضافة العضو وارسال الدعوة  بنجاح" : "تم اضافة العضو بنجاح",
                    isSuccess = true
                });

            }
            catch (Exception e)
            {
                // Return an error view or redirect with an error message (customize as per your need)
                return RedirectToAction("Error", "Home", new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<bool> ResendPasswordLinkToMemberAsync([FromBody] string memberId)
        {
            if (string.IsNullOrEmpty(memberId))
            {
                return false;
            }

            return await SendPasswordLinkToMemberAsync(memberId);
        }

        private async Task<bool> SendPasswordLinkToMemberAsync(string memberId)
        {
            try
            {
                var member = await _userManager.FindByIdAsync(memberId);
                if (member == null)
                {
                    return false;
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(member);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    member.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public ActionResult AddCategory(Category model)
        {
            Sandooq sandooq = _dbContext.dbSandooqs.First(doo8 => doo8.id == model.sandooq.id);
            if (ModelState.IsValid)
            {
                model.sandooq = sandooq;
                sandooq.categories.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Details", "Sandooqs", new { model.sandooq.id, message = "تم اضافة الفئة بنجاح", isSuccess = true });
            }
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            string errorsString = string.Join(",", errors);
            return RedirectToAction("Details", "Sandooqs", new { model.sandooq.id, message = errorsString, isSuccess = false });
        }
        public ActionResult AddIncome(AddIncomeViewModel model)
        {
            ModelState.Remove("sandooqMembers");
            ModelState.Remove("sandooqCategories");
            if (ModelState.IsValid)
            {
                Income income = new Income();
                income.category = _dbContext.dbCategories.Find(model.categoryID);
                income.sandooq = _dbContext.dbSandooqs.Find(model.sandooqID);
                if (income.category.isRequireMember && string.IsNullOrEmpty(model.memberID))
                {
                    return RedirectToAction("Details", "Sandooqs", new { id = model.sandooqID, message = "الفئة (" + income.category.name + ") تتطلب اختيار عضو", isSuccess = false });
                }
                else if (!income.category.isRequireMember)
                {

                    int membersCount = income.sandooq.members.Count();
                    List<Income> incomeDevidedToAllMember = new List<Income>();
                    income.amount = model.amount / membersCount;
                    foreach (ApplicationUser member in income.sandooq.members)
                    {
                        Income i = new Income
                        {
                            category = income.category,
                            sandooq = income.sandooq,
                            amount = income.amount,
                            member = member,
                        };
                        incomeDevidedToAllMember.Add(i);
                    }
                    _dbContext.dbIncomes.AddRange(incomeDevidedToAllMember);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Details", "Sandooqs", new { income.sandooq.id, message = "تمت اضافة دخل بنجاح", isSuccess = true });
                }
                else
                {
                    income.member = _dbContext.Users.Find(model.memberID);
                    income.amount = model.amount;
                }

                _dbContext.dbIncomes.Add(income);
                _dbContext.SaveChanges();
                return RedirectToAction("Details", "Sandooqs", new { income.sandooq.id, message = "تمت اضافة دخل بنجاح", isSuccess = true });
            }
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            string errorsString = string.Join(",", errors);
            return RedirectToAction("Details", "Sandooqs", new { id = model.sandooqID, message = errorsString, isSuccess = false });
        }
        public async Task<ActionResult> AddExpense(AddExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {

                Sandooq sandooq = _dbContext.dbSandooqs.Find(model.sandooqID);
                Expense expense = new Expense();
                expense.sandooq = sandooq;
                expense.category = _dbContext.dbCategories.Find(model.categoryID);
                if (sandooq.GetBalance() < model.amount)
                {
                    return RedirectToAction("Details", "Sandooqs", new { id = model.sandooqID, message = "رصيد الصندوق لا يسمح بالسحب", isSuccess = false });
                }
                else if (sandooq.GetAvailableBalance(model.memberID) < model.amount)
                {
                    return RedirectToAction("Details", "Sandooqs", new { id = model.sandooqID, message = "رصيد الصندوق يحتوي على ودائع لا يمكن سحبها الا من صاحبها", isSuccess = false });
                }
                else if (expense.category.isRequireMember && string.IsNullOrEmpty(model.memberID))
                {
                    return RedirectToAction("Details", "Sandooqs", new { id = model.sandooqID, message = "الفئة (" + expense.category.name + ") تتطلب اختيار عضو", isSuccess = false });
                }
                else if (!expense.category.isRequireMember)
                {

                    int membersCount = expense.sandooq.members.Count();
                    List<Expense> expenseDevidedToAllMember = new List<Expense>();
                    expense.amount = model.amount / membersCount;
                    foreach (ApplicationUser member in expense.sandooq.members)
                    {
                        Expense e = new Expense
                        {
                            category = expense.category,
                            sandooq = expense.sandooq,
                            amount = expense.amount,
                            member = member,
                        };
                        expenseDevidedToAllMember.Add(e);
                    }
                    _dbContext.dbExpenses.AddRange(expenseDevidedToAllMember);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Details", "Sandooqs", new { expense.sandooq.id, message = "تمت اضافة مصروف بنجاح", isSuccess = true });
                }
                else
                {
                    expense.member = _dbContext.Users.Find(model.memberID);
                    expense.amount = model.amount;
                }


                _dbContext.dbExpenses.Add(expense);
                _dbContext.SaveChanges();
                return RedirectToAction("Details", "Sandooqs", new { expense.sandooq.id, message = "تمت اضافة مصروف بنجاح", isSuccess = true });
            }
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            string errorsString = string.Join(",", errors);
            return RedirectToAction("Details", "Sandooqs", new { id = model.sandooqID, message = errorsString, isSuccess = false });
        }
        public async Task<ActionResult> MemberDetails(string memberID, int sandooqID)
        {
            ApplicationUser member = await _dbContext.Users.AsNoTracking().FirstAsync(m => m.Id.Equals(memberID));
            var sandooq = await _dbContext.dbSandooqs.AsNoTracking().FirstOrDefaultAsync(s => s.id == sandooqID);
            List<Expense> expenses = _dbContext.dbExpenses.AsNoTracking().Where(e => e.sandooq.id == sandooqID && e.member.Id.Equals(memberID)).ToList();
            List<Income> incomes = _dbContext.dbIncomes.AsNoTracking().Where(i => i.sandooq.id == sandooqID && i.member.Id.Equals(memberID)).ToList();
            return View(new MemberSandooqDetailsViewModel
            {
                memberID = member.Id,
                sandooqID = sandooq.id,
                memberFullName = member.FullName
                ,
                memberExpenses = expenses,
                memberIncomes = incomes,
                sandooqName = sandooq.name
            });
        }

        public ActionResult MemberLounge(int id)
        {


            ViewBag.CurrentUserId = _currentUserId;
            var sandooq = _dbContext.dbSandooqs.FirstOrDefault(s => s.id == id && s.members.Any(m => m.Id == _currentUserId));
            if (sandooq == null)
            {
                return RedirectToAction("Index", "Sandooqs", new { message = "غير مصرح لك الدخول الى هذا الصندوق", isSuccess = false });
            }
            return View(sandooq);

        }
    }
}
