using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class StarfinderAbilitiesAndSkillsViewModel : ObservableObject
	{
		private StarfinderCharacter? selectedCharacter;

		public StarfinderSkill[] Skills { get; private set; }
		public StarfinderAbility[] Abilities { get; private set; }

		public StarfinderAbilitiesAndSkillsViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			Skills = StarfinderSkill.Default;
			Abilities = StarfinderAbility.Default;

			if (_characterStore.SelectedCharacter == null)
				return;

			//OnCharacterChanged(_characterStore.SelectedCharacter);
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			if (newCharacter == null) 
				return;

			if (newCharacter is not StarfinderCharacter starFinderCharacter)
				return;

			selectedCharacter = starFinderCharacter;

			Skills = StarfinderAbility.GetSkills(selectedCharacter.Abilities);
			Skills = Skills.OrderBy(x => x.Name).ToArray();
			OnPropertyChanged(nameof(Skills));

			Abilities = selectedCharacter.Abilities;
			OnPropertyChanged(nameof(Abilities));
		}
	}
}
