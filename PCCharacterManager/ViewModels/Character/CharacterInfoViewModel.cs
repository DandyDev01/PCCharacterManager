using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
		
		private Property? selectedProperty;
		public Property? SelectedProperty
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

			_characterStore.SelectedCharacterChange += OnCharacterChanged;

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
		}

		private void UpdateFeatures(object? sender, NotifyCollectionChangedEventArgs e)
		{
			AllFeatures.Clear();

			if (selectedCharacter is null) 
				return;

			foreach (var item in selectedCharacter.CharacterClass.Features)
			{
				AllFeatures.Add(new Feature(item.Name, item.Desc, selectedCharacter.CharacterClass.Name, item.Level.ToString()));
			}

			foreach (var item in selectedCharacter.Race.Features)
			{
				AllFeatures.Add(new Feature(item.Name, item.Desc, selectedCharacter.Race.Name, "-"));
			}

			foreach (var item in selectedCharacter.Race.RaceVariant.Properties)
			{
				AllFeatures.Add(new Feature(item.Name, item.Desc, selectedCharacter.Race.RaceVariant.Name, "-"));
			}

			FeaturesCollectionView?.Refresh();
		}
	}
}
