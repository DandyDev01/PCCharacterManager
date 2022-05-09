using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class CharacterClassFeature : Property
	{
		public int Level { get; set; }

		public CharacterClassFeature()
		{
			Level = 0;
		}
	}
}
