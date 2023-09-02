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

		public IEnumerable<SpellItemEditableViewModel> Search(string searchTerm, 
			IEnumerable<SpellItemEditableViewModel> itemsToSearch)
		{
			if (searchTerm == string.Empty || string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Contains('*'))
				return itemsToSearch.ToList();

			List<SpellItemEditableViewModel> results = new();

			bool containsSpecialWord = ContainsSpecialWord(searchTerm);
			if (containsSpecialWord) SpecialWordSearch(ref results, itemsToSearch, searchTerm);
			else DefaultSearch(ref results, itemsToSearch, searchTerm);

			return results;
		}

		/// <summary>
		/// returns the 1st occurence of a specialWord contained in the searchTerm
		/// </summary>
		/// <param name="searchTerm">term to search for specialWord</param>
		/// <returns>1st occurence of a specialWord in the provided searchTerm or string.Empty if there is not one</returns>
		protected string ExtractSpecialWord(string searchTerm)
		{
			foreach (string s in specialWords)
			{
				if (searchTerm.Contains(s)) return s;
			}

			return string.Empty;
		}

		/// <summary>
		/// determines if the searchTerm contains any of the specialWords
		/// </summary>
		/// <param name="searchTerm">term to check</param>
		/// <returns>true if the searchTerm contains any of the specialWords</returns>
		protected bool ContainsSpecialWord(string searchTerm)
		{
			foreach (string s in specialWords)
			{
				if (searchTerm.Contains(s)) return true;
			}

			return false;
		}
		
		protected void SpecialWordSearch(ref List<SpellItemEditableViewModel> results, 
			IEnumerable<SpellItemEditableViewModel> itemsToSearch, string searchTerm)
		{
			string specialWord = ExtractSpecialWord(searchTerm);
			string[] searchTerms = searchTerm.Split(specialWord);
			searchTerms[0] = searchTerms[0].Trim();
			searchTerms[1] = searchTerms[1].Trim();

			switch (specialWord)
			{
				case "||":
					ORSearch(ref results, itemsToSearch, searchTerms);
					break;
				case "&&":
					ANDSearch(ref results, itemsToSearch, searchTerms);
					break;
			}
		}

		protected static void ORSearch(ref List<SpellItemEditableViewModel> results, IEnumerable<SpellItemEditableViewModel> itemsToSearch, string[] searchTerms)
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

		protected static void ANDSearch(ref List<SpellItemEditableViewModel> results, IEnumerable<SpellItemEditableViewModel> itemsToSearch, string[] searchTerms)
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

		protected static void DefaultSearch(ref List<SpellItemEditableViewModel> results, IEnumerable<SpellItemEditableViewModel> itemsToSearch, string searchTerm)
		{
			foreach (SpellItemEditableViewModel spellVM in itemsToSearch)
			{
				if (ContainsSearchTerm(spellVM.Spell, searchTerm))
				{
					results.Add(spellVM);
				}
			}
		}

		private static bool ContainsSearchTerm(Spell spell, string searchTerm)
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
