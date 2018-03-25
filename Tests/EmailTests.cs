using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RazorViewRender.Domain.ViewRenderService;
using RazorViewRender.WebApp.EmailTemplates.Models;
using System.IO;
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
			var webHost = WebHost.CreateDefaultBuilder()
								 .UseStartup<TestStartup>()
								 .Build();

			_viewRenderingService = webHost.Services.GetService<IViewRenderingService>();
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
