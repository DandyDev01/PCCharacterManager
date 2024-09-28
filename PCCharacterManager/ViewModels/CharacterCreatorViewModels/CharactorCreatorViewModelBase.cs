using PCCharacterManager.Models;
using PCCharacterManager.Utility;

namespace PCCharacterManager.ViewModels.CharacterCreatorViewModels
{
	public abstract class CharactorCreatorViewModelBase : ObservableObject
	{
		public abstract CharacterBase? Create();
	}
}
