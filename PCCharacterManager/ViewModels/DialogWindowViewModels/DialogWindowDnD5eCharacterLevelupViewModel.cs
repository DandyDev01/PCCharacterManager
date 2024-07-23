using Accessibility;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	public class DialogWindowDnD5eCharacterLevelupViewModel : ObservableObject
	{
		private readonly DialogServiceBase _dialogService;
		private readonly DnD5eCharacter _character;

		private int _numberOfClassesAdded;

		private bool _hasAddedClass = false;
		public bool HasAddedClass => _hasAddedClass;

		private DnD5eCharacterClassData _selectedCharacterClass;
		public DnD5eCharacterClassData SelectedCharacterClass
		{
			get
			{
				return _selectedCharacterClass;
			}
			set
			{
				OnPropertyChanged(ref _selectedCharacterClass, value);
				PopulateFeaturesToDisplay();
				PopulateProfsToDisplay();
				MaxHealth = _character.Health.MaxHealth;
			}
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

		private string _characterName;
		public string CharacterName
		{
			get
			{
				return _characterName;
			}
			set
			{
				OnPropertyChanged(ref _characterName, value);
			}
		}

		public ObservableCollection<string> WeaponProfsToDisplay { get; }
		public ObservableCollection<string> ArmorProfsToDisplay { get; }
		public ObservableCollection<string> ToolProfsToDisplay { get; }
		public ObservableCollection<DnD5eCharacterClassFeature> FeaturesToDisplay { get; }
		public ObservableCollection<DnD5eCharacterClassData> ClassesToDisplay { get; }

		public ICommand AddClassCommand { get; }
		public ICommand RollHitdieCommand { get; }

		public DialogWindowDnD5eCharacterLevelupViewModel(DialogServiceBase dialogService, DnD5eCharacter character)
		{
			_dialogService = dialogService;
			_character = character;

			_characterName = character.Name;
			_maxHealth = _character.Health.MaxHealth;

			_numberOfClassesAdded = 0;

			WeaponProfsToDisplay = new();
			ArmorProfsToDisplay = new();
			ToolProfsToDisplay = new();

			FeaturesToDisplay = new();
			ClassesToDisplay = new(GetClassesToDisplay(character));
			_selectedCharacterClass = ClassesToDisplay[0];
			PopulateFeaturesToDisplay();


			AddClassCommand = new RelayCommand(AddClass);
			RollHitdieCommand = new RelayCommand(RollForMaxHealth);
		}

		/// <summary>
		/// Populate the features that the character will unlock if they take a level in the selected class.
		/// </summary>
		/// <exception cref="Exception">Data for the selected class cannot be found.</exception>
		private void PopulateFeaturesToDisplay()
		{
			FeaturesToDisplay.Clear();
			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);

			// find character class
			DnD5eCharacterClassData? data = classes.Find(x => x.Name.Equals(_selectedCharacterClass.Name));

			// className contains extra data, cut it off and search again.
			data ??= classes.Find(x => x.Name.Equals(_selectedCharacterClass.Name.Substring(0, _selectedCharacterClass.Name.IndexOf(" "))));

			if (data == null)
				throw new Exception("The class " + _selectedCharacterClass.Name + " does not exist");

			foreach (var item in data.Features)
			{
				if (item.Level == _selectedCharacterClass.Level.Level + 1)
				{
					FeaturesToDisplay.Add(item);
				}
			}
		}

		/// <summary>
		/// Unlock the features of a specified class for a specified level.
		/// </summary>
		/// <param name="character">Character that will get the features.</param>
		/// <param name="className">Name of the class whose features are being unlocked.</param>
		/// <param name="classLevel">Level of the features being unlocked.</param>
		private void UnlockNewClassFeatures(DnD5eCharacter character, string className, int classLevel)
		{
			var classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson);

			// find character class
			DnD5eCharacterClassData? data = classes.Find(x => x.Name.Equals(className));

			// className contains extra data, cut it off and search again.
			data ??= classes.Find(x => x.Name.Equals(className.Substring(0, className.IndexOf(" "))));

			if (data == null)
				throw new Exception("The class " + className + " does not exist");

			foreach (var item in data.Features)
			{
				if (item.Level == classLevel+1)
				{
					if (item.Name.ToLower().Contains("ability score"))
					{
						var message = _dialogService.ShowMessage(item.Desc, "You get an ability score improvement",
							MessageBoxButton.OK, MessageBoxImage.None); 
						continue;
					}

					character.CharacterClass.Features.Add(item);
				}
			}
		}

		/// <summary>
		/// Populates the weapon, armor, and tool profs the character will gain if they take a level in the selected class.
		/// </summary>
		private void PopulateProfsToDisplay()
		{
			var classData = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).Where(x => x.Name == _selectedCharacterClass.Name).First();

			if (classData is null)
				throw new Exception("Could not find data for class " + _selectedCharacterClass.Name);

			ToolProfsToDisplay.Clear();
			WeaponProfsToDisplay.Clear();
			ArmorProfsToDisplay.Clear();

			ToolProfsToDisplay.AddRange(classData.GetGainedToolProficiences(_character));
			WeaponProfsToDisplay.AddRange(classData.GetGainedWeaponProficiences(_character));
			ArmorProfsToDisplay.AddRange(classData.GetGainedArmorProficiences(_character));
		}

		/// <summary>
		/// Gets data about the classes the character has.
		/// </summary>
		/// <param name="character">Character to get class data from.</param>
		/// <returns>Array of class data for each class the character has.</returns>
		/// <exception cref="Exception">Could not find data on any of the characters classes.</exception>
		/// <exception cref="ArithmeticException">Could not get the level from one of the classes the character has.</exception>
		private DnD5eCharacterClassData[] GetClassesToDisplay(DnD5eCharacter character)
		{
			List<DnD5eCharacterClassData> results = new();
			var classData = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();

			var classNamesAndLevels = character.CharacterClass.GetClassNamesAndLevels();

			results.AddRange(classData.Where(x => classNamesAndLevels.Contains(y => y.Key.Contains(x.Name))));

			if (results.Any() == false)
				throw new Exception("Could not find any classes");

			for (int i = 0; i < results.Count; i++)
			{
				results[i].Level.Level = classNamesAndLevels.Where(x => x.Key == results[i].Name).First().Value;
			}

			return results.ToArray();
		}

		/// <summary>
		/// User selects a class to add to their character.
		/// </summary>
		/// <exception cref="Exception">Cannot find data on class the user wants to add.</exception>
		private void AddClass()
		{
			string classToAddName = GetClassToAddName(_character);

			if (classToAddName == string.Empty)
				return;

			var classData = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();

			DnD5eCharacterClassData classToAddData = classData.Where(x => x.Name.Equals(classToAddName)).First();
			
			if (classToAddData == null)
				throw new Exception("Cannot find data for class " + classToAddName);

			classToAddData.Level.Level = 0;

			_character.CharacterClass.Name += "/" + classToAddName + " 0";

			ClassesToDisplay.Add(classToAddData);
			_hasAddedClass = true;
			_numberOfClassesAdded += 1;
		}

		/// <summary>
		/// Rolls the selected classes hitdie to determine a new max health value.
		/// </summary>
		private void RollForMaxHealth()
		{
			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(_selectedCharacterClass.HitDie, 1);
			int currHealth = _character.Health.MaxHealth;

			MaxHealth = currHealth + numToAddToHealth;
		}

		/// <summary>
		/// Add the proficiences of a class to a character.
		/// </summary>
		/// <param name="character">Character to add the proficines too.</param>
		/// <param name="className">Name of the class to get the proficiences from.</param>
		/// <exception cref="Exception"></exception>
		private void AddNewClassProficiences(DnD5eCharacter character, string className)
		{
			var classData = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).Where(x => x.Name == className).First();

			if (classData is null)
				throw new Exception("Could not find data for class " + className);

			classData.AddProficiences(character);

			string[] proficientSkills = Ability.GetProficientSkillNames(character.Abilities);
			string[] options = classData.PossibleSkillProficiences.Where(x => proficientSkills.Contains(x) == false).ToArray();

			if (options.Any())
			{
				DialogWindowSelectStingValueViewModel vm =
					new DialogWindowSelectStingValueViewModel(options, 1);

				string result = string.Empty;
				_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(vm, r =>
				{
					result = r;
				});

				if (result == false.ToString())
					return;
			}

			// TODO: do something with the selected item(s).
		}

		/// <summary>
		/// Ask the user for the name of the class they want to add. 
		/// </summary>
		/// <param name="character">Character that will get the class.</param>
		/// <returns>Name of the class the user wants to add OR  an empty string when no class is selected.</returns>
		private string GetClassToAddName(DnD5eCharacter character)
		{
			var classes = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray();
			string[] classNames = new string[classes.Length];

			for (int i = 0; i < classes.Length; i++)
			{
				// exclude classes the character already has and classes the character does
				// not meet the prerequisites for.
				if (character.CharacterClass.Name.Contains(classes[i].Name) || classes[i].MeetsPrerequisites(character) == false)
					continue;

				classNames[i] = classes[i].Name;
			}

			classNames = classNames.Where(x => x is not null).ToArray();

			DialogWindowSelectStingValueViewModel vm =
				new DialogWindowSelectStingValueViewModel(classNames, 1);

			string result = string.Empty;
			_dialogService.ShowDialog<SelectStringValueDialogWindow, DialogWindowSelectStingValueViewModel>(vm, r =>
			{
				result = r;
			});

			if (result == false.ToString() || vm.SelectedItems.Any() == false)
				return string.Empty;

			return vm.SelectedItems.First();
		}

		public void ProcessLevelup()
		{
			int newLevel = DnD5eDialogStreamCharacterLeveler.GetCurrentLevelOfClassBeingLeveledUp(_character, 
				_selectedCharacterClass.Name, _character.CharacterClass.Name.Split("/"));
			
			if (_hasAddedClass)
				AddNewClassProficiences(_character, _selectedCharacterClass.Name);
			
			UnlockNewClassFeatures(_character, _selectedCharacterClass.Name, newLevel);
			_character.CharacterClass.UpdateCharacterClassName(_selectedCharacterClass.Name, newLevel);
			
			_character.Health.MaxHealth = MaxHealth;

			_character.Level.LevelUp();
			foreach (var ability in _character.Abilities)
			{
				ability.SetProfBonus(_character.Level.ProficiencyBonus);
			}
		}

		/// <summary>
		/// Removes any classes that where added to the character.
		/// </summary>
		internal void RemoveAddedClasses()
		{
			for (int i = 0; i < _numberOfClassesAdded; i++)
			{
				string characterClasses = _character.CharacterClass.Name;
				int lengthOfClassNameToKeep = characterClasses.LastIndexOf('/');
				_character.CharacterClass.Name = characterClasses.Substring(0, lengthOfClassNameToKeep);
			}
		}
	}
}
