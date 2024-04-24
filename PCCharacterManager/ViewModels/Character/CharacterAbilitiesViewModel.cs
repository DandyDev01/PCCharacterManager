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

namespace PCCharacterManager.ViewModels.Character
{
    public class CharacterAbilitiesViewModel : ObservableObject 
    {
		private readonly CollectionViewPropertySort abilitiesCollectionViewPropertySort;
		private readonly CollectionViewPropertySort skillsCollectionViewPropertySort;

		private DnD5eCharacter selectedCharacter;
		public DnD5eCharacter SelectedCharacter
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

		public ObservableCollection<Ability> Abilities { get; }
		public ICollectionView AbilitiesCollectionView { get; }

		public ObservableCollection<AbilitySkill> Skills { get; }
		public ICollectionView SkillsCollectionView { get; }

		public CharacterAbilitiesViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			selectedCharacter = _characterStore.SelectedCharacter;


			Abilities = new ObservableCollection<Ability>();
			AbilitiesCollectionView = CollectionViewSource.GetDefaultView(Abilities);
			abilitiesCollectionViewPropertySort = new CollectionViewPropertySort(AbilitiesCollectionView);

			Skills = new ObservableCollection<AbilitySkill>();
			SkillsCollectionView = CollectionViewSource.GetDefaultView(Skills);
			skillsCollectionViewPropertySort = new CollectionViewPropertySort(AbilitiesCollectionView);
		}

		/// <summary>
		/// What to do when the selectedCharacter changes
		/// </summary>
		/// <param name="newCharacter">the newly selected character</param>
		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			SelectedCharacter = newCharacter;

			if (selectedCharacter is null)
				return;


			UpdateAbilitiesAndSkills(null, null);
		}

		private void UpdateAbilitiesAndSkills(object? sender, NotifyCollectionChangedEventArgs? e)
		{
			Abilities.Clear();
			Skills.Clear();

			if (selectedCharacter is null)
				return;

			foreach (Ability ability in selectedCharacter.Abilities)
			{
				Abilities.Add(ability);
			}

			foreach(Ability ability in Abilities)
			{
				foreach(AbilitySkill skill in ability.Skills)
				{
					Skills.Add(skill);
				}
			}

			SkillsCollectionView?.Refresh();
			AbilitiesCollectionView?.Refresh();
		}
	}
}
