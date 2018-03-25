using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using RazorViewRender.Domain.ViewRenderService;
using RazorViewRender.WebApp.EmailTemplates.Models;
using System.Reflection;

namespace RazorViewRender.Tests
{
	public class TestStartup
	{
		public TestStartup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			var assembly = typeof(EmailModel).GetTypeInfo().Assembly;

			//Create an EmbeddedFileProvider for that assembly
			var embeddedFileProvider = new EmbeddedFileProvider(assembly, "RazorViewRender.WebApp");

			//Add the file provider to the Razor view engine
			services.Configure<RazorViewEngineOptions>(options =>
			{
				options.FileProviders.Add(embeddedFileProvider);
			});

			services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IViewRenderingService, ViewRenderingService>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute("Deafult", "{controller=Home}/{action=Get}");
			});
		}
	}
}
