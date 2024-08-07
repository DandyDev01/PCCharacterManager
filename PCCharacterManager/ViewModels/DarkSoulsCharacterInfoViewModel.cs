﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

		public ICommand EditDrivePointsCommand { get; }

		public DarkSoulsCharacterInfoViewModel(CharacterStore characterStore, DialogServiceBase dialogService, 
			RecoveryBase recovery) : base(characterStore, dialogService, recovery)
		{
			Race = "Unkindled";
			EditDrivePointsCommand = new RelayCommand(EditDrivePoints);
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		protected override void OnCharacterChanged(CharacterBase newCharacter)
		{
			if(_selectedCharacter is not null)
			{
				_selectedCharacter.CharacterClass.Features.CollectionChanged -= UpdateFeatures;
				_selectedCharacter.Health.PropertyChanged -= UpdateHealth;
			}

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
			SelectedCharacter.Health.PropertyChanged += UpdateHealth;

			//FeaturesListVM.UpdateCollection(null);
			ConditionsListVM.UpdateCollection(SelectedCharacter.Conditions);
			MovementTypesListVM.UpdateCollection(SelectedCharacter.MovementTypes_Speeds);
			CombatActionsVM.UpdateCollection(SelectedCharacter.CombatActions);
			ArmorProfsVM.UpdateCollection(SelectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(SelectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(SelectedCharacter.WeaponProficiencies);

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

		protected override void AdjustExperience()
		{
			if (_selectedCharacter is null)
				return;

			DialogWindowStringInputViewModel dataContext = new("Enter amount to add or remove.");

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			int temp;
			try
			{
				temp = int.Parse(dataContext.Answer);
			}
			catch
			{
				_dialogService.ShowMessage("Must be a whole number", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
				AdjustExperience();
				return;
			}

			_selectedCharacter.Level.ExperiencePoints += temp;

			// NOTE: check if they can level up, if they can, ask if they want to. 
		}

		protected override void EditCharacter()
		{
			if (SelectedCharacter == null)
				return;

			DialogWindowEditDarkSoulsCharacterViewModel windowVM = new(SelectedCharacter, _dialogService);

			string result = string.Empty;
			_dialogService.ShowDialog<EditDarkSoulsCharacterDialogWindow, DialogWindowEditDarkSoulsCharacterViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;
		}

		protected override void AddHealth()
		{
			if (_selectedCharacter is null)
				return;

			DialogWindowChangeHealthViewModel dataContext = new();
			string result = string.Empty;
			_dialogService.ShowDialog<ChangeHealthDialogWindow, DialogWindowChangeHealthViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			var characterHealth = _selectedCharacter.Health;

			if (dataContext.IsTempHealth)
			{
				characterHealth.TempHitPoints += dataContext.Amount;
				characterHealth.TempHitPoints = Math.Clamp(characterHealth.TempHitPoints, 0, 1000000);
			}
			else
			{
				characterHealth.CurrHealth += dataContext.Amount;
				characterHealth.CurrHealth = Math.Clamp(characterHealth.CurrHealth, 0, characterHealth.MaxHealth);
			}

			UpdateHealth();
		}

		protected override void UpdateHealth()
		{
			if (_selectedCharacter is null)
				return;

			Health = _selectedCharacter.Health.CurrHealth.ToString() + '/' + _selectedCharacter.Health.MaxHealth.ToString() 
				+ " (" + _selectedCharacter.Health.TempHitPoints + " temp)";
		}

		protected override void AddFeature()
		{
			DialogWindowAddFeatureViewModel windowVM = new(this);
			string result = string.Empty;
			_dialogService.ShowDialog<AddFeatureDialogWindow, DialogWindowAddFeatureViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			FeatureTypeSortCommand?.Execute(null);
		}

		protected override void EditArmorClass()
		{
			if (_selectedCharacter is null)
				return;

			DialogWindowEditArmorClassViewModel dataContext = new(_selectedCharacter.ArmorClass);

			string result = string.Empty;
			_dialogService.ShowDialog<EditArmorClassDialogWindow, DialogWindowEditArmorClassViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedCharacter.ArmorClass.Armor = dataContext.Armor;
			_selectedCharacter.ArmorClass.Shild = dataContext.Shild;
			_selectedCharacter.ArmorClass.Misc = dataContext.Misc;
			_selectedCharacter.ArmorClass.Temp = dataContext.Temp;

			ArmorClass = _selectedCharacter.ArmorClass.TotalArmorClass;
		}

		protected override void RemoveFeature()
		{
			if (_selectedProperty == null || _selectedCharacter == null)
				return;

			Feature feature = _selectedProperty;
			AllFeatures.Remove(_selectedProperty);

			if (feature.FeatureType == _selectedCharacter.CharacterClass.Name)
			{
				// does not handle same name items
				var toRemove = _selectedCharacter.CharacterClass.Features.Where(x => x.Name == feature.Name).First();
				_selectedCharacter.CharacterClass.Features.Remove(toRemove);
			}
			else if (feature.FeatureType == _selectedCharacter.Race.Name)
			{
				// does not handle same name items
				var toRemove = _selectedCharacter.Race.Features.Where(x => x.Name == feature.Name).First();
				_selectedCharacter.Race.Features.Remove(toRemove);
			}
			else if (feature.FeatureType == _selectedCharacter.Race.RaceVariant.Name)
			{
				// does not handle same name items
				var toRemove = _selectedCharacter.Race.RaceVariant.Properties.Where(x => x.Name == feature.Name).First();
				_selectedCharacter.Race.RaceVariant.Properties.Remove(toRemove);
			}

			_selectedProperty = AllFeatures.FirstOrDefault();
		}

		private void EditDrivePoints()
		{
			if (_selectedCharacter is null)
				return;

			DialogWindowStringInputViewModel dataContext = new("Enter amount to add or remove.");

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			int temp;
			try
			{
				temp = int.Parse(dataContext.Answer);
			}
			catch
			{
				_dialogService.ShowMessage("Must be a whole number", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
				EditDrivePoints();
				return;
			}

			_selectedCharacter.DrivePoints = temp;

			// NOTE: check if they can level up, if they can, ask if they want to. 
		}
	}
}
