using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using Condition = PCCharacterManager.Models.Condition;
using PCCharacterManager.Services;

namespace PCCharacterManager.ViewModels
{
	public class CharacterInfoViewModel : ObservableObject
	{
		protected readonly CollectionViewPropertySort _collectionViewPropertySort;
		protected readonly CharacterStore _characterStore;
		protected readonly DialogServiceBase _dialogService;
		protected readonly RecoveryBase _recovery;

		private DnD5eCharacter? _selectedCharacter;
		public DnD5eCharacter? SelectedCharacter
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

		public PropertyListViewModel MovementTypesListVM { get; }
		public PropertyListViewModel FeaturesListVM { get; }
		public ConditionListViewModel ConditionsListVM { get; }

		public StringListViewModel LanguagesVM { get; }
		public StringListViewModel CombatActionsVM { get; }
		public StringListViewModel ArmorProfsVM { get; }
		public StringListViewModel WeaponProfsVM { get; }
		public StringListViewModel ToolProfsVM { get; }
		public StringListViewModel OtherProfsVM { get; }

		public ObservableCollection<Feature> AllFeatures { get; }
		public ICollectionView FeaturesCollectionView { get; }

		protected Feature? _selectedProperty;
		public Feature? SelectedProperty
		{
			get
			{
				return _selectedProperty;
			}
			set
			{
				OnPropertyChanged(ref _selectedProperty, value);
			}
		}

		private string _race;
		public string Race
		{
			get
			{
				return _race;
			}
			set
			{
				OnPropertyChanged(ref _race, value);
			}
		}

		private string _health;
		public string Health
		{
			get
			{
				return _health;
			}
			set
			{
				OnPropertyChanged(ref _health, value);
			}
		}

		private string _armorClass;
		public string ArmorClass
		{
			get
			{
				return _armorClass;
			}
			set
			{
				OnPropertyChanged(ref _armorClass, value);
			}
		}

		private string _characterClass;
		public string CharacterClass
		{
			get
			{
				return _characterClass;
			}
			set
			{
				OnPropertyChanged(ref _characterClass, value);
			}
		}

		public ICommand NameSortCommand { get; }
		public ICommand FeatureTypeSortCommand { get; }
		public ICommand LevelSortCommand { get; }

		public ICommand ShortRestCommand { get; }
		public ICommand LongRestCommand { get; }
		public ICommand AddFeatureCommand { get; }
		public ICommand RemoveFeatureCommand { get; }
		public ICommand AdjustExperienceCommand { get; }
		public ICommand AdjustHealthCommand { get; }
		public ICommand EditArmorClassCommand { get; }
		public ICommand EditCharacterCommand { get; }
		public ICommand LevelCharacterCommand { get; }
		public ICommand NextCombatRoundCommand { get; }
		public ICommand StartEncounterCommand { get; }
		public ICommand EndEncounterCommand { get; }

		public CharacterInfoViewModel(CharacterStore characterStore, DialogServiceBase dialogService, RecoveryBase recovery)
		{
			_characterStore = characterStore;

			_recovery = recovery;
			_dialogService = dialogService;

			_race = string.Empty;
			_health = string.Empty;
			_armorClass = string.Empty;
			_characterClass = string.Empty;

			AllFeatures = new ObservableCollection<Feature>();
			FeaturesCollectionView = CollectionViewSource.GetDefaultView(AllFeatures);
			_collectionViewPropertySort = new CollectionViewPropertySort(FeaturesCollectionView);

			NameSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(Feature.Name));
			FeatureTypeSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(Feature.FeatureType));
			LevelSortCommand = new ItemCollectionViewPropertySortCommand(_collectionViewPropertySort,
				nameof(Feature.Level));

			NextCombatRoundCommand = new RelayCommand(NextCombatRound);
			StartEncounterCommand = new RelayCommand(StartEncounter);
			EndEncounterCommand = new RelayCommand(EndEncounter);

