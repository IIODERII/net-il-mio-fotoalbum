using la_mia_pizzeria_static;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Identity;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //List<Category> categories = new List<Category>();
            //categories.Add(new Category() { Name = "Paesaggi"});
            //categories.Add(new Category() { Name = "Ritratti" });
            //categories.Add(new Category() { Name = "Natura" });
            //categories.Add(new Category() { Name = "Urban" });
            //categories.Add(new Category() { Name = "Macro" });
            //categories.Add(new Category() { Name = "Architettura" });
            //using(var db = new PictureContext())
            //{
            //    foreach (var category in categories)
            //    {
            //        db.Add(category);
            //    }
            //    db.SaveChanges();
            //}
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PictureContext>();
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PictureContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
