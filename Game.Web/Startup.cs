
//using Fluent.Infrastructure.FluentModel;
using Game.Models;
using Game.Web.Data;
using Game.Web.Pages;
using Game.Web.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var cs = Configuration.GetConnectionString("Default");
			services.AddDbContext<DataContext>(options => options.UseSqlServer(cs));
			//	services.AddDbContext<DataContext>(options =>
			//options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddIdentity<IdentityUser, IdentityRole>(options =>
			{
				options.Password.RequiredLength = 5;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.SignIn.RequireConfirmedEmail = false;

			})
				.AddEntityFrameworkStores<DataContext>();

			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddSingleton<IDataAcces, DataAcces>();
			services.AddScoped<BrowserService>();
			services.AddTransient<MapBase>();
			services.AddScoped<DialogService>();
			services.AddScoped<NotificationService>();
			services.AddScoped<TooltipService>();
			services.AddScoped<ContextMenuService>();
			//services.AddHttpClient<IEmployeeService, EmployeeService>(client =>
			//{
			//	client.BaseAddress = new Uri("https://localhost:44358/");
			//});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			//app.UseDefaultFiles();
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
			//serviceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();
		}
	}
}
