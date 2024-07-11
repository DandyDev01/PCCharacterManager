using PCCharacterManager.DialogWindows;
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
using System.Windows.Navigation;

namespace PCCharacterManager.ViewModels
{
	/// <summary>
	/// resbolsible for the logic of creating a new DnD5e character
	/// </summary>
	public class CharacterCreatorViewModel : CharactorCreatorViewModelBase, INotifyDataErrorInfo
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

		private DnD5eCharacterRaceVariant _selectedRaceVariant;
		public DnD5eCharacterRaceVariant SelectedRaceVariant
		{
			get { return _selectedRaceVariant; }
			set
			{
				OnPropertyChanged(ref _selectedRaceVariant, value);
				SelectedRace.RaceVariant = value;
			}
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
		
		private DnD5eCharacterRaceData _selectedRace;
		public DnD5eCharacterRaceData SelectedRace
		{
			get { return _selectedRace; }
			set
			{
				OnPropertyChanged(ref _selectedRace, value);
				UpdateRaceVariantsToDisplay();
			}
		}
		
		private DnD5eBackgroundData _selectedBackground;
		public DnD5eBackgroundData SelectedBackground
		{
			get { return _selectedBackground; }
			set { OnPropertyChanged(ref _selectedBackground, value); }
		}

		private Alignment _selectedAlignment;
		public Alignment SelectedAlignment
		{
			get { return _selectedAlignment; }
			set { OnPropertyChanged(ref _selectedAlignment, value); }
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

		public Array AlignmentsToDisplay { get; private set; }
		public List<DnD5eCharacterRaceData> RacesToDisplay { get; private set; }
		public List<DnD5eBackgroundData> BackgroundsToDisplay { get; private set; }
		public List<DnD5eCharacterClassData> CharacterClassesToDisplay { get; private set; }

		public ObservableCollection<ListViewMultiSelectItemsLimitedCountViewModel> SelectedStartingEquipmentVMs { get; private set; }
		public ObservableCollection<DnD5eCharacterRaceVariant> RaceVariantsToDisplay { get; private set; }
		public ObservableCollection<int> AbilityScores { get; private set; }
		
		private readonly List<string> notAnOption;

		public ICommand RollAbilityScoresCommand { get; }
		public ICommand RollForHealthCommand { get; }

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public CharacterCreatorViewModel(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;

			propertyNameToError = new Dictionary<string, List<string>>();

			_abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();
			BackgroundsToDisplay = ReadWriteJsonCollection<DnD5eBackgroundData>.ReadCollection(DnD5eResources.BackgroundDataJson);
			CharacterClassesToDisplay = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);
			RacesToDisplay = ReadWriteJsonCollection<DnD5eCharacterRaceData>.ReadCollection(DnD5eResources.RaceDataJson);
			RaceVariantsToDisplay = new ObservableCollection<DnD5eCharacterRaceVariant>();
			AlignmentsToDisplay = Enum.GetValues(typeof(Alignment));

			_name = string.Empty;
			_selectedCharacterClass = CharacterClassesToDisplay[0];
			_selectedBackground = BackgroundsToDisplay[0];
			_selectedRace = RacesToDisplay[0];
			_selectedRaceVariant = _selectedRace.Variants[0];
			_selectedClassSkillProfs = new ListViewMultiSelectItemsLimitedCountViewModel(_selectedCharacterClass.NumOfSkillProficiences,
				_selectedCharacterClass.PossibleSkillProficiences.ToList());
			notAnOption = new List<string>();
			RaceVariantsToDisplay.AddRange(_selectedRace.Variants);

			SelectedStartingEquipmentVMs = new ObservableCollection<ListViewMultiSelectItemsLimitedCountViewModel>();
			AbilityScores = new ObservableCollection<int>(RollDie.DefaultAbilityScores);

			RollAbilityScoresCommand = new RelayCommand(AbilityRoll);
			RollForHealthCommand = new RelayCommand(RollForHealth);

			BasicStringFieldValidation(nameof(Name), Name);
			UpdateSelectedClassStartEquipment();
		}
		
		/// <summary>
		/// builds a new character with inputted data
		/// </summary>
		/// <returns>new character that was created</returns>
		public override DnD5eCharacter Create()
		{
			var newCharacter = new DnD5eCharacter(SelectedCharacterClass, SelectedRace, SelectedBackground, _abilities);
			newCharacter.Name = Name;
			newCharacter.Level.ProficiencyBonus = 2;

			newCharacter.Inventory.AddRange(GetStartEquipment());

			newCharacter.Health.SetMaxHealth(MaxHealth);
			newCharacter.Health.CurrHealth = MaxHealth;

			SetClassSavingThrows(newCharacter);
			SetSelectedClassSkillProfs(newCharacter);

			if (SetBackgroundSkillProfs(newCharacter) == false)
				throw new Exception("Background Skill Profs Error.");

			if (SetOtherBackgroundProfs(newCharacter) == false)
				throw new Exception("Background Other Profs Error.");

			// add class tool profs to character tool profs
			// NOTE: does not handle duplicates from background
			newCharacter.ToolProficiences.AddRange(_selectedCharacterClass.ToolProficiencies);

			// add languages from background
			// NOTE: Add support for exotic languages
			foreach (string languageName in _selectedBackground.Languages)
			{
				if (languageName.Contains("your choice", StringComparison.OrdinalIgnoreCase))
				{
					if (BackgroundChooseLanguage(newCharacter, languageName) == false)
						throw new Exception("Background Choose Languages Error.");
				}
			}

			RaceAbilityScoreIncreasesUserChoice(newCharacter);
			RaceVariantAbilityScoreIncreases(newCharacter);
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

			return newCharacter;
		}

		private void RollForHealth()
		{
			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(_selectedCharacterClass.HitDie, 1);
			int currHealth = MaxHealth;
			int conMod = Abilities.First(x => x.Name.ToLower().Equals("constitution")).Modifier;

			MaxHealth = currHealth + numToAddToHealth + conMod + 2;
		}

		private void AddFirstLevelClassFeatures(DnD5eCharacter newCharacter)
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

		private bool SetOtherBackgroundProfs(DnD5eCharacter newCharacter)
		{
			// handle other profs for the background
			// NOTE: does not handle &
			foreach (var item in _selectedBackground.OtherProfs)
			{
				if (item.ToLower().Contains("tool", StringComparison.OrdinalIgnoreCase))
				{
					if (item.Contains(StringConstants.OR))
					{
						var options = StringFormater.CreateGroup(item, StringConstants.OR);
						
						DialogWindowSelectStingValueViewModel windowVM = new(options.ToArray(), 1);

						string result = string.Empty;
						_dialogService.ShowDialog<SelectStringValueDialogWindow, 
							DialogWindowSelectStingValueViewModel>(windowVM, r =>
						{
							result = r;
						});

						if (result == false.ToString())
							return false;

						newCharacter.ToolProficiences.Add(windowVM.SelectedItems.First());
						continue;
					}

					newCharacter.ToolProficiences.Add(item);
					continue;
				}

				newCharacter.OtherProficiences.Add(item);
			}

			return true;
		}

		private bool SetBackgroundSkillProfs(DnD5eCharacter newCharacter)
		{
			// set skill prof, Background
			notAnOption.Clear();
			notAnOption.AddRange(SelectedClassSkillProfs.SelectedItems);
			foreach (string skillName in _selectedBackground.SkillProfs)
			{
				// you can choose one of at least 2
				if (skillName.Contains(StringConstants.OR))
				{
					return ChooseSkillToHaveProficiencyInFromBackground(newCharacter,skillName);
				}
				else if (skillName.Contains("your choice", StringComparison.OrdinalIgnoreCase))
				{
					return BackgroundChooseSkillYourChoice(newCharacter);

				}
				// class give prof to skill
				if (_selectedClassSkillProfs.SelectedItems.Contains(skillName))
				{
					_dialogService.ShowMessage("class and background both give skill prof to " + skillName +
						" please select a different skill to have prof in", "cannot double prof in skill",
						MessageBoxButton.OK, MessageBoxImage.Information);

					return BackgroundChooseSkillYourChoice(newCharacter);
				}
				else // class does not give prof to skill
				{
					AbilitySkill s = Ability.FindSkill(newCharacter.Abilities, skillName);
					Ability a = Ability.FindAbility(newCharacter.Abilities, s);
					s.SkillProficiency = true;
					a.SetProfBonus(2);

					notAnOption.Add(skillName);
				}
			} // end for

			return true;
		}

		private void RaceVariantAbilityScoreIncreases(DnD5eCharacter newCharacter)
		{
			// ability score increases, race variant
			foreach (var property in _selectedRaceVariant.Properties)
			{
				if (property.Name.ToLower().Contains("ability score increase"))
				{
					string abilityName = StringFormater.Get1stWord(property.Desc);
					int increaseAmount = StringFormater.FindQuantity(property.Desc);

					Ability a = Ability.FindAbility(newCharacter.Abilities, abilityName);
					a.Score += increaseAmount;
				}
			}
		}

		private void RaceAbilityScoreIncreasesUserChoice(DnD5eCharacter newCharacter)
		{
			// handle race ability score increase 'You Choice'
			for (int i = 0; i < _selectedRace.AbilityScoreIncreases.Length; i++)
			{
				// 'You Choice'
				if (_selectedRace.AbilityScoreIncreases[i].Contains("your choice", StringComparison.OrdinalIgnoreCase))
				{
					int increaseAmount = StringFormater.FindQuantity(_selectedRace.AbilityScoreIncreases[i]);
					
					DialogWindowSelectStingValueViewModel windowVM =new(Ability.GetAbilityNames(newCharacter.Abilities).ToArray(), 1);

					string result = string.Empty;
					_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(windowVM, r =>
					{
						result = r;
					});

					if (result == false.ToString())
						return;

					foreach (var item in windowVM.SelectedItems)
					{
						Ability a = Ability.FindAbility(newCharacter.Abilities, item);
						a.Score += increaseAmount;
					}
				}
				// default
				else
				{
					string abilityName = StringFormater.Get1stWord(_selectedRace.AbilityScoreIncreases[i]);
					int increaseAmount = StringFormater.FindQuantity(_selectedRace.AbilityScoreIncreases[i]);

					Ability a = Ability.FindAbility(newCharacter.Abilities, abilityName);
					a.Score += increaseAmount;
				}
			}
		}

		/// <summary>
		/// choose languages the character will know, based on the background selected
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private bool BackgroundChooseLanguage(DnD5eCharacter newCharacter, string str)
		{
			int amount = StringFormater.FindQuantity(str);

			var languages = ReadWriteJsonCollection<string>.ReadCollection(DnD5eResources.LanguagesJson);
			List<string> options = new();
			foreach (var language in languages)
			{
				if (newCharacter.Languages.Contains(language) == false)
					options.Add(language);
			}

			DialogWindowSelectStingValueViewModel windowVM = new(options.ToArray(), amount);

			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return false;

			newCharacter.AddLanguages(windowVM.SelectedItems.ToArray());

			return true;
		}

		/// <summary>
		/// set the characters skill proficiencies, based on the class selected
		/// </summary>
		private void SetSelectedClassSkillProfs(DnD5eCharacter newCharacter)
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
		private void SetClassSavingThrows(DnD5eCharacter newCharacter)
		{
			// set ability saving throws, Class
			foreach (var str in _selectedCharacterClass.SavingThrows)
			{
				Ability.FindAbility(newCharacter.Abilities, str).ProfSave = true;
			}
		}

		/// <summary>
		/// Choose a skill to have proficiency in. Choices are determined by the selected background
		/// </summary>
		/// <param name="skillName"></param>
		/// <returns></returns>
		private bool ChooseSkillToHaveProficiencyInFromBackground(DnD5eCharacter newCharacter, string skillName)
		{
			List<string> selectedSkills = new();
			List<string> options = StringFormater.CreateGroup(skillName, StringConstants.OR).ToList();

			// removes options that class give prof in
			foreach (string item in notAnOption)
			{
				options.Remove(item);
			}

			// all options are chosen. Can happen with Rouge & Urban Bounty Hunter
			if (options.Count <= 0)
			{
				if (!BackgroundChooseSkillYourChoice(newCharacter))
					return false;
			}
			else
			{
				DialogWindowSelectStingValueViewModel windowVM = new(options.ToArray(), 1);

				string result = string.Empty;
				_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(windowVM, r =>
				{
					result = r;
				});

				if (result == false.ToString())
					return false;

				foreach (string item in windowVM.SelectedItems)
				{
					AbilitySkill skill = Ability.FindSkill(newCharacter.Abilities, item);
					Ability ability = Ability.FindAbility(newCharacter.Abilities, skill);

					skill.SkillProficiency = true;
					ability.SetProfBonus(2);

					notAnOption.Add(item);
					selectedSkills.Add(item);
				}
			}

			return true;
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

		/// <summary>
		/// update the race variants to be displayed in the view
		/// </summary>
		private void UpdateRaceVariantsToDisplay()
		{
			RaceVariantsToDisplay.Clear();
			
			RaceVariantsToDisplay.AddRange(_selectedRace.Variants);

			SelectedRaceVariant = RaceVariantsToDisplay[0];
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

		/// <summary>
		/// Randomly set the scores for the characters abilities
		/// </summary>
		private void AbilityRoll()
		{
			RollDie rollDie = new();

			for (int i = 0; i < 6; i++)
			{
				Abilities[i].Score = rollDie.AbilityScoreRoll();
			}
		}

		/// <summary>
		/// open a dialog window to choose a skill the character will 
		/// be proficient in
		/// </summary>
		/// <returns>wheather or not the user cancels or selects</returns>
		private bool BackgroundChooseSkillYourChoice(DnD5eCharacter newCharacter)
		{
			List<string> options = Ability.GetSkillNames();
			foreach (var item in notAnOption)
			{
				options.Remove(item);
			}

			DialogWindowSelectStingValueViewModel windowVM = new(options.ToArray(), 1);
		
			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
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
	
		public static DnD5eCharacter CreateRandonCharacter()
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
