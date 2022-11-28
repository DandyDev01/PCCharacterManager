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
		/// <summary>
		/// gets item that contain both search terms
		/// </summary>
		/// <param name="results">list of results to add items too</param>
		/// <param name="itemsToSearch">items to search through</param>
		/// <param name="searchTerms">terms to look for</param>
		protected override void ANDSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string[] searchTerms)
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

		/// <summary>
		/// gets items that contain either search term
		/// </summary>
		/// <param name="results">list of results to add items too</param>
		/// <param name="itemsToSearch">items to search through</param>
		/// <param name="searchTerms">terms to look for</param>
		protected override void ORSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string[] searchTerms)
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

		protected override void DefaultSearch(ref List<ItemViewModel> results, IEnumerable<ItemViewModel> itemsToSearch, string searchTerm)
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
	}
}
