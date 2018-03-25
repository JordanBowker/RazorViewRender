using Microsoft.AspNetCore.Mvc;
using RazorViewRender.WebApp.EmailTemplates.Models;

namespace RazorViewRender.WebApp.Controllers.Home
{
	public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Get()
		{
			return View("~/EmailTemplates/Templates/EmailView.cshtml", new EmailModel());
		}
	}
}
