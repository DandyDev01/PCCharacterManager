using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class ItemSearch : ISearch<ItemDisplayViewModel>
	{
		public IEnumerable<ItemDisplayViewModel> Search(string searchTerm, IEnumerable<ItemDisplayViewModel> itemsToSearch)
		{
			List<ItemDisplayViewModel> result = new List<ItemDisplayViewModel>();

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
					}
				}
			} // end if

			return result;
		}
	}
}
