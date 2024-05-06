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

		public TabControlViewModel(CharacterStore characterStore, ICharacterDataService dataService, DialogService dialogService)
		{
			CharacterListVM = new CharacterListViewModel(characterStore, dataService, dialogService);
			CharacterStatsVM = new CharacterStatsViewModel(characterStore, dialogService);
			InventoryVM = new CharacterInventoryViewModel(characterStore, dialogService);
			SpellBookVM = new CharacterSpellBookViewModel(characterStore, dialogService);
			NotesVM = new CharacterNoteBookViewModel(characterStore, dialogService);
		}
	}
}
