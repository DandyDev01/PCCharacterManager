using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class CharacterInventoryViewModelTests
	{
		[TestMethod()]
		public void PopulatePropertiesToDisplayTest()
		{
			var recovery = new SimpleCharacterRecovery();
			var characterStore = new CharacterStore(recovery);
			var dialogService = new PassDialogService();
			CharacterInventoryViewModel vm = new(characterStore, dialogService, recovery);

			Item item = new();
			Property visibel = new();
			Property hidden = new();
			hidden.Hidden = true;

			item.AddProperty(visibel);
			item.AddProperty(hidden);

			vm.SelectedItem = new ItemViewModel(item);
			
			Assert.AreEqual(2, vm.PropertiesToDisplay.Count);
			
			vm.ShowPropertiesToDisplayCommand.Execute(vm);

			Assert.AreEqual("Don't show hidden properties", vm.ShowHiddenPropertiesText);
			Assert.AreEqual(1, vm.PropertiesToDisplay.Count);
		}

		[TestMethod()]
		public void CalculateInventoryWeightTest()
		{
			var recovery = new SimpleCharacterRecovery();
			var characterStore = new CharacterStore(recovery);
			var dialogService = new PassDialogService();
			CharacterInventoryViewModel vm = new(characterStore, dialogService, recovery);
			DnD5eCharacter character = CharacterCreatorViewModel.CreateRandonCharacter();
			characterStore.BindSelectedCharacter(character);

			Item item = new();
			Property visibel = new();
			Property hidden = new();
			hidden.Hidden = true;

			item.Weight = 10.ToString();
			item.AddProperty(visibel);
			item.AddProperty(hidden);

			character.Inventory.Add(item);

			vm.CalculateInventoryWeight();
			Assert.AreEqual(vm.InventoryWeight, item.Weight + "/" + character.CarryWeight);
		}

		[TestMethod]
		public void OtherConstructor()
		{
			var items = ReadWriteJsonCollection<Item>.ReadCollection(DnD5eResources.AllItemsJson);
			var recovery = new SimpleCharacterRecovery();
			ObservableCollection<ItemViewModel> itemVMs = new ObservableCollection<ItemViewModel>();

			foreach (var item in items)
			{
				itemVMs.Add(new ItemViewModel(item));
			}

			var dialogService = new PassDialogService();
			CharacterInventoryViewModel vm = new(itemVMs, dialogService, recovery);
		}
	}
}
