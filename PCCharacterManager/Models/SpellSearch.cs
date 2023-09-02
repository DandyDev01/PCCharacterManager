using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public enum SpellType { SPELL, CANTRIP, BOTH }
	public enum OrderByOption { ALPHABETICAL, SCHOOL, PREPARED, LEVEL, DURATION }

	public class SpellSearch : ISearch<SpellItemEditableViewModel>
	{
		protected string[] specialWords = { "&&", "||", "*" };
		
		public override bool Search(object obj)
		{
			if (obj is SpellItemEditableViewModel spellItem)
			{
				bool schoolContainsSearchTerm = false;
				bool levelContainsSearchTerm = false;

				if (spellItem.Spell is not Spell spell)
					return false;

				bool nameContainsSearchTerm = spell.Name.ToLower().Contains(searchTerm);
				
				if (!nameContainsSearchTerm)
					schoolContainsSearchTerm = spell.School.ToString().ToLower().Contains(searchTerm);

				if (!schoolContainsSearchTerm && !nameContainsSearchTerm)
					levelContainsSearchTerm = spell.Level.ToString().Contains(searchTerm);

				return nameContainsSearchTerm || schoolContainsSearchTerm || levelContainsSearchTerm;
			}

			return false;
		}
	} // end class
} // end namespace
