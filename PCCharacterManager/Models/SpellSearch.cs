using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class SpellSearch : ISearch<SpellItemEditableViewModel>
	{
		public IEnumerable<SpellItemEditableViewModel> Search(string searchTerm, IEnumerable<SpellItemEditableViewModel> itemsToSearch)
		{
			List<SpellItemEditableViewModel> results = new List<SpellItemEditableViewModel>();

			foreach (SpellItemEditableViewModel spellVM in itemsToSearch)
			{
				if(ContainsSearchTerm(spellVM.Spell, searchTerm))
				{
					results.Add(spellVM);
				}
			}

			return results;
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
