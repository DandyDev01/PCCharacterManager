using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class StarfinderCharacterCreatorViewModel : ObservableObject
	{
		private readonly ICharacterDataService dataService;
		private readonly CharacterStore character;

		public StarfinderCharacterCreatorViewModel(ICharacterDataService _dataService, CharacterStore _characterStore)
		{
			dataService = _dataService;
			character = _characterStore;
		}
	}
}
