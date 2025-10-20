using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<string> AllRoles { get; set; } = new();

        public class InputModel
        {
            public string Id { get; set; }

            //[Display(Name = "Эл. почта")]
            //[EmailAddress]
            //public string Email { get; set; }

            [Required(ErrorMessage = "Поле \"Логин\" обязательно для заполнения.")]
            [Display(Name = "Логин")]
            public string UserName { get; set; }

            public bool IsLocked { get; set; }


            [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль (оставьте пустым, чтобы не менять)")]
            public string? NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите новый пароль")]
            [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
            public string? ConfirmPassword { get; set; }

            public List<string> UserRoles { get; set; } = new();
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            Input = new InputModel
            {
                Id = user.Id,
                //Email = user.Email,
                UserName = user.UserName,
                IsLocked = user.LockoutEnd > DateTime.Now,
                UserRoles = userRoles.ToList()
            };

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

            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null) return NotFound();


            //user.Email = Input.Email;

            user.UserName = Input.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                AllRoles = (await _roleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
                return Page();
            }
            if (!ModelState.IsValid)
            {
                AllRoles = (await _roleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
                return Page();
            }

            // Сброс пароля, если указан
            if (!string.IsNullOrWhiteSpace(Input.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, Input.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    AllRoles = (await _roleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
                    return Page();
                }
            }

            // Обновление блокировки
            if (Input.IsLocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }

            // Обновляем роли
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Except(SelectedRoles ?? Enumerable.Empty<string>());
            var rolesToAdd = (SelectedRoles ?? Enumerable.Empty<string>()).Except(currentRoles);

            if (rolesToRemove.Any())
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (rolesToAdd.Any())
                await _userManager.AddToRolesAsync(user, rolesToAdd);

            TempData["SuccessMessage"] = "Пользователь успешно обновлён.";
            return RedirectToPage("./Index");
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