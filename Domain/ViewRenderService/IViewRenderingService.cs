using System.Threading.Tasks;

namespace RazorViewRender.Domain.ViewRenderService
{
	public interface IViewRenderingService
	{
		Task<string> Render(string viewPath);
		Task<string> RenderWithModel<TModel>(string viewPath, TModel model);
	}
}
