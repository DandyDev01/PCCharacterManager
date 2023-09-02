﻿using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	/// <summary>
	/// resbolsible for the logic of creating a new DnD5e character
	/// </summary>
	public class CharacterCreatoreViewModel : CharactorCreatorViewModelBase, INotifyDataErrorInfo
	{
		private string name;
		public string Name
		{
			get { return name; }
			set 
			{ 
				OnPropertyChanged(ref name, value);
				BasicStringFieldValidation(nameof(Name), value);
			}
		}

		private ListViewMultiSelectItemsLimitedCountViewModel selectedClassSkillProfs;
		public ListViewMultiSelectItemsLimitedCountViewModel SelectedClassSkillProfs
		{
			get { return selectedClassSkillProfs; }
			set { OnPropertyChanged(ref selectedClassSkillProfs, value); }
		}

		private DnD5eCharacterRaceVariant selectedRaceVariant;
		public DnD5eCharacterRaceVariant SelectedRaceVariant
		{
			get { return selectedRaceVariant; }
			set
			{
				OnPropertyChanged(ref selectedRaceVariant, value);
				SelectedRace.RaceVariant = value;
			}
		}
		
		private DnD5eCharacterClassData selectedCharacterClass;
		public DnD5eCharacterClassData SelectedCharacterClass
		{
			get { return selectedCharacterClass; }
			set
			{
				OnPropertyChanged(ref selectedCharacterClass, value);
				SelectedClassSkillProfs.PopulateItems(selectedCharacterClass.NumOfSkillProficiences, selectedCharacterClass.PossibleSkillProficiences.ToList());
				UpdateSelectClassSkillProfs();
				UpdateSelectedClassStartEquipment();
			}
		}
		
		private DnD5eCharacterRaceData selectedRace;
		public DnD5eCharacterRaceData SelectedRace
		{
			get { return selectedRace; }
			set
			{
				OnPropertyChanged(ref selectedRace, value);
				UpdateRaceVariantsToDisplay();
			}
		}
		
		private DnD5eBackgroundData selectedBackground;
		public DnD5eBackgroundData SelectedBackground
		{
			get { return selectedBackground; }
			set { OnPropertyChanged(ref selectedBackground, value); }
		}

		private DnD5eCharacter newCharacter;
		public DnD5eCharacter NewCharacter
		{
			get { return newCharacter; }
			set { OnPropertyChanged(ref newCharacter, value); }
		}

		private Alignment selectedAlignment;
		public Alignment SelectedAlignment
		{
			get { return selectedAlignment; }
			set { OnPropertyChanged(ref selectedAlignment, value); }
		}

		private bool isValid;
		public bool IsValid
		{
			get
			{
				return isValid;
			}
			set
			{
				OnPropertyChanged(ref isValid, value);
			}
		}

		public Array AlignmentsToDisplay { get; private set; }
		public List<DnD5eCharacterRaceData> RacesToDisplay { get; private set; }
		public List<DnD5eBackgroundData> BackgroundsToDisplay { get; private set; }
		public List<DnD5eCharacterClassData> CharacterClassesToDisplay { get; private set; }

		public ObservableCollection<ListViewMultiSelectItemsLimitedCountViewModel> SelectedStartingEquipmentVMs { get; private set; }
		public ObservableCollection<DnD5eCharacterRaceVariant> RaceVariantsToDisplay { get; private set; }
		public ObservableCollection<int> AbilityScores { get; private set; }
		
		private List<string> notAnOption;

		public ICommand RollAbilityScoresCommand { get; private set; }

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public CharacterCreatoreViewModel()
		{
			newCharacter = new DnD5eCharacter();

			propertyNameToError = new Dictionary<string, List<string>>();

			BackgroundsToDisplay = ReadWriteJsonCollection<DnD5eBackgroundData>.ReadCollection(DnD5eResources.BackgroundDataJson);
			CharacterClassesToDisplay = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);
			RacesToDisplay = ReadWriteJsonCollection<DnD5eCharacterRaceData>.ReadCollection(DnD5eResources.RaceDataJson);
			RaceVariantsToDisplay = new ObservableCollection<DnD5eCharacterRaceVariant>();
			AlignmentsToDisplay = Enum.GetValues(typeof(Alignment));

			name = string.Empty;
			selectedCharacterClass = CharacterClassesToDisplay[0];
			selectedBackground = BackgroundsToDisplay[0];
			selectedRace = RacesToDisplay[0];
			selectedRaceVariant = selectedRace.Variants[0];
			selectedClassSkillProfs = new ListViewMultiSelectItemsLimitedCountViewModel(selectedCharacterClass.NumOfSkillProficiences,
				selectedCharacterClass.PossibleSkillProficiences.ToList());
			notAnOption = new List<string>();

			SelectedStartingEquipmentVMs = new ObservableCollection<ListViewMultiSelectItemsLimitedCountViewModel>();
			AbilityScores = new ObservableCollection<int>(RollDie.DefaultAbilityScores);

			RollAbilityScoresCommand = new RelayCommand(AbilityRoll);

			BasicStringFieldValidation(nameof(Name), Name);
		}
		
		/// <summary>
		/// builds a new character with inputed data
		/// </summary>
		/// <returns>new character that was created</returns>
		public override DnD5eCharacter Create()
		{
			DnD5eCharacter tempCharacter = newCharacter;
			newCharacter = new DnD5eCharacter(SelectedCharacterClass, SelectedRace, SelectedBackground);
			newCharacter.Name = Name;
			newCharacter.Abilities = tempCharacter.Abilities;
			newCharacter.Level.ProficiencyBonus = 2;
			List<Item> allItems = ReadWriteJsonCollection<Item>.ReadCollection(DnD5eResources.AllItemsJson);

			// iterate over all SelectedStartingEquipmentVMs
			foreach (var viewModel in SelectedStartingEquipmentVMs)
			{
				// iterate over all selcted items
				foreach (var str in viewModel.SelectedItems)
				{
					string[] items = StringFormater.CreateGroup(str, '&');
					int[] quantities = new int[items.Length];
					for (int i = 0; i < items.Length; i++)
					{
						quantities[i] = Convert.ToInt32(StringFormater.FindQuantity(items[i]));
						items[i] = StringFormater.RemoveQuantity(items[i]);
					}

					// iterate over all items that are part of a selected item
					for (int i = 0; i < items.Length; i++)
					{
						// find the Item obj w/ the matching name
						foreach (var item in allItems)
						{
							if (items[i].ToLower().Equals(item.Name.ToLower()))
							{
								item.Quantity = quantities[i];
								newCharacter.Inventory.Add(item);
							}
						} // end foreach
					} // end for
				} // end foreach 
			} // end foreach

			// set ability saveing throws, Class
			foreach (var str in selectedCharacterClass.SavingThrows)
			{
				Ability.FindAbility(newCharacter.Abilities, str).ProfSave = true;
			}

			// class selected skill profs
			foreach (var item in selectedClassSkillProfs.SelectedItems)
			{
				AbilitySkill s = Ability.FindSkill(newCharacter.Abilities, item);
				s.SkillProficiency = true;
				Ability a = Ability.FindAbility(newCharacter.Abilities, s);
				a.SetProfBonus(2);
			}

			// set skill prof, Background
			notAnOption.Clear();
			notAnOption.AddRange(SelectedClassSkillProfs.SelectedItems);
			foreach (var str in selectedBackground.SkillProfs)
			{
				// you can choose one of at least 2
				if (str.Contains("^"))
				{
					List<string> selectedSkills = new List<string>();

					// open dialog window to choose 1
					// removes options that class give prof in
					var options = StringFormater.CreateGroup(str, '^').ToList();
					foreach (var item in notAnOption)
					{
						options.Remove(item);
					}

					// all options are chosen. Can happen with Rouge & Urban BOunty Hunter
					if (options.Count <= 0)
					{
						if (!CreateHelper())
							return null;
					}
					else
					{
						Window window = new SelectStringValueDialogWindow();
						DialogWindowSelectStingValue windowVM =
							new DialogWindowSelectStingValue(window, options.ToArray(), 1);
						window.DataContext = windowVM;
						window.ShowDialog();

						if (window.DialogResult == false)
							return null;

						foreach (var item in windowVM.SelectedItems)
						{
							AbilitySkill s = Ability.FindSkill(newCharacter.Abilities, item);
							s.SkillProficiency = true;
							Ability a = Ability.FindAbility(newCharacter.Abilities, s);
							a.SetProfBonus(2);
							notAnOption.Add(item);
							selectedSkills.Add(item);
						}
					}
				}
				else if (str.Contains("your choice"))
				{
					if (!CreateHelper())
						return null;

				}
				else // DEFAULT
				{
					// class give prof to skill
					if (selectedClassSkillProfs.SelectedItems.Contains(str))
					{
						MessageBox.Show("class and background both give skill prof to " + str + 
							" please select a different skill to have prof in", "cannot double prof in skill",
							MessageBoxButton.OK, MessageBoxImage.Information);

						if (!CreateHelper())
							return null;
					}
					else // class does not give prof to skill
					{
						AbilitySkill s = Ability.FindSkill(newCharacter.Abilities, str);
						s.SkillProficiency = true;
						Ability a = Ability.FindAbility(newCharacter.Abilities, s);
						a.SetProfBonus(2);
						notAnOption.Add(str);
					}

				}
			} // end for

			// handle other profs for the background
			// NOTE: does not handle &
			foreach (var item in selectedBackground.OtherProfs)
			{
				if (item.ToLower().Contains("tool"))
				{

					if (item.Contains("^"))
					{
						var options = StringFormater.CreateGroup(item, '^');
						Window window = new SelectStringValueDialogWindow();
						DialogWindowSelectStingValue windowVM =
							new DialogWindowSelectStingValue(window, options.ToArray(), 1);
						window.DataContext = windowVM;
						window.ShowDialog();

						if (window.DialogResult == false)
							return null;

						newCharacter.ToolProficiences.Add(windowVM.SelectedItems.First());
						continue;
					}

					newCharacter.ToolProficiences.Add(item);
					continue;
				}

				newCharacter.OtherProficiences.Add(item);
			}

			// add class tool profs to character tool profs
			// NOTE: does not handle doups from backgorund
			foreach (var item in selectedCharacterClass.ToolProficiencies)
			{
				newCharacter.ToolProficiences.Add(item);
			}

			// add languages from background
			// NOTE: Add support for exotic languages
			foreach (var str in selectedBackground.Languages)
			{
				if (str.ToLower().Contains("your choice"))
				{
					int amount = StringFormater.FindQuantity(str);

					var languages = ReadWriteJsonCollection<string>.ReadCollection(DnD5eResources.LanguagesJson);
					List<string> options = new List<string>();
					foreach (var language in languages)
					{
						if (!newCharacter.Languages.Contains(language))
							options.Add(language);
					}

					Window window = new SelectStringValueDialogWindow();
					DialogWindowSelectStingValue windowVM =
						new DialogWindowSelectStingValue(window, options.ToArray(), amount);
					window.DataContext = windowVM;
					window.ShowDialog();

					if (window.DialogResult == false)
						return null;

					newCharacter.AddLanguages(windowVM.SelectedItems.ToArray());
				}
			}

			// handle race ability score increase 'You Choice'
			for (int i = 0; i < selectedRace.AbilityScoreIncreases.Length; i++)
			{
				// 'You Choice'
				if (selectedRace.AbilityScoreIncreases[i].ToLower().Contains("your choice"))
				{
					int increaseAmount = StringFormater.FindQuantity(selectedRace.AbilityScoreIncreases[i]);
					Window window = new SelectStringValueDialogWindow();
					DialogWindowSelectStingValue windowVM =
						new DialogWindowSelectStingValue(window, Ability.GetAbilityNames(newCharacter.Abilities).ToArray(), 1);
					window.DataContext = windowVM;
					window.ShowDialog();

					foreach (var item in windowVM.SelectedItems)
					{
						Ability a = Ability.FindAbility(newCharacter.Abilities, item);
						a.Score += increaseAmount;
					}
				}
				// default
				else
				{
					string abilityName = StringFormater.Get1stWord(selectedRace.AbilityScoreIncreases[i]);
					int increaseAmount = StringFormater.FindQuantity(selectedRace.AbilityScoreIncreases[i]);

					Ability a = Ability.FindAbility(newCharacter.Abilities, abilityName);
					a.Score += increaseAmount;
				}
			}

			// ability score inceases, race variant
			foreach (var property in selectedRaceVariant.Properties)
			{
				if (property.Name.ToLower().Contains("ability score increase"))
				{
					string abilityName = StringFormater.Get1stWord(property.Desc);
					int increaseAmount = StringFormater.FindQuantity(property.Desc);

					Ability a = Ability.FindAbility(newCharacter.Abilities, abilityName);
					a.Score += increaseAmount;
				}
			}

			foreach (var item in selectedCharacterClass.Features)
			{
				if (item.Level == 1)
				{
					newCharacter.CharacterClass.Features.Add(item);
				}
				else
					break;
			}

			newCharacter.DateModified = DateTime.Now.ToString();

			// there is a issue when the skill scores are not setting properly unless this is done
			foreach (var item in newCharacter.Abilities)
			{
				int temp = item.Score;
				item.Score = 1;
				item.Score = temp;
			}

			return newCharacter;
		}

		private void UpdateRaceVariantsToDisplay()
		{
			RaceVariantsToDisplay.Clear();
			foreach (var raceVariant in selectedRace.Variants)
			{
				RaceVariantsToDisplay.Add(raceVariant);
			}

			SelectedRaceVariant = RaceVariantsToDisplay[0];
		}

		private void UpdateSelectClassSkillProfs()
		{
			selectedClassSkillProfs.PopulateItems(SelectedCharacterClass.NumOfSkillProficiences, SelectedCharacterClass.PossibleSkillProficiences.ToList());
		}

		private void UpdateSelectedClassStartEquipment()
		{
			// break each array item into a listViewMultiSelectItemsLimitedCountVM. Items are slip w/ string formatter
			SelectedStartingEquipmentVMs.Clear();
			foreach (var item in selectedCharacterClass.StartEquipment)
			{
				string[] group = StringFormater.CreateGroup(item, '^');
				SelectedStartingEquipmentVMs.Add(new ListViewMultiSelectItemsLimitedCountViewModel(1, group.ToList()));
			}
		}

		private void AbilityRoll()
		{
			RollDie rollDie = new RollDie();

			for (int i = 0; i < 6; i++)
			{
				newCharacter.Abilities[i].Score = rollDie.AbilityScoreRoll();
				AbilityScores[i] = newCharacter.Abilities[i].Score;

				Console.WriteLine("DEBUG: " + newCharacter.Abilities[i].Name + ": " + newCharacter.Abilities[i].Score);
			}
		}

		/// <summary>
		/// open a dialog window to choose a skill the character will 
		/// be proficient in
		/// </summary>
		/// <returns>wheather or not the user cancels or selects</returns>
		private bool CreateHelper()
		{
			List<string> options = Ability.GetSkillNames();
			foreach (var item in notAnOption)
			{
				options.Remove(item);
			}

			Window window = new SelectStringValueDialogWindow();
			DialogWindowSelectStingValue windowVM =
				new DialogWindowSelectStingValue(window, options.ToArray(), 1);
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return false;

			foreach (var item in windowVM.SelectedItems)
			{
				AbilitySkill s = Ability.FindSkill(newCharacter.Abilities, item);
				s.SkillProficiency = true;
				Ability a = Ability.FindAbility(newCharacter.Abilities, s);
				a.SetProfBonus(2);
				notAnOption.Add(item);
			}

			return true;
		}

		public IEnumerable GetErrors(string? propertyName)
		{
			return propertyNameToError.GetValueOrDefault(propertyName, new List<string>());
		}

		private void BasicStringFieldValidation(string propertyName, string propertyValue)
		{
			propertyNameToError.Remove(propertyName);

			List<string> errors = new List<string>();
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
	}
}