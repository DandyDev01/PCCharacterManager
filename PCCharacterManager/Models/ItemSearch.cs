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
		public IEnumerable<ItemViewModel> Search(string searchTerm, IEnumerable<ItemViewModel> itemsToSearch)
		{
			//searchTerm = searchTerm.ToLower();

			List<ItemViewModel> result = new List<ItemViewModel>();

			if (searchTerm == string.Empty || string.IsNullOrWhiteSpace(searchTerm))
			{
				return itemsToSearch;
			}
			else
			{
				foreach (var item in itemsToSearch)
				{
					if (item.BoundItem.Name.ToLower().Contains(searchTerm))
					{
						result.Add(item);
						continue;
					}

					// the item name did not contain the search term
					// check the properties for the search term
					foreach (var property in item.BoundItem.Properties)
					{
						if (property.Name.ToLower().Contains(searchTerm))
						{
							result.Add(item);
							break;
						}
						else if (property.Desc.ToLower().Contains(searchTerm))
						{
							result.Add(item);
							break;
						}
					} // end property check
				} // end item name check
			} // end if

			return result;
		}
	}
}
