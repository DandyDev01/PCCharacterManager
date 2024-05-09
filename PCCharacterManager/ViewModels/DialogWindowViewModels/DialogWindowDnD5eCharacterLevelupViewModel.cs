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

			FeaturesToDisplay = new();
			ClassesToDisplay = new(GetClassesToDisplay(character));
			_selectedCharacterClass = ClassesToDisplay[0];
			PopulateFeaturesToDisplay();


			AddClassCommand = new RelayCommand(AddClass);
			RollHitdieCommand = new RelayCommand(RollForMaxHealth);
		}

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
					if (item.Name.ToLower().Contains("ability score"))
					{
						var message = MessageBox.Show(item.Desc, "You get an ability score improvement", MessageBoxButton.OK);
						continue;
					}

					FeaturesToDisplay.Add(item);
				}
			}
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
			var characterClassNames = character.CharacterClass.Name.Split('/');
			var classLevels = new int[characterClassNames.Length];

			for (int i = 0; i < characterClassNames.Length; i++)
			{
				string level = characterClassNames[i].Substring(characterClassNames[i].IndexOf(" "));
				if (int.TryParse(level, out classLevels[i]) == false)
					throw new ArithmeticException("Could not get level.");

				string name = characterClassNames[i].Substring(0, characterClassNames[i].IndexOf(" "));
				characterClassNames[i] = name;
			}

			results.AddRange(classData.Where(x => characterClassNames.Contains(x.Name)));

			if (results.Any() == false)
				throw new Exception("Could not find any classes");

			for (int i = 0; i < results.Count; i++)
			{
				results[i].Level.Level = classLevels[i];
			}

			return results.ToArray();
		}

		/// <summary>
		/// User selects a class to add to their character.
		/// </summary>
		/// <exception cref="Exception">Cannot find data on class the user wants to add.</exception>
		private void AddClass()
		{
			string classToAddName = GetClassToAddName(_character, _character.CharacterClass.Name);
			var classData = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();

			DnD5eCharacterClassData classToAddData = classData.Where(x => x.Name.Equals(classToAddName)).First();
			
			if (classToAddData == null)
				throw new Exception("Cannot find data for class " + classToAddName);

			classToAddData.Level.Level = 0;

			_character.CharacterClass.Name += "/" + classToAddName + " 0";

			ClassesToDisplay.Add(classToAddData);
		}

		private void IncreaseAbilityScore()
		{
			throw new NotImplementedException();
		}

		private void RollForMaxHealth()
		{
			RollDie rollDie = new RollDie();

			int numToAddToHealth = rollDie.Roll(_selectedCharacterClass.HitDie, 1);
			int currHealth = _character.Health.MaxHealth;

			MaxHealth = currHealth + numToAddToHealth;
		}

		/// <summary>
		/// Get the name of the class to add.
		/// </summary>
		/// <returns>Name of the class the user wants to add.</returns>
		private string GetClassToAddName(DnD5eCharacter character, string currentClasses)
		{
			var classes = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray();
			string[] classNames = new string[classes.Length];

			for (int i = 0; i < classes.Length; i++)
			{
				// exclude classes the character already has and classes the character does
				// not meet the prerequisites for.
				if (currentClasses.Contains(classes[i].Name) || MeetsPrerequisites(character, classes[i]) == false)
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

			if (result == false.ToString())
				return string.Empty;

			return vm.SelectedItems.First();
		}

		/// <summary>
		/// Determines if the character meets the prerequisites for the class they want to multiclass in.
		/// </summary>
		/// <param name="character">Character that is being checked.</param>
		/// <param name="characterMultiClassData">Multiclass prerequisite data.</param>
		/// <returns></returns>
		private bool MeetsPrerequisites(DnD5eCharacter character, CharacterMultiClassData characterMultiClassData)
		{
			string[] prerequisites = characterMultiClassData.Prerequisites.Split('^', '&');
			int[] score = new int[prerequisites.Length];

			// get ability prerequisite name and score.
			for (int i = 0; i < prerequisites.Length; i++)
			{
				prerequisites[i] = prerequisites[i].Trim();
				if (int.TryParse(prerequisites[i].Substring(prerequisites[i].IndexOf(" ")).Trim(), out score[i]) == false)
				{
					throw new Exception("Could not find score.");
				}
				prerequisites[i] = prerequisites[i].Substring(0, prerequisites[i].IndexOf(" ")).Trim();
			}

			// prerequsite contains an OR
			if (characterMultiClassData.Prerequisites.Contains('^'))
			{
				bool meetsOne = false;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i]).First();
					if (ability.Score >= score[i])
						meetsOne = true;
				}

				if (meetsOne)
					return true;
			}
			// prerequisite contains an AND
			else if (characterMultiClassData.Prerequisites.Contains('&'))
			{
				bool meetsAll = true;
				for (int i = 0; i < prerequisites.Length; i++)
				{
					Ability ability = character.Abilities.Where(x => x.Name == prerequisites[i]).First();
					if (ability.Score < score[i])
						meetsAll = false;
				}

				if (meetsAll)
					return true;
			}
			// there is only a single prerequisite 
			else
			{
				string abilityname = characterMultiClassData.Prerequisites;
				int abilityScore = 0;
				if (int.TryParse(abilityname.Substring(abilityname.IndexOf(" ")).Trim(), out abilityScore) == false)
				{
					throw new Exception("Could not find score.");
				}

				abilityname = abilityname.Substring(0, abilityname.IndexOf(" ")).Trim();
				Ability a = character.Abilities.Where(x => x.Name == abilityname).First();

				if (a.Score >= abilityScore)
					return true;
			}

			return false;
		}

		public void ProcessLevelup()
		{
			throw new NotImplementedException();
		}
	}
}
