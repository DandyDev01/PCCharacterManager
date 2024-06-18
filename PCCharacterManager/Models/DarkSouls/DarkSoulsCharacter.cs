using PCCharacterManager.Models.DarkSouls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DarkSoulsCharacter : DnD5eCharacter
	{

		private DarkSoulsOragin _oragin;
		public DarkSoulsOragin Oragin => _oragin;

		public DarkSoulsCharacter(DnD5eCharacterClassData classData, DarkSoulsOragin oragin)
		{
			CharacterClass = new DnD5eCharacterClass(classData);
			
			_oragin = oragin;
			_background = oragin.Name;
		}
	}
}
