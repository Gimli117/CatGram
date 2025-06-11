using CatGram.Data;
using CatGram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CatGram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AllowAnonymousToFolder("/Identity");
            });

            builder.Services.AddSession();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.MapRazorPages();
            app.Run();
        }
    }
}