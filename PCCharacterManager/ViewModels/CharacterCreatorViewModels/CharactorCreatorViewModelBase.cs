using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels.CharacterCreatorViewModels
{
	public abstract class CharactorCreatorViewModelBase : ObservableObject
	{
		public abstract DnD5eCharacter Create();
	}
}
