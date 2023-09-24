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
	public class TabControlViewModel
	{
		public CharacterStatsViewModel CharacterStatsVM { get; private set; }
		public CharacterListViewModel CharacterListVM { get; private set; }
		public CharacterInventoryViewModel InventoryVM { get; private set; }
		public CharacterSpellBookViewModel SpellBookVM { get; private set; }
		public CharacterNoteBookViewModel NotesVM { get; private set; }

		public TabControlViewModel(CharacterStore _characterStore, ICharacterDataService _dataService)
		{
			CharacterListVM = new CharacterListViewModel(_characterStore, _dataService);
			CharacterStatsVM = new CharacterStatsViewModel(_characterStore);
			InventoryVM = new CharacterInventoryViewModel(_characterStore);
			SpellBookVM = new CharacterSpellBookViewModel(_characterStore);
			NotesVM = new CharacterNoteBookViewModel(_characterStore);
		}
	}
}
