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

namespace PCCharacterManager.ViewModels
{
	public class CharacterInfoViewModel : ObservableObject
	{
		private readonly CollectionViewPropertySort collectionViewPropertySort;
		private readonly CharacterStore characterStore;

		private DnD5eCharacter? selectedCharacter;
		public DnD5eCharacter? SelectedCharacter
		{
			get
			{
				return selectedCharacter;
			}
			set
			{
				OnPropertyChanged(ref selectedCharacter, value);
			}
		}

		public PropertyListViewModel MovementTypesListVM { get; }
		public PropertyListViewModel FeaturesListVM { get; }
		public PropertyListViewModel ConditionsListVM { get; }

		public StringListViewModel LanguagesVM { get; }
		public StringListViewModel ArmorProfsVM { get; }
		public StringListViewModel WeaponProfsVM { get; }
		public StringListViewModel ToolProfsVM { get; }
		public StringListViewModel OtherProfsVM { get; }

		public ObservableCollection<Feature> AllFeatures { get; }
		public ICollectionView FeaturesCollectionView { get; }

		private Feature? selectedProperty;
		public Feature? SelectedProperty
		{
			get
			{
				return selectedProperty;
			}
			set
			{
				OnPropertyChanged(ref selectedProperty, value);
			}
		}

		private string race;
		public string Race
		{
			get
			{
				return race;
			}
			set
			{
				OnPropertyChanged(ref race, value);
			}
		}

		private string health;
		public string Health
		{
			get
			{
				return health;
			}
			set
			{
				OnPropertyChanged(ref health, value);
			}
		}

		private string armorClass;
		public string ArmorClass
		{
			get
			{
				return armorClass;
			}
			set
			{
				OnPropertyChanged(ref armorClass, value);
			}
		}

		private string characterClass;
		public string CharacterClass
		{
			get
			{
				return characterClass;
			}
			set
			{
				OnPropertyChanged(ref characterClass, value);
			}
		}

		private string hitDie;
		public string HitDie
		{
			get
			{
				return hitDie;
			}
			set
			{
				OnPropertyChanged(ref hitDie, value);
			}
		}

		public ICommand NameSortCommand { get; }
		public ICommand FeatureTypeSortCommand { get; }
		public ICommand LevelSortCommand { get; }
		public ICommand AddFeatureCommand { get; }
		public ICommand RemoveFeatureCommand { get; }
		public ICommand AdjustExperienceCommand { get; }
		public ICommand AdjustHealthCommand { get; }
		public ICommand EditArmorClassCommand { get; }
		public ICommand EditCharacterCommand { get; }
		public ICommand LevelCharacterCommand { get; }

		public CharacterInfoViewModel(CharacterStore _characterStore)
		{
			characterStore = _characterStore;
			AllFeatures = new ObservableCollection<Feature>();
			FeaturesCollectionView = CollectionViewSource.GetDefaultView(AllFeatures);
			collectionViewPropertySort = new CollectionViewPropertySort(FeaturesCollectionView);

			NameSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(Feature.Name));
			FeatureTypeSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(Feature.FeatureType));
			LevelSortCommand = new ItemCollectionViewPropertySortCommand(collectionViewPropertySort,
				nameof(Feature.Level));

			AddFeatureCommand = new RelayCommand(AddFeature);
			RemoveFeatureCommand = new RelayCommand(RemoveFeature);

			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			selectedCharacter ??= new();

			selectedProperty = AllFeatures.FirstOrDefault();	

			FeaturesListVM = new PropertyListViewModel("Features");

			ConditionsListVM = new PropertyListViewModel("Conditions");
			MovementTypesListVM = new PropertyListViewModel("Movement", selectedCharacter.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", selectedCharacter.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", selectedCharacter.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", selectedCharacter.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", selectedCharacter.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", selectedCharacter.WeaponProficiencies);
			LevelCharacterCommand = new LevelCharacterCommand(characterStore);
			AdjustExperienceCommand = new RelayCommand(AdjustExperience);
			AdjustHealthCommand = new RelayCommand(AddHealth);
			EditArmorClassCommand = new RelayCommand(EditArmorClass);
			EditCharacterCommand = new RelayCommand(EditCharacter);
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			if (SelectedCharacter is not null)
				SelectedCharacter.CharacterClass.Features.CollectionChanged -= UpdateFeatures;

			SelectedCharacter = newCharacter;

			SelectedCharacter.CharacterClass.Features.CollectionChanged += UpdateFeatures;

			if (selectedCharacter is null)
				return;

			//FeaturesListVM.UpdateCollection(null);
			MovementTypesListVM.UpdateCollection( selectedCharacter.MovementTypes_Speeds);
			LanguagesVM.UpdateCollection(selectedCharacter.Languages);
			ToolProfsVM.UpdateCollection(selectedCharacter.ToolProficiences);
			ArmorProfsVM.UpdateCollection(selectedCharacter.ArmorProficiencies);
			OtherProfsVM.UpdateCollection(selectedCharacter.OtherProficiences);
			WeaponProfsVM.UpdateCollection(selectedCharacter.WeaponProficiencies);

			Race = selectedCharacter.Race.RaceVariant.Name;

			var temp = selectedCharacter.Health;
			Health = temp.CurrHealth.ToString() + '/' + temp.MaxHealth + " (" + temp.TempHitPoints + " temp)";

			var characterClass = selectedCharacter.CharacterClass;
			CharacterClass = characterClass.Name + " " + characterClass.Level.Level;

			ArmorClass = selectedCharacter.ArmorClass.ArmorClassValue;

			HitDie = selectedCharacter.CharacterClass.HitDie.ToString();

			UpdateFeatures(null, null);
			SelectedProperty = AllFeatures.FirstOrDefault();
		}

