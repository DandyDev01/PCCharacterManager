using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class TabControlViewModel : TabItemViewModel
	{
		public CharacterStatsViewModel CharacterStatsVM { get; private set; }
		public CharacterListViewModel CharacterListVM { get; private set; }
		public CharacterInventoryViewModel InventoryVM { get; private set; }
		public CharacterSpellBookViewModel SpellBookVM { get; private set; }
		public CharacterNoteBookViewModel NotesVM { get; private set; }

		public TabControlViewModel(CharacterStore _characterStore, ICharacterDataService _dataService)
			: base(_characterStore, _dataService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			CharacterListVM = new CharacterListViewModel(_characterStore, dataService);
			CharacterStatsVM = new CharacterStatsViewModel(_characterStore, dataService);
			InventoryVM = new CharacterInventoryViewModel(_characterStore, dataService, _characterStore.SelectedCharacter.Inventory);
			SpellBookVM = new CharacterSpellBookViewModel(_characterStore, dataService);
			NotesVM = new CharacterNoteBookViewModel(_characterStore, dataService, _characterStore.SelectedCharacter.NoteManager);
		}
	}
}
