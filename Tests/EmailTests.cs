using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.PlatformAbstractions;
using NUnit.Framework;
using RazorViewRender.Domain.ViewRenderService;
using RazorViewRender.WebApp.EmailTemplates.Models;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RazorViewRender.Tests
{
	[TestFixture]
	public class EmailTests
	{
		private IViewRenderingService _viewRenderingService;

		[SetUp]
		public void Setup()
		{
			var services = new ServiceCollection();

			ConfigureDefaultServices(services);

			var serviceProvider = services.BuildServiceProvider();
			_viewRenderingService = serviceProvider.GetService<IViewRenderingService>();
		}


		private void ConfigureDefaultServices(IServiceCollection services)
		{
			services.AddSingleton(PlatformServices.Default.Application);

			var appDirectory = Directory.GetCurrentDirectory();
			services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
			{
				WebRootFileProvider = new PhysicalFileProvider(appDirectory),
				ApplicationName = "RazorViewRender.Tests"
			});

			var assembly = typeof(EmailModel).GetTypeInfo().Assembly;

			//Create an EmbeddedFileProvider for that assembly
			var embeddedFileProvider = new EmbeddedFileProvider(assembly, "RazorViewRender.WebApp");

			//Add the file provider to the Razor view engine
			services.Configure<RazorViewEngineOptions>(options =>
			{
				options.FileProviders.Clear();
				options.FileProviders.Add(embeddedFileProvider);
			});

			services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
			services.AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"));

			services.AddLogging();
			services.AddMvc();
			services.AddTransient<IViewRenderingService, ViewRenderingService>();
		}

		[Test]
		public async Task EmailTempalte()
		{
			//arrange
			var emailModel = new EmailModel { Name = "Test", ImageSource = "ImSrc" };

			//act
			var actual = await _viewRenderingService.RenderWithModel("~/EmailTemplates/Templates/EmailView.cshtml", emailModel);

			//assert
			actual = RemoveWhitespace(actual);
			var expectedString = RemoveWhitespace(GetTestEmailHtml());

			Assert.That(actual, Is.EqualTo(expectedString));
		}


		private string GetTestEmailHtml()
		{
			return File.ReadAllText("../../../TestEmail.html");
		}

		private string RemoveWhitespace(string input)
		{
			return Regex.Replace(input, @"\s+", "");
		}
	}
}
