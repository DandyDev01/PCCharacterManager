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

		public DnD5eCharacterClassFeature(string name, string desc, int level)
		{
			Level = level;
			Name = name;
			Desc = desc;
		}

		public DnD5eCharacterClassFeature()
		{
			Level = 0;
		}
	}
}
