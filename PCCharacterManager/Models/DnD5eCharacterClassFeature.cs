using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClassFeature : Property
	{
		public int Level { get; set; }

		public DnD5eCharacterClassFeature()
		{
			Level = 0;
		}
	}
}
