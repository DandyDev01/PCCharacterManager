using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public abstract class ISearch<T>
	{
		protected string[] specialWords = { "&&", "||", "*" };

		public IEnumerable<T> Search(string searchTerm, IEnumerable<T> itemsToSearch)
		{
			if (searchTerm == string.Empty || string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Contains("*"))
				return itemsToSearch.ToList();

			List<T> results = new List<T>();

			bool containsSpecialWord = ContainsSpecialWord(searchTerm);
			if (containsSpecialWord) SpecialWordSearch(ref results, itemsToSearch, searchTerm);
			else DefaultSearch(ref results, itemsToSearch, searchTerm);

			return results;
		}

		/// <summary>
		/// gets item that contain both search terms
		/// </summary>
		/// <param name="results">list of results to add items too</param>
		/// <param name="itemsToSearch">items to search through</param>
		/// <param name="searchTerms">terms to look for</param>
		protected abstract void ANDSearch(ref List<T> results, IEnumerable<T> itemsToSearch, string[] searchTerms);

		/// <summary>
		/// gets items that contain either search term
		/// </summary>
		/// <param name="results">list of results to add items too</param>
		/// <param name="itemsToSearch">items to search through</param>
		/// <param name="searchTerms">terms to look for</param>
		protected abstract void ORSearch(ref List<T> results, IEnumerable<T> itemsToSearch, string[] searchTerms);

		protected abstract void DefaultSearch(ref List<T> results, IEnumerable<T> itemsToSearch, string searchTerm);
		
		protected void SpecialWordSearch(ref List<T> results, IEnumerable<T> itemsToSearch, string searchTerm)
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
	}
}
