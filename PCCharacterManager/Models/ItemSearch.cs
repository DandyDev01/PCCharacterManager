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
		public override bool Search(object obj)
		{
			if (obj is ItemViewModel itemVM)
			{
				if (itemVM.BoundItem == null)
					return false;

				if (searchTerm.Equals(string.Empty))
					return true;

				if (itemVM.DisplayName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
					return true;

				if (itemVM.DisplayItemCategory.ToString().Contains(SearchTerm,
					StringComparison.OrdinalIgnoreCase))
					return true;

				if (itemVM.DisplayItemType.ToString().Contains(SearchTerm,
					StringComparison.OrdinalIgnoreCase))
					return true;


				foreach (Property property in itemVM.BoundItem.Properties)
				{
					if (property.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
						return true;

					if (property.Desc.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
						return true;
				}
			}

			return false;
		}
	}
}
