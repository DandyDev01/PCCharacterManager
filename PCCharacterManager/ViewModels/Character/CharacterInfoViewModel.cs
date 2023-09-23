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

		public PropertyListViewModel MovementTypesListVM { get; protected set; }
		public PropertyListViewModel FeaturesListVM { get; protected set; }

		public StringListViewModel LanguagesVM { get; protected set; }
		public StringListViewModel ArmorProfsVM { get; protected set; }
		public StringListViewModel WeaponProfsVM { get; protected set; }
		public StringListViewModel ToolProfsVM { get; protected set; }
		public StringListViewModel OtherProfsVM { get; protected set; }

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

		public ICommand NameSortCommand { get; }
		public ICommand FeatureTypeSortCommand { get; }
		public ICommand LevelSortCommand { get; }
		public ICommand AddFeatureCommand { get; }
		public ICommand RemoveFeatureCommand { get; }

		public CharacterInfoViewModel(CharacterStore _characterStore)
		{
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

			if (selectedCharacter is null)
				return;

			selectedProperty = AllFeatures.FirstOrDefault();	

			FeaturesListVM = new PropertyListViewModel("Features", null);
			MovementTypesListVM = new PropertyListViewModel("Movement", selectedCharacter.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", selectedCharacter.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", selectedCharacter.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", selectedCharacter.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", selectedCharacter.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", selectedCharacter.WeaponProficiencies);
			OnPropertyChanged(nameof(MovementTypesListVM));
			OnPropertyChanged(nameof(LanguagesVM));
			OnPropertyChanged(nameof(ArmorProfsVM));
			OnPropertyChanged(nameof(WeaponProfsVM));
			OnPropertyChanged(nameof(ToolProfsVM));
			OnPropertyChanged(nameof(OtherProfsVM));

			SelectedProperty = AllFeatures[0];
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

			FeaturesListVM = new PropertyListViewModel("Features", null);
			MovementTypesListVM = new PropertyListViewModel("Movement", selectedCharacter.MovementTypes_Speeds);
			LanguagesVM = new StringListViewModel("Languages", selectedCharacter.Languages);
			ToolProfsVM = new StringListViewModel("Tool Profs", selectedCharacter.ToolProficiences);
			ArmorProfsVM = new StringListViewModel("Armor Profs", selectedCharacter.ArmorProficiencies);
			OtherProfsVM = new StringListViewModel("Other Profs", selectedCharacter.OtherProficiences);
			WeaponProfsVM = new StringListViewModel("Weapon Profs", selectedCharacter.WeaponProficiencies);
			OnPropertyChanged(nameof(MovementTypesListVM));
			OnPropertyChanged(nameof(LanguagesVM));
			OnPropertyChanged(nameof(ArmorProfsVM));
			OnPropertyChanged(nameof(WeaponProfsVM));
			OnPropertyChanged(nameof(ToolProfsVM));
			OnPropertyChanged(nameof(OtherProfsVM));

			UpdateFeatures(null, null);
			SelectedProperty = AllFeatures.FirstOrDefault();
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
	}
}
