using PCCharacterManager.Models;
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
	public class DarkSoulsCharacterInfoViewModel : CharacterInfoViewModel
	{
		private DarkSoulsCharacter _selectedCharacter;
		public new DarkSoulsCharacter SelectedCharacter
		{
			get
			{
				return _selectedCharacter;
			}
			set
			{
				OnPropertyChanged(ref _selectedCharacter, value);
			}
		}

		public DarkSoulsCharacterInfoViewModel(CharacterStore characterStore, DialogServiceBase dialogService, 
			RecoveryBase recovery) : base(characterStore, dialogService, recovery)
		{
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			if (SelectedCharacter is not null)
			{
				SelectedCharacter.CharacterClass.Features.CollectionChanged -= UpdateFeatures;
			}
			else
			{
				return;
			}

			SelectedCharacter = newCharacter as DarkSoulsCharacter;

			SelectedCharacter.CharacterClass.Features.CollectionChanged += UpdateFeatures;

			//FeaturesListVM.UpdateCollection(null);
			ConditionsListVM.UpdateCollection(_selectedCharacter.Conditions);
			MovementTypesListVM.UpdateCollection(_selectedCharacter.MovementTypes_Speeds);
			LanguagesVM.UpdateCollection(_selectedCharacter.Languages);
			CombatActionsVM.UpdateCollection(_selectedCharacter.CombatActions);
			ToolProfsVM.UpdateCollection(_selectedCharacter.ToolProficiences);
			ArmorProfsVM.UpdateCollection(_selectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(_selectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(_selectedCharacter.WeaponProficiencies);

			Race = _selectedCharacter.Race.RaceVariant.Name;

			var temp = _selectedCharacter.Health;
			Health = temp.CurrHealth.ToString() + '/' + temp.MaxHealth + " (" + temp.TempHitPoints + " temp)";

			var characterClass = _selectedCharacter.CharacterClass;
			CharacterClass = characterClass.Name + "(total: " + _selectedCharacter.Level.Level
				+ ", PB: " + _selectedCharacter.Level.ProficiencyBonus + ")";

			ArmorClass = _selectedCharacter.ArmorClass.TotalArmorClass;

			UpdateFeatures(null, null);
			SelectedProperty = AllFeatures.FirstOrDefault();
		}
	}
}
