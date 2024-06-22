using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using PCCharacterManager.Services;

namespace PCCharacterManager.ViewModels.Character
{
	public class CharacterAbilitiesViewModel : ObservableObject
	{
		private readonly CollectionViewPropertySort _abilitiesCollectionViewPropertySort;
		private readonly CollectionViewPropertySort _skillsCollectionViewPropertySort;
		private readonly DialogServiceBase _dialogService;
		
		private CharacterBase _selectedCharacter;
		public CharacterBase SelectedCharacter
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

		private Ability _selectedAbility;
		public Ability SelectedAbility
		{
			get
			{
				return _selectedAbility;
			}
			set
			{
				OnPropertyChanged(ref _selectedAbility, value);
			}
		}

		public ObservableCollection<Ability> Abilities { get; }
		public ICollectionView AbilitiesCollectionView { get; }

		public ObservableCollection<AbilitySkill> Skills { get; }
		public ICollectionView SkillsCollectionView { get; }

		public ICommand EditSelectedAbilityCommand { get; }

		public ICommand AbilityNameSortCommand { get; }
		public ICommand AbilityScoreSortCommand { get; }
		public ICommand AbiltiyModifierSortCommand { get; }
		public ICommand AbilitySaveSortCommand { get; }
		public ICommand AbilityProficiencySortCommand { get; }

		public ICommand SkillProficiencySortCommand { get; }
		public ICommand SkillNameSortCommand { get; }
		public ICommand SkillScoreSortCommand { get; }
		public ICommand SkillAbilitySortCommand { get; }

		public CharacterAbilitiesViewModel(CharacterStore characterStore, DialogServiceBase dialogService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_dialogService = dialogService;
			_selectedCharacter = characterStore.SelectedCharacter;


			Abilities = new ObservableCollection<Ability>();
			AbilitiesCollectionView = CollectionViewSource.GetDefaultView(Abilities);
			_abilitiesCollectionViewPropertySort = new CollectionViewPropertySort(AbilitiesCollectionView);

			Skills = new ObservableCollection<AbilitySkill>();
			SkillsCollectionView = CollectionViewSource.GetDefaultView(Skills);
			_skillsCollectionViewPropertySort = new CollectionViewPropertySort(SkillsCollectionView);

			EditSelectedAbilityCommand = new RelayCommand(EditSelectedAbility);

			AbilityNameSortCommand = new ItemCollectionViewPropertySortCommand(_abilitiesCollectionViewPropertySort,
				nameof(Ability.Name));
			AbilityScoreSortCommand = new ItemCollectionViewPropertySortCommand(_abilitiesCollectionViewPropertySort,
				nameof(Ability.Score));
			AbiltiyModifierSortCommand = new ItemCollectionViewPropertySortCommand(_abilitiesCollectionViewPropertySort,
				nameof(Ability.Modifier));
			AbilitySaveSortCommand = new ItemCollectionViewPropertySortCommand(_abilitiesCollectionViewPropertySort,
				nameof(Ability.Save));
			AbilityProficiencySortCommand = new ItemCollectionViewPropertySortCommand(_abilitiesCollectionViewPropertySort,
				nameof(Ability.ProfSave));

			SkillProficiencySortCommand = new ItemCollectionViewPropertySortCommand(_skillsCollectionViewPropertySort,
				nameof(AbilitySkill.SkillProficiency));
			SkillNameSortCommand = new ItemCollectionViewPropertySortCommand(_skillsCollectionViewPropertySort,
				nameof(AbilitySkill.Name));
			SkillScoreSortCommand = new ItemCollectionViewPropertySortCommand(_skillsCollectionViewPropertySort,
				nameof(AbilitySkill.Score));
			SkillAbilitySortCommand = new ItemCollectionViewPropertySortCommand(_skillsCollectionViewPropertySort,
				nameof(AbilitySkill.AbilityName));
		}

		private void EditSelectedAbility()
		{
			DialogWindowStringInputViewModel dataContext = new("Please enter a whole number between the values 1 and 30.");

			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			int intValue = -1;
			try
			{
				intValue = int.Parse(dataContext.Answer);
			}
			catch
			{
				_dialogService.ShowMessage("Could not parse input to whole number", 
					"Input error. input must be a whole number", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (intValue > 30 || intValue < 1)
			{
				_dialogService.ShowMessage("Input must be between the values 1 and 30", "Invalid input",
					MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			SelectedAbility.Score = intValue;
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(CharacterBase newCharacter)
		{
			SelectedCharacter = newCharacter;

			if (_selectedCharacter is null)
				return;


			UpdateAbilitiesAndSkills(null, null);
		}

		private void UpdateAbilitiesAndSkills(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			Abilities.Clear();
			Skills.Clear();

			if (_selectedCharacter is null)
				return;

			Ability[] abilities = Array.Empty<Ability>();

			if (_selectedCharacter is DnD5eCharacter dnd)
			{
				abilities = dnd.Abilities;
			}
			else if (_selectedCharacter is DarkSoulsCharacter dark)
			{
				abilities = dark.Abilities;
			}
			else
			{
				return;
			}

			foreach (Ability ability in abilities)
			{
				Abilities.Add(ability);
			}

			foreach(Ability ability in Abilities)
			{
				foreach(AbilitySkill skill in ability.Skills)
				{
					Skills.Add(skill);
					skill.AbilityName = ability.Name;
				}
			}

			SkillsCollectionView?.Refresh();
			AbilitiesCollectionView?.Refresh();
		}
	}
}
