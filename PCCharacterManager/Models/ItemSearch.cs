using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class ItemSearch : ISearch<ItemViewModel>
	{
		private string[] specialWords = { "&&", "||", "*" };

		public IEnumerable<ItemViewModel> Search(string searchTerm, IEnumerable<ItemViewModel> itemsToSearch)
		{
			if (searchTerm == string.Empty || string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Contains("*"))
				return itemsToSearch.ToList();

			List<ItemViewModel> results = new List<ItemViewModel>();

			bool containsSpecialWord = ContainsSpecialWord(searchTerm);
			if (containsSpecialWord) SpecialWordSearch(ref results, itemsToSearch, searchTerm);
			else DefaultSearch(ref results, itemsToSearch, searchTerm);

			return results;
		}

		private void SpecialWordSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string searchTerm)
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

		private void ANDSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string[] searchTerms)
		{
			foreach (var item in itemsToSearch)
			{
				if (item.BoundItem.Name.ToLower().Contains(searchTerms[0]) &&
					item.BoundItem.Name.ToLower().Contains(searchTerms[1]))
				{
					results.Add(item);
					continue;
				}

				// the item name did not contain the search term
				// check the properties for the search term
				foreach (var property in item.BoundItem.Properties)
				{
					if (property.Name.ToLower().Contains(searchTerms[0]) &&
						item.BoundItem.Name.ToLower().Contains(searchTerms[1]))
					{
						results.Add(item);
						break;
					}
					else if (property.Desc.ToLower().Contains(searchTerms[0]) &&
						item.BoundItem.Name.ToLower().Contains(searchTerms[1]))
					{
						results.Add(item);
						break;
					}
				} // end property check
			} // end item name check
		}

		private void ORSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string[] searchTerms)
		{
			foreach (var item in itemsToSearch)
			{
				if (item.BoundItem.Name.ToLower().Contains(searchTerms[0]) || 
					item.BoundItem.Name.ToLower().Contains(searchTerms[1]))
				{
					results.Add(item);
					continue;
				}

				// the item name did not contain the search term
				// check the properties for the search term
				foreach (var property in item.BoundItem.Properties)
				{
					if (property.Name.ToLower().Contains(searchTerms[0]) || 
						item.BoundItem.Name.ToLower().Contains(searchTerms[1]))
					{
						results.Add(item);
						break;
					}
					else if (property.Desc.ToLower().Contains(searchTerms[0]) || 
						item.BoundItem.Name.ToLower().Contains(searchTerms[1]))
					{
						results.Add(item);
						break;
					}
				} // end property check
			} // end item name check
		}

		private void DefaultSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string searchTerm)
		{
			foreach (var item in itemsToSearch)
			{
				if (item.BoundItem.Name.ToLower().Contains(searchTerm))
				{
					results.Add(item);
					continue;
				}

				// the item name did not contain the search term
				// check the properties for the search term
				foreach (var property in item.BoundItem.Properties)
				{
					if (property.Name.ToLower().Contains(searchTerm))
					{
						results.Add(item);
						break;
					}
					else if (property.Desc.ToLower().Contains(searchTerm))
					{
						results.Add(item);
						break;
					}
				} // end property check
			} // end item name check
		}

		/// <summary>
		/// returns the 1st occurence of a specialWord contained in the searchTerm
		/// </summary>
		/// <param name="searchTerm">term to search for specialWord</param>
		/// <returns>1st occurence of a specialWord in the provided searchTerm or string.Empty if there is not one</returns>
		private string ExtractSpecialWord(string searchTerm)
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
		private bool ContainsSpecialWord(string searchTerm)
		{
			foreach (string s in specialWords)
			{
				if (searchTerm.Contains(s)) return true;
			}

			return false;
		}
	}
}
