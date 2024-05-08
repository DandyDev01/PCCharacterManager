using Accessibility;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	public class DialogWindowDnD5eCharacterLevelupViewModel : ObservableObject
	{
		private readonly DialogServiceBase _dialogService;
		private readonly DnD5eCharacter _character;

		public ICommand AddClassCommand { get; }
		public ICommand RollHitdieCommand { get; }

		public DialogWindowDnD5eCharacterLevelupViewModel(DialogServiceBase dialogService, DnD5eCharacter character)
		{
			_dialogService = dialogService;
			_character = character;
			AddClassCommand = new RelayCommand(AddClass);
			RollHitdieCommand = new RelayCommand(RollForMaxHealth);
		}

		private void AddClass()
		{
			string classToAddName = GetClassToAddName(_character, _character.CharacterClass.Name);
		}

		private void IncreaseAbilityScore()
		{
			throw new NotImplementedException();
		}

		private void RollForMaxHealth()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get the name of the class to add.
		/// </summary>
		/// <returns>Name of the class the user wants to add.</returns>
		private string GetClassToAddName(DnD5eCharacter character, string currentClasses)
		{
			var classes = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray();
			string[] classNames = new string[classes.Length];

			for (int i = 0; i < classes.Length; i++)
			{
				// exclude classes the character already has and classes the character does
				// not meet the prerequisites for.
				if (currentClasses.Contains(classes[i].Name) || MeetsPrerequisites(character, classes[i]) == false)
					continue;

				classNames[i] = classes[i].Name;
			}

			classNames = classNames.Where(x => x is not null).ToArray();

			DialogWindowSelectStingValueViewModel vm =
				new DialogWindowSelectStingValueViewModel(classNames, 1);

			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(vm, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return string.Empty;

			return vm.SelectedItems.First();
		}

		/// <summary>
		/// Determines if the character meets the prerequisites for the class they want to multiclass in.
		/// </summary>
		/// <param name="character">Character that is being checked.</param>
		/// <param name="characterMultiClassData">Multiclass prerequisite data.</param>
		/// <returns></returns>
		private bool MeetsPrerequisites(DnD5eCharacter character, CharacterMultiClassData characterMultiClassData)
		{
			string[] prerequisites = characterMultiClassData.Prerequisites.Split('^', '&');
			int[] score = new int[prerequisites.Length];

			// get ability prerequisite name and score.
			for (int i = 0; i < prerequisites.Length; i++)
			{
				prerequisites[i] = prerequisites[i].Trim();
				if (int.TryParse(prerequisites[i].Substring(prerequisites[i].IndexOf(" ")).Trim(), out score[i]) == false)
				{
					throw new Exception("Could not find score.");
				}
				prerequisites[i] = prerequisites[i].Substring(0, prerequisites[i].IndexOf(" ")).Trim();
			}

			// prerequsite contains an OR
			if (characterMultiClassData.Prerequisites.Contains('^'))
			{
				bool meetsOne = false;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i]).First();
					if (ability.Score >= score[i])
						meetsOne = true;
				}

				if (meetsOne)
					return true;
			}
			// prerequisite contains an AND
			else if (characterMultiClassData.Prerequisites.Contains('&'))
			{
				bool meetsAll = true;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i]).First();
					if (ability.Score < score[i])
						meetsAll = false;
				}

				if (meetsAll)
					return true;
			}
			// there is only a single prerequisite 
			else
			{
				string abilityname = characterMultiClassData.Prerequisites;
				int abilityScore = 0;
				if (int.TryParse(abilityname.Substring(abilityname.IndexOf(" ")).Trim(), out abilityScore) == false)
				{
					throw new Exception("Could not find score.");
				}

				abilityname = abilityname.Substring(0, abilityname.IndexOf(" ")).Trim();
				Ability a = character.Abilities.Where(x => x.Name == abilityname).First();

				if (a.Score >= abilityScore)
					return true;
			}

			return false;
		}

		public void ProcessLevelup()
		{
			throw new NotImplementedException();
		}
	}
}