			AddFeatureCommand = new RelayCommand(AddFeature);
			RemoveFeatureCommand = new RelayCommand(RemoveFeature);

			_characterStore.OnCharacterLevelup += OnCharacterChanged;
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			_selectedProperty = AllFeatures.FirstOrDefault();	

			FeaturesListVM = new PropertyListViewModel("Features", dialogService);

			ConditionsListVM = new ConditionListViewModel("Conditions", dialogService);
			MovementTypesListVM = new PropertyListViewModel("Movement", dialogService);
			LanguagesVM = new StringListViewModel("Languages", dialogService);
			CombatActionsVM = new StringListViewModel("Actions", dialogService);
			ToolProfsVM = new StringListViewModel("Tool Profs", dialogService);
			ArmorProfsVM = new StringListViewModel("Armor Profs", dialogService);
			OtherProfsVM = new StringListViewModel("Other Profs", dialogService);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", dialogService);
			LevelCharacterCommand = new LevelCharacterCommand(_characterStore, dialogService);
			AdjustExperienceCommand = new RelayCommand(AdjustExperience);
			AdjustHealthCommand = new RelayCommand(AddHealth);
			EditArmorClassCommand = new RelayCommand(EditArmorClass);
			EditCharacterCommand = new RelayCommand(EditCharacter);
			ShortRestCommand = new RelayCommand(ShortRest);
			LongRestCommand = new RelayCommand(LongRest);
		}
		
		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		protected virtual void OnCharacterChanged(CharacterBase newCharacter)
		{
			if (_selectedCharacter is not null)
			{
				_selectedCharacter.CharacterClass.Features.CollectionChanged -= UpdateFeatures;
				_selectedCharacter.Health.PropertyChanged -= UpdateHealth;
			}


			if (CharacterTypeHelper.IsValidCharacterType(newCharacter, CharacterType.DnD5e)
				&& newCharacter is DnD5eCharacter c)
			{
				SelectedCharacter = c;
			}
			else
			{
				SelectedCharacter = null;
				return;
			}

			SelectedCharacter.Health.PropertyChanged += UpdateHealth;
			SelectedCharacter.CharacterClass.Features.CollectionChanged += UpdateFeatures;

			//FeaturesListVM.UpdateCollection(null);
			ConditionsListVM.UpdateCollection(SelectedCharacter.Conditions);
			MovementTypesListVM.UpdateCollection( SelectedCharacter.MovementTypes_Speeds);
			LanguagesVM.UpdateCollection(SelectedCharacter.Languages);
			CombatActionsVM.UpdateCollection(SelectedCharacter.CombatActions);
			ToolProfsVM.UpdateCollection(SelectedCharacter.ToolProficiences);
			ArmorProfsVM.UpdateCollection(SelectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(SelectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(SelectedCharacter.WeaponProficiencies);

			Race = SelectedCharacter.Race.Name + " " + SelectedCharacter.Race.RaceVariant.Name;

			Health characterHealth = SelectedCharacter.Health;
			Health = characterHealth.CurrHealth.ToString() + '/' + characterHealth.MaxHealth + " (" + 
				characterHealth.TempHitPoints + " temp)";

			DnD5eCharacterClass characterClass = SelectedCharacter.CharacterClass;
			CharacterClass = characterClass.Name + "(total: " + SelectedCharacter.Level.Level 
				+ ", PB: " + SelectedCharacter.Level.ProficiencyBonus + ")";

			ArmorClass = SelectedCharacter.ArmorClass.TotalArmorClass;

			UpdateFeatures(null, null);
			SelectedProperty = AllFeatures.FirstOrDefault();
		}

		protected void UpdateHealth(object? sender, PropertyChangedEventArgs e)
		{
			UpdateHealth();
		}

		private void LongRest()
		{
			if (_selectedCharacter is null)
				return;

			int maxNumberOfRegainedHitDie = Math.Max(1, _selectedCharacter.Level.Level / 2);
			int spentHitDie = _selectedCharacter.SpentHitDie;
			int regainedHitDie = Math.Clamp(_selectedCharacter.Level.Level - spentHitDie, 1, maxNumberOfRegainedHitDie);
			_selectedCharacter.SpentHitDie -= regainedHitDie;
			_selectedCharacter.SpentHitDie = Math.Clamp(_selectedCharacter.SpentHitDie, 0, _selectedCharacter.Level.Level);
			
			_selectedCharacter.Health.CurrHealth = _selectedCharacter.Health.MaxHealth;
			_selectedCharacter.SpellBook.RechargeSpellSlots();

			UpdateHealth();
		}

		private void ShortRest()
		{
			if (_selectedCharacter is null) 
				return;
			
			DialogWindowShortRestViewModel vm = new(_selectedCharacter);
			string result = string.Empty;
			_dialogService.ShowDialog<ShortRestDialogWindow, DialogWindowShortRestViewModel>(vm, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			_selectedCharacter.Health.CurrHealth = vm.Health;
			_selectedCharacter.SpentHitDie = vm.SpentHitDice;

			UpdateHealth();
		}

		private void EndEncounter()
		{
			if (_selectedCharacter is null)
				return;

			_selectedCharacter.CombatRound = 0;
			_selectedCharacter.IsInCombat = false;
			_selectedCharacter.Status = CharacterStatus.IDLE;
		}

		private void StartEncounter()
		{
			if (_selectedCharacter is null)
				return;

			_selectedCharacter.CombatRound = 0;
			_selectedCharacter.IsInCombat = true;
			_selectedCharacter.Status = CharacterStatus.COMBAT;
		}

		private void NextCombatRound()
		{
			if (_selectedCharacter is null)
				return;

			_selectedCharacter.CombatRound += 1;

			if (_selectedCharacter.Conditions.Count <= 0)
				return;

			foreach (var condition in _selectedCharacter.Conditions)
			{
				condition.PassRound();
			}

			Condition[] expiredCondition = _selectedCharacter.Conditions
				.Where(x => x.RoundsPassed >= x.DurationInRounds).ToArray();
			

			foreach (Condition condition in expiredCondition)
			{
				_selectedCharacter.Conditions.Remove(condition);
			}
		}

		protected virtual void EditArmorClass()
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

		protected virtual void AdjustExperience()
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

		protected virtual void AddHealth()
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

		protected virtual void UpdateHealth()
		{
			if (_selectedCharacter is null)
				return;

			Health = _selectedCharacter.Health.CurrHealth.ToString() + '/' + _selectedCharacter.Health.MaxHealth.ToString() 
				+ " (" + _selectedCharacter.Health.TempHitPoints + " temp)";
		}

		protected virtual void AddFeature()
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

		protected virtual void RemoveFeature()
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

		protected virtual void EditCharacter()
		{
			if (SelectedCharacter == null)
				return;

			DialogWindowEditCharacterViewModel windowVM = new(SelectedCharacter, _dialogService);

			string result = string.Empty;
			_dialogService.ShowDialog<EditCharacterDialogWindow, DialogWindowEditCharacterViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;
		}

		protected virtual void UpdateFeatures(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			AllFeatures.Clear();

			if (_selectedCharacter is null)
				return;

			foreach (var item in _selectedCharacter.CharacterClass.Features)
			{
				AllFeatures.Add(new Feature(item, _selectedCharacter.CharacterClass.Name, item.Level.ToString()));
			}

			foreach (var item in _selectedCharacter.Race.Features)
			{
				AllFeatures.Add(new Feature(item, _selectedCharacter.Race.Name, "-"));
			}

			foreach (var item in _selectedCharacter.Race.RaceVariant.Properties)
			{
				AllFeatures.Add(new Feature(item, _selectedCharacter.Race.RaceVariant.Name, "-"));
			}

			FeaturesCollectionView?.Refresh();
		}
	}
}
