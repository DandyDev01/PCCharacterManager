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
		private StarfinderCharacter selectedCharacter;

		public StarfinderSkill[] Skills { get; private set; }
		public StarfinderAbility[] Abilities { get; private set; }

		public StarfinderAbilitiesAndSkillsViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;
			OnCharacterChanged(_characterStore.SelectedCharacter);
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			if (newCharacter == null) return;

			selectedCharacter = newCharacter as StarfinderCharacter;
			if (selectedCharacter == null) return;

			Skills = StarfinderAbility.GetSkills(selectedCharacter.Abilities);
			OnPropertyChaged("Skills");

			Abilities = selectedCharacter.Abilities;
			OnPropertyChaged("Abilities");
		}
	}
}
