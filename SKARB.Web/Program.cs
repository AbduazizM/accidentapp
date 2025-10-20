using LogApp.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Контекст данных
builder.Services.AddDbContext<SkarbDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreDbConnection")));

// Контекст аутентификации
builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDbConnection")));

// Страница ошибок ДЛЯ РАЗРАБОТКИ
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Параметры аутентификации
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.User.RequireUniqueEmail = false;

    //options.Password.RequireDigit = true;
    //options.Password.RequireNonAlphanumeric = true;
    //options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;


})
.AddUserStore<CustomUserStore>()
.AddRoleStore<CustomRoleStore>()
.AddErrorDescriber<RuIdentityErrorDescriber>()
.AddPasswordValidator<CustomPasswordValidator>()
.AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();

// Настройка путей для страниц аутентификации
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Поддержка Razor Pages и MVC
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditOrDelete", policy =>
        policy.RequireRole("admin", "operator"));
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    identityContext.Database.Migrate();
}



// Создание ролей и администратора при первом запуске
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "admin", "operator", "analyst", "auditor" };

    // Создание ролей, если не существуют
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    // Создание пользователей для каждой роли
    foreach (var roleName in roles)
    {
        var userName = roleName;
        var password = $"{roleName}123!";

        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = userName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
                logger.LogInformation("Создан пользователь: {UserName} с ролью {Role}", userName, roleName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError("Ошибка создания пользователя {UserName}: {Error}", userName, error.Description);
                }
            }
        }
    }


    //        // Создание администратора, если не существует
    //        var adminUserName = "admin";
    //var adminPassword = "Admin123!";

    //var admin = await userManager.FindByNameAsync(adminUserName);
    //if (admin == null)
    //{
    //    admin = new IdentityUser
    //    {
    //        UserName = adminUserName,
    //        //Email = adminEmail,
    //        EmailConfirmed = true
    //    };

    //    var result = await userManager.CreateAsync(admin, adminPassword);
    //    if (result.Succeeded)
    //    {
    //        await userManager.AddToRoleAsync(admin, "Admin");
    //    }
    //    else
    //    {
    //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    //        foreach (var error in result.Errors)
    //        {
    //            logger.LogError("Ошибка создания админа: {Error}", error.Description);
    //        }
    //    }
    //}

    //// Создание аккаунта OTK для тестирования
    //var otkUserName = "otk_test";
    //var otkPassword = "Otktest123!";

    //var otk = await userManager.FindByNameAsync(otkUserName);
    //if (otk == null)
    //{
    //    otk = new IdentityUser
    //    {
    //        UserName = otkUserName,
    //        EmailConfirmed = true
    //    };

    //    var result = await userManager.CreateAsync(otk, otkPassword);
    //    if (result.Succeeded)
    //    {
    //        await userManager.AddToRoleAsync(otk, "OTK");
    //    }
    //    else
    //    {
    //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    //        foreach (var error in result.Errors)
    //        {
    //            logger.LogError("Ошибка создания админа: {Error}", error.Description);
    //        }
    //    }
    //}

    //// Создание аккаунта OTK для тестирования
    //var testOnlyUserName = "test_only";
    ////var testOnlyEmail = "test_only@test_only.x";
    //var testOnlyPassword = "Testonly123!";

    //var testOnly = await userManager.FindByNameAsync(testOnlyUserName);
    //if (testOnly == null)
    //{
    //    testOnly = new IdentityUser
    //    {
    //        UserName = testOnlyUserName,
    //        //Email = testOnlyEmail,
    //        EmailConfirmed = true
    //    };

    //    var result = await userManager.CreateAsync(testOnly, testOnlyPassword);
    //    if (result.Succeeded)
    //    {
    //        await userManager.AddToRoleAsync(testOnly, "TestResultsViewer");
    //    }
    //    else
    //    {
    //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    //        foreach (var error in result.Errors)
    //        {
    //            logger.LogError("Ошибка создания админа: {Error}", error.Description);
    //        }
    //    }
    //}
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapFallback(async context =>
{
    var user = context.User;

    if (user.Identity?.IsAuthenticated == true)
    {
        context.Response.Redirect("/Incident/Index");
    }
    else
    {
        context.Response.Redirect("/Identity/Account/Login");
    }
}).WithMetadata(new Microsoft.AspNetCore.Mvc.RouteAttribute("/"));

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path.StartsWithSegments("/Identity/Account/Register"))
//    {
//        context.Response.Redirect("/Identity/Account/Login");
//        return;
//    }
//    await next();
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();