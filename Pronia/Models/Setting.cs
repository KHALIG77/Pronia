using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
	public class Setting
	{
		[Required]
		[MaxLength(120)]
		public string Key {get; set;}
		public string Value { get; set;}	
	}
}
