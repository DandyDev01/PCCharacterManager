using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class DarkSoulsCharacterInfoViewModel : CharacterInfoViewModel
	{
		private DarkSoulsCharacter? _selectedCharacter;
		public new DarkSoulsCharacter? SelectedCharacter
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
			Race = "Unkindled";
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		protected override void OnCharacterChanged(CharacterBase newCharacter)
		{
			if(_selectedCharacter is not null)
				_selectedCharacter.CharacterClass.Features.CollectionChanged -= UpdateFeatures;


			if (CharacterTypeHelper.IsValidCharacterType(newCharacter, CharacterType.dark_souls)
				&& newCharacter is DarkSoulsCharacter c)
			{
				SelectedCharacter = c;
			}
			else
			{
				SelectedCharacter = null;
				return;
			}

			SelectedCharacter.CharacterClass.Features.CollectionChanged += UpdateFeatures;

			//FeaturesListVM.UpdateCollection(null);
			ConditionsListVM.UpdateCollection(SelectedCharacter.Conditions);
			MovementTypesListVM.UpdateCollection(SelectedCharacter.MovementTypes_Speeds);
			LanguagesVM.UpdateCollection(SelectedCharacter.Languages);
			CombatActionsVM.UpdateCollection(SelectedCharacter.CombatActions);
			ToolProfsVM.UpdateCollection(SelectedCharacter.ToolProficiences);
			ArmorProfsVM.UpdateCollection(SelectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(SelectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(SelectedCharacter.WeaponProficiencies);

			Race = SelectedCharacter.Race.RaceVariant.Name;

			var temp = SelectedCharacter.Health;
			Health = temp.CurrHealth.ToString() + '/' + temp.MaxHealth + " (" + temp.TempHitPoints + " temp)";

			var characterClass = SelectedCharacter.CharacterClass;
			CharacterClass = characterClass.Name + "(total: " + SelectedCharacter.Level.Level
				+ ", PB: " + SelectedCharacter.Level.ProficiencyBonus + ")";

			ArmorClass = SelectedCharacter.ArmorClass.TotalArmorClass;

			UpdateFeatures(null, null);
			SelectedProperty = AllFeatures.FirstOrDefault();
		}

		protected override void UpdateFeatures(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			AllFeatures.Clear();

			if (_selectedCharacter is null)
				return;

			foreach (var item in _selectedCharacter.CharacterClass.Features)
			{
				AllFeatures.Add(new Feature(item, _selectedCharacter.CharacterClass.Name, item.Level.ToString()));
			}

			AllFeatures.Add(new Feature(_selectedCharacter.Origin.BloodiedEffect, _selectedCharacter.Origin.Name, "-"));

			FeaturesCollectionView?.Refresh();
		}
	}
}
