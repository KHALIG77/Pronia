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
		public DbSet<Slider> Sliders {get; set; }
		public DbSet<Feature> Features { get; set; }	
		public DbSet<Plant> Plants { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Tag>Tags { get; set; }
		public DbSet<PlantTag>PlantTags { get; set; }
		public DbSet<PlantImage> PlantImages { get; set; }
		public DbSet<AppUser>AppUsers { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }
		public DbSet<Order> Orders {get; set; }
		public DbSet<OrderItem> OrderItems {get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Setting>().HasKey(x => x.Key);
			base.OnModelCreating(builder);
		}
	}
}
