using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore
{
	public class Location
	{
		public int LocationId { get; set; }
		[Required]
		[StringLength(40)]
		public string LocationName { get; set; } = null!;
	}
}