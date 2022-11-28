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
		protected override void ORSearch(ref List<SpellItemEditableViewModel> results, IEnumerable<SpellItemEditableViewModel> itemsToSearch, string[] searchTerms)
		{
			foreach (SpellItemEditableViewModel spellItem in itemsToSearch)
			{
				bool schoolContainsSearchTerm = false;
				bool levelContainsSearchTerm = false;
				bool nameContainsSearchTerm = spellItem.Spell.Name.ToLower().Contains(searchTerms[0]) ||
					spellItem.Spell.Name.ToLower().Contains(searchTerms[1]);

				if (!nameContainsSearchTerm)
					schoolContainsSearchTerm = spellItem.Spell.School.ToString().ToLower().Contains(searchTerms[0]) ||
						spellItem.Spell.Name.ToLower().Contains(searchTerms[1]);

				if (!schoolContainsSearchTerm && !nameContainsSearchTerm)
					levelContainsSearchTerm = spellItem.Spell.Level.ToString().Contains(searchTerms[0]) ||
						spellItem.Spell.Name.ToLower().Contains(searchTerms[1]);

				if(nameContainsSearchTerm || schoolContainsSearchTerm || levelContainsSearchTerm)
					results.Add(spellItem);
			}
		}

		protected override void ANDSearch(ref List<SpellItemEditableViewModel> results, IEnumerable<SpellItemEditableViewModel> itemsToSearch, string[] searchTerms)
		{
			foreach (SpellItemEditableViewModel spellItem in itemsToSearch)
			{
				bool schoolContainsSearchTerm = false;
				bool levelContainsSearchTerm = false;
				bool nameContainsSearchTerm = spellItem.Spell.Name.ToLower().Contains(searchTerms[0]) &&
					spellItem.Spell.Name.ToLower().Contains(searchTerms[1]);

				if (!nameContainsSearchTerm)
					schoolContainsSearchTerm = spellItem.Spell.School.ToString().ToLower().Contains(searchTerms[0]) &&
						spellItem.Spell.Name.ToLower().Contains(searchTerms[1]);

				if (!schoolContainsSearchTerm && !nameContainsSearchTerm)
					levelContainsSearchTerm = spellItem.Spell.Level.ToString().Contains(searchTerms[0]) &&
						spellItem.Spell.Name.ToLower().Contains(searchTerms[1]);

				if (nameContainsSearchTerm || schoolContainsSearchTerm || levelContainsSearchTerm)
					results.Add(spellItem);
			}
		}

		protected override void DefaultSearch(ref List<SpellItemEditableViewModel> results, IEnumerable<SpellItemEditableViewModel> itemsToSearch, string searchTerm)
		{
			foreach (SpellItemEditableViewModel spellVM in itemsToSearch)
			{
				if (ContainsSearchTerm(spellVM.Spell, searchTerm))
				{
					results.Add(spellVM);
				}
			}
		}

		private bool ContainsSearchTerm(Spell spell, string searchTerm)
		{
			bool schoolContainsSearchTerm = false;
			bool levelContainsSearchTerm = false;
			bool nameContainsSearchTerm = spell.Name.ToLower().Contains(searchTerm);

			if (!nameContainsSearchTerm)
				schoolContainsSearchTerm = spell.School.ToString().ToLower().Contains(searchTerm);

			if (!schoolContainsSearchTerm && !nameContainsSearchTerm)
				levelContainsSearchTerm = spell.Level.ToString().Contains(searchTerm);

			return nameContainsSearchTerm || schoolContainsSearchTerm || levelContainsSearchTerm;

		}
	} // end class
} // end namespace
