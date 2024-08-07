﻿using PCCharacterManager.Models;
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
		private StarfinderCharacter? _selectedCharacter;

		public StarfinderSkill[] Skills { get; private set; }
		public StarfinderAbility[] Abilities { get; private set; }

		public StarfinderAbilitiesAndSkillsViewModel(CharacterStore characterStore)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			Skills = StarfinderSkill.Default;
			Abilities = StarfinderAbility.Default;

			if (characterStore.SelectedCharacter == null)
				return;

			//OnCharacterChanged(_characterStore.SelectedCharacter);
		}

		private void OnCharacterChanged(CharacterBase newCharacter)
		{
			if (newCharacter == null) 
				return;

			if (newCharacter is not StarfinderCharacter starFinderCharacter)
				return;

			_selectedCharacter = starFinderCharacter;

			Skills = StarfinderAbility.GetSkills(_selectedCharacter.Abilities);
			Skills = Skills.OrderBy(x => x.Name).ToArray();
			OnPropertyChanged(nameof(Skills));

			Abilities = _selectedCharacter.Abilities;
			OnPropertyChanged(nameof(Abilities));
		}
	}
}
