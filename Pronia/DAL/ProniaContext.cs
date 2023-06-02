using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pronia.DAL
{
	public class ProniaContext:IdentityDbContext
	{
		public ProniaContext(DbContextOptions<ProniaContext> options):base(options) 
		{ 

		}
	}
}
