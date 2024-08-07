﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using PCCharacterManager.Models.DarkSouls;
using System.IO;

namespace PCCharacterManager.ViewModels.CharacterCreatorViewModels
{
	public class DarkSoulsCharacterCreatorViewModel : CharactorCreatorViewModelBase, INotifyDataErrorInfo
	{
		private readonly DialogServiceBase _dialogService;

		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				OnPropertyChanged(ref _name, value);
				BasicStringFieldValidation(nameof(Name), value);
			}
		}

		private Ability[] _abilities;
		public Ability[] Abilities
		{
			get { return _abilities; }
			set { _abilities = value; }
		}

		private int _maxHealth;
		public int MaxHealth
		{
			get
			{
				return _maxHealth;
			}
			set
			{
				OnPropertyChanged(ref _maxHealth, value);
			}
		}

		private ListViewMultiSelectItemsLimitedCountViewModel _selectedClassSkillProfs;
		public ListViewMultiSelectItemsLimitedCountViewModel SelectedClassSkillProfs
		{
			get { return _selectedClassSkillProfs; }
			set { OnPropertyChanged(ref _selectedClassSkillProfs, value); }
		}

		private DnD5eCharacterClassData _selectedCharacterClass;
		public DnD5eCharacterClassData SelectedCharacterClass
		{
			get { return _selectedCharacterClass; }
			set
			{
				OnPropertyChanged(ref _selectedCharacterClass, value);
				SelectedClassSkillProfs.PopulateItems(_selectedCharacterClass.NumOfSkillProficiences, _selectedCharacterClass.PossibleSkillProficiences.ToList());
				UpdateSelectClassSkillProfs();
				UpdateSelectedClassStartEquipment();
			}
		}

		private DarkSoulsOrigin _selectedOrigin;
		public DarkSoulsOrigin SelectedOrigin
		{
			get { return _selectedOrigin; }
			set
			{
				OnPropertyChanged(ref _selectedOrigin, value);
				UpdateAbilities();
			}
		}

		private bool _isValid;
		public bool IsValid
		{
			get
			{
				return _isValid;
			}
			set
			{
				OnPropertyChanged(ref _isValid, value);
			}
		}

		public List<DarkSoulsOrigin> OriginsToDisplay { get; private set; }
		public List<DnD5eCharacterClassData> CharacterClassesToDisplay { get; private set; }

		public ObservableCollection<ListViewMultiSelectItemsLimitedCountViewModel> SelectedStartingEquipmentVMs { get; private set; }

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public ICommand RollForHealthCommand { get; }

		public DarkSoulsCharacterCreatorViewModel(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;

			propertyNameToError = new Dictionary<string, List<string>>();

			_abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DarkSoulsResources.AbilitiesJson).ToArray();
			OriginsToDisplay = ReadWriteJsonCollection<DarkSoulsOrigin>.ReadCollection(DarkSoulsResources.OriginsDataJson);
			CharacterClassesToDisplay = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DarkSoulsResources.CharacterClassDataJson);

			RollForHealthCommand = new RelayCommand(RollForHealth);

			_name = string.Empty;
			_selectedCharacterClass = CharacterClassesToDisplay[0];
			_selectedOrigin = OriginsToDisplay[0];
			_selectedClassSkillProfs = new ListViewMultiSelectItemsLimitedCountViewModel(_selectedCharacterClass.NumOfSkillProficiences,
				_selectedCharacterClass.PossibleSkillProficiences.ToList());

			SelectedStartingEquipmentVMs = new ObservableCollection<ListViewMultiSelectItemsLimitedCountViewModel>();

			BasicStringFieldValidation(nameof(Name), Name);
			UpdateSelectedClassStartEquipment();
			UpdateAbilities();
		}

		private void RollForHealth()
		{
			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(_selectedOrigin.HitDie, 1);
			int currHealth = MaxHealth;
			int conMod = Abilities.First(x => x.Name.ToLower().Equals("constitution")).Modifier;

			MaxHealth = currHealth + numToAddToHealth + conMod + 2;
		}

		/// <summary>
		/// builds a new character with inputted data
		/// </summary>
		/// <returns>new character that was created</returns>
		public override CharacterBase Create()
		{
			var newCharacter = new DarkSoulsCharacter(SelectedCharacterClass, SelectedOrigin, _abilities);
			newCharacter.Name = Name;
			newCharacter.Level.ProficiencyBonus = 2;
			newCharacter.CharacterClass.HitDie = _selectedOrigin.HitDie;

			newCharacter.Inventory.AddRange(GetStartEquipment());
			newCharacter.Health.SetMaxHealth(_maxHealth);
			newCharacter.Health.CurrHealth = _maxHealth;

			SetClassSavingThrows(newCharacter);
			SetSelectedClassSkillProfs(newCharacter);


			AddFirstLevelClassFeatures(newCharacter);

			// there is a issue when the skill scores are not setting properly unless this is done
			foreach (Ability ability in newCharacter.Abilities)
			{
				int temp = ability.Score;
				ability.Score = 1;
				ability.Score = temp;
			}

			newCharacter.Id = CharacterIDGenerator.GenerateID();

			newCharacter.DateModified = DateTime.Now.ToString();

			if (SelectedCharacterClass.Note.Title != string.Empty)
			{
				newCharacter.NoteManager.NoteSections[0].Notes.Add(SelectedCharacterClass.Note);
			}

			newCharacter.CharacterClass.Name += " 1";

			string[] startingItems = StringFormater.CreateGroup(_selectedCharacterClass.StartEquipment[0], '&');

			foreach (string item in startingItems)
			{
				Item itemToAdd = new();
				itemToAdd.Name = item;
				newCharacter.Inventory.Add(itemToAdd);
			}

			newCharacter.Initiative = newCharacter.Abilities.First(x => x.Name == "Dexterity").Modifier;

			var race = new DnD5eCharacterRace();
			race.Name = "Unkindled";

			newCharacter.Race = race;

			return newCharacter;
		}


		private void UpdateAbilities()
		{
			for (int i = 0; i < Abilities.Length; i++)
			{
				Abilities[i].Score = StringFormater.FindQuantity(_selectedOrigin.BaseStatistics[i]);
			}
		}

		private void AddFirstLevelClassFeatures(DarkSoulsCharacter newCharacter)
		{
			foreach (var item in _selectedCharacterClass.Features)
			{
				if (item.Level == 1)
				{
					newCharacter.CharacterClass.Features.Add(item);
					continue;
				}

				return;
			}
		}

		/// <summary>
		/// set the characters skill proficiencies, based on the class selected
		/// </summary>
		private void SetSelectedClassSkillProfs(DarkSoulsCharacter newCharacter)
		{
			// class selected skill profs
			foreach (var item in _selectedClassSkillProfs.SelectedItems)
			{
				AbilitySkill s = Ability.FindSkill(newCharacter.Abilities, item);
				s.SkillProficiency = true;
				Ability a = Ability.FindAbility(newCharacter.Abilities, s);
				a.SetProfBonus(2);
			}
		}

		/// <summary>
		/// Set the characters saving throws, based on the chosen class
		/// </summary>
		private void SetClassSavingThrows(DarkSoulsCharacter newCharacter)
		{
			// set ability saving throws, Class
			foreach (var str in _selectedCharacterClass.SavingThrows)
			{
				Ability.FindAbility(newCharacter.Abilities, str).ProfSave = true;
			}
		}

		private List<Item> GetStartEquipment()
		{
			List<Item> results = new();
			List<Item> parsedItems = ReadWriteJsonCollection<Item>.ReadCollection(DnD5eResources.AllItemsJson);

			// iterate over all SelectedStartingEquipmentVMs
			foreach (ListViewMultiSelectItemsLimitedCountViewModel viewModel in SelectedStartingEquipmentVMs)
			{
				// iterate over all selected items
				foreach (string selectedItemName in viewModel.SelectedItems)
				{
					string[] itemNames = StringFormater.CreateGroup(selectedItemName, StringConstants.AND);
					int[] quantities = new int[itemNames.Length];

					for (int i = 0; i < itemNames.Length; i++)
					{
						quantities[i] = Convert.ToInt32(StringFormater.FindQuantity(itemNames[i]));
						itemNames[i] = StringFormater.RemoveQuantity(itemNames[i]);
					}

					// iterate over all items that are part of a selected item
					for (int i = 0; i < itemNames.Length; i++)
					{
						// find the Item obj w/ the matching name
						foreach (Item item in parsedItems)
						{
							if (itemNames[i].ToLower().Equals(item.Name.ToLower()))
							{
								item.Quantity = quantities[i];
								results.Add(item);
								break;
							}
						} // end for each
					} // end for
				} // end for each 
			} // end for each

			return results;
		}

		private void UpdateSelectClassSkillProfs()
		{
			_selectedClassSkillProfs.PopulateItems(SelectedCharacterClass.NumOfSkillProficiences, SelectedCharacterClass.PossibleSkillProficiences.ToList());
		}

		/// <summary>
		/// Update the starting equipment options to be displayed
		/// </summary>
		private void UpdateSelectedClassStartEquipment()
		{
			// break each array item into a listViewMultiSelectItemsLimitedCountVM. Items are slip w/ string formatter
			SelectedStartingEquipmentVMs.Clear();
			foreach (var item in _selectedCharacterClass.StartEquipment)
			{
				string[] group = StringFormater.CreateGroup(item, StringConstants.OR);
				SelectedStartingEquipmentVMs.Add(new ListViewMultiSelectItemsLimitedCountViewModel(1, group.ToList()));
			}
		}

		public IEnumerable GetErrors(string? propertyName)
		{
			return propertyNameToError.GetValueOrDefault(propertyName, new List<string>());
		}

		private void BasicStringFieldValidation(string propertyName, string propertyValue)
		{
			propertyNameToError.Remove(propertyName);

			List<string> errors = new();
			propertyNameToError.Add(propertyName, errors);
			if (string.IsNullOrEmpty(propertyValue) || string.IsNullOrWhiteSpace(propertyValue))
			{
				propertyNameToError[propertyName].Add("Cannot be empty or white space");
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
			}

			if (char.IsWhiteSpace(propertyValue.FirstOrDefault()))
			{
				propertyNameToError[propertyName].Add("Cannot start with white space");
				ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
			}

			if (propertyNameToError[propertyName].Any() == false)
			{
				propertyNameToError.Remove(propertyName);
			}

			IsValid = !HasErrors;
		}

		public static DnD5eCharacter CreateRandomCharacter()
		{
			var characterClassData = ReadWriteJsonCollection<DnD5eCharacterClassData>
				.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray().GetRandom();

			var characterBackgroundData = ReadWriteJsonCollection<DnD5eBackgroundData>
				.ReadCollection(DnD5eResources.BackgroundDataJson).ToArray().GetRandom();

			var characterRaceData = ReadWriteJsonCollection<DnD5eCharacterRaceData>
				.ReadCollection(DnD5eResources.RaceDataJson).ToArray().GetRandom();
			var characterRaceVarient = characterRaceData.Variants.ToArray().GetRandom();
			
			var abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();

			characterRaceData.RaceVariant = characterRaceVarient;

			DnD5eCharacter character = new(characterClassData, characterRaceData, characterBackgroundData, abilities)
			{
				Name = "John Doe"
			};

			character.CharacterClass.Name += " 1";

			character.DateModified = DateTime.Now.ToString();

			// roll abilities
			RollDie rollDie = new();
			for (int i = 0; i < 6; i++)
			{
				character.Abilities[i].Score = rollDie.AbilityScoreRoll();
			}

			// TODO: deal with languages, starting equipment, etc.

			return character;
		}
	}
}
