using Pronia.DAL;

namespace Pronia.Services
{
	public class LayoutService
	{
		private readonly ProniaContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LayoutService(ProniaContext context,IHttpContextAccessor httpContextAccessor)
        {
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}
		public Dictionary<string,string> GetSettings() 
		{
			return _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);   
		}
    }
}
