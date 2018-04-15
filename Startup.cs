﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreToDo.Data;
using AspNetCoreToDo.Models;
using AspNetCoreToDo.Services;

namespace AspNetCoreToDo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                    .AddFacebook(options=>
                    {
                        options.AppId=Configuration["Facebook:AppId"];
                        options.AppSecret=Configuration["Facebook:AppSecret"];
                    });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<ITodoItemService,TodoItemService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                EnsureRoleAsync(roleManager).Wait();
                EnsureTestAdminAsync(userManager).Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists=await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if(alreadyExists) return;
            
            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var testAdmin=await userManager.Users.Where(x=>x.UserName=="ylmazbarscan@gmail.com").SingleOrDefaultAsync();

            if (testAdmin!=null) return;

            testAdmin=new ApplicationUser {UserName="ylmazbarscan@gmail.com",Email="ylmazbarscan@gmail.com"};
            await userManager.CreateAsync(testAdmin,"Y!m@z13ar$C@n");
            await userManager.AddToRoleAsync(testAdmin,Constants.AdministratorRole);

        }
    }
}