using PCCharacterManager.Commands;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowAddItemViewModel : ObservableObject
	{ 	
		private ObservableCollection<ItemViewModel> AllItemVMs { get; }
		public CharacterInventoryViewModel InventoryVM { get; }

		public DialogWindowAddItemViewModel(DialogServiceBase dialogService)
		{
			IEnumerable<Item> allItems = ReadWriteJsonCollection<Item>.ReadCollection(DnD5eResources.AllItemsJson);
			AllItemVMs = new ObservableCollection<ItemViewModel>();
			foreach (Item item in allItems)
			{
				ItemViewModel itemVM = new();
				itemVM.Bind(item);
				AllItemVMs.Add(itemVM);
			}
			InventoryVM = new CharacterInventoryViewModel(AllItemVMs, dialogService, null);
		
		}
	}
}
