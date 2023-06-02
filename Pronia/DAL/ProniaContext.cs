using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DAL
{
	public class ProniaContext:IdentityDbContext
	{
		public ProniaContext(DbContextOptions<ProniaContext> options):base(options) 
		{ 
		
		}
		public DbSet<Setting> Settings { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Setting>().HasKey(x => x.Key);
			base.OnModelCreating(builder);
		}
	}
}
