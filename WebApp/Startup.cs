using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RazorViewRender.Domain.ViewRenderService;

namespace RazorViewRender.WebApp
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

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
