using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;

namespace PCCharacterManager.ViewModels
{
	public class TabControlViewModel
	{
		public CharacterStatsViewModel CharacterStatsVM { get; }
		public CharacterListViewModel CharacterListVM { get; }
		public CharacterInventoryViewModel InventoryVM { get; }
		public CharacterSpellBookViewModel SpellBookVM { get; }
		public CharacterNoteBookViewModel NotesVM { get; }

		public TabControlViewModel(CharacterStore characterStore, ICharacterDataService dataService,
			DialogServiceBase dialogService, RecoveryBase recovery)
		{
			CharacterListVM = new CharacterListViewModel(characterStore, dataService, dialogService);
			CharacterStatsVM = new CharacterStatsViewModel(characterStore, dialogService, recovery);
			InventoryVM = new CharacterInventoryViewModel(characterStore, dialogService, recovery);
			SpellBookVM = new CharacterSpellBookViewModel(characterStore, dialogService, recovery);
			NotesVM = new CharacterNoteBookViewModel(characterStore, dialogService, recovery);
		}
	}
}
