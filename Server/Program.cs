using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Keepi;
using Keepi.Server;
using Keepi.Client.Communication;
using Keepi.Client.Repositories.Interfaces;
using Keepi.Client.Repositories.Implementation;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Db_Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "User Profiles")),
    RequestPath = "/User Profiles"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Notifications Files")),
    RequestPath = "/Notifications Files"
});

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();





//using Microsoft.AspNetCore.ResponseCompression;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Keepi;
//using Keepi.Server;
//using Keepi.Client.Communication;
//using Keepi.Client.Repositories.Interfaces;
//using Keepi.Client.Repositories.Implementation;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authentication;

//var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddHttpClient();
////builder.Services.AddScoped<IHttpService, HttpService>();


//// Add services to the container.

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();
//builder.Services.AddDbContext<Db_Context>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//        .AddEntityFrameworkStores<Db_Context>();

//var builder2 = builder.Services.AddIdentityServer()
//    .AddApiAuthorization<IdentityUser, Db_Context>();

//// הוספת מפתח זמני - למטרות פיתוח בלבד
//builder2.AddDeveloperSigningCredential();

//builder.Services.AddAuthentication()
//    .AddIdentityServerJwt();


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseIdentityServer();
//app.UseAuthorization();

//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

//app.Run();



//// Configure the HTTP request pipeline.
////if (!app.Environment.IsDevelopment())
////{
////    app.UseExceptionHandler("/Error");
////    app.UseHsts();
////}


////app.UseAuthorization();

////app.MapControllerRoute(
////    name: "default",
////    pattern: "{controller=Home}/{action=Index}/{id?}");
////app.MapRazorPages();

////app.Run();