		private void EditArmorClass()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel dataContext = new DialogWindowStringInputViewModel(window);
			window.DataContext = dataContext;

			dataContext.Answer = selectedCharacter.ArmorClass.ArmorClassValue;

			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			

			selectedCharacter.ArmorClass.ArmorClassValue = dataContext.Answer;
			ArmorClass = selectedCharacter.ArmorClass.ArmorClassValue;
		}

		private void AdjustExperience()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel dataContext = new DialogWindowStringInputViewModel(window);
			window.DataContext = dataContext;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			int temp = 0;
			try
			{
				temp = int.Parse(dataContext.Answer);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Must be a whole number", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
				AdjustExperience();
				return;
			}

			selectedCharacter.Level.ExperiencePoints += temp;
			
			// NOTE: check if they can level up, if they can, ask if they want to. 
		}

		private void AddHealth()
		{
			Window window = new ChangeHealthDialogWindow();
			DialogWindowChangeHealthViewModel dataContext = new DialogWindowChangeHealthViewModel(window);
			window.DataContext = dataContext;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			var temp = selectedCharacter.Health;

			if (dataContext.IsTempHealth)
			{
				temp.TempHitPoints += dataContext.Amount;
				temp.TempHitPoints = Math.Clamp(temp.TempHitPoints, 0, 1000000);
			}
			else
			{
				temp.CurrHealth += dataContext.Amount;
				temp.CurrHealth = Math.Clamp(temp.CurrHealth, 0, temp.MaxHealth);
			}

			Health = temp.CurrHealth.ToString() + '/' + temp.MaxHealth.ToString() + " (" + temp.TempHitPoints + " temp)";
		}

		private void AddFeature()
		{
			Window window = new AddFeatureDialogWindow();
			window.DataContext = new DialogWindowAddFeatureViewModel(window, this);
			window.ShowDialog();

			if (window.DialogResult == false)
				return;
			
			FeatureTypeSortCommand?.Execute(null);
		}

		private void RemoveFeature()
		{
			if (selectedProperty == null || selectedCharacter == null)
				return;

			Feature feature = selectedProperty;
			AllFeatures.Remove(selectedProperty);

			if (feature.FeatureType == selectedCharacter.CharacterClass.Name)
			{
				// does not handle same name items
				var toRemove = selectedCharacter.CharacterClass.Features.Where(x => x.Name == feature.Name).First();
				selectedCharacter.CharacterClass.Features.Remove(toRemove);
			}
			else if (feature.FeatureType == selectedCharacter.Race.Name)
			{
				// does not handle same name items
				var toRemove = selectedCharacter.Race.Features.Where(x => x.Name == feature.Name).First();
				selectedCharacter.Race.Features.Remove(toRemove);
			}
			else if (feature.FeatureType == selectedCharacter.Race.RaceVariant.Name)
			{
				// does not handle same name items
				var toRemove = selectedCharacter.Race.RaceVariant.Properties.Where(x => x.Name == feature.Name).First();
				selectedCharacter.Race.RaceVariant.Properties.Remove(toRemove);
			}

			selectedProperty = AllFeatures.FirstOrDefault();
		}

		private void UpdateFeatures(object? sender, NotifyCollectionChangedEventArgs e)
		{
			AllFeatures.Clear();

			if (selectedCharacter is null)
				return;

			foreach (var item in selectedCharacter.CharacterClass.Features)
			{
				AllFeatures.Add(new Feature(item, selectedCharacter.CharacterClass.Name, item.Level.ToString()));
			}

			foreach (var item in selectedCharacter.Race.Features)
			{
				AllFeatures.Add(new Feature(item, selectedCharacter.Race.Name, "-"));
			}

			foreach (var item in selectedCharacter.Race.RaceVariant.Properties)
			{
				AllFeatures.Add(new Feature(item, selectedCharacter.Race.RaceVariant.Name, "-"));
			}

			FeaturesCollectionView?.Refresh();
		}

		private void EditCharacter()
		{
			if (characterStore.SelectedCharacter == null)
				return;

			Window window = new EditCharacterDialogWindow();
			DialogWindowEditCharacterViewModel windowVM = new(window, characterStore.SelectedCharacter);
			window.DataContext = windowVM;

			window.ShowDialog();
		}
	}
}
