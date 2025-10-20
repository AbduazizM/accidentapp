using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<string> AllRoles { get; set; } = new();

        public class InputModel
        {
            //[Display(Name = "Эл. почта")]
            //public string Email { get; set; }

            [Required(ErrorMessage = "Поле \"Логин\" обязательно для заполнения.")]
            [Display(Name = "Логин")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Поле \"Пароль\" обязательно для заполнения.")]
            [Display(Name = "Пароль")]
            [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Поле \"Потвердить пароль\" обязательно для заполнения.")]
            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите пароль")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            AllRoles = (await _roleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] SelectedRoles)
        {
            //if (string.IsNullOrWhiteSpace(Input.Email))
            //{
            //    ModelState.Remove("Input.Email");
            //}
            //else
            //{
            //    if (!IsValidEmail(Input.Email))
            //    {
            //        ModelState.AddModelError("Input.Email", "Некорректный формат электронной почты.");
            //    }
            //}

            if (!ModelState.IsValid)
            {
                AllRoles = (await _roleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = Input.UserName,
                //Email = Input.Email
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                if (SelectedRoles?.Any() == true)
                {
                    await _userManager.AddToRolesAsync(user, SelectedRoles);
                }

                TempData["SuccessMessage"] = "Пользователь успешно создан.";
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            AllRoles = (await _roleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
            return Page();
        }

        //private bool IsValidEmail(string email)
        //{
        //    try
        //    {
        //        var addr = new System.Net.Mail.MailAddress(email);
        //        return addr.Address == email;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}