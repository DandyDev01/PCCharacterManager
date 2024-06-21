using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using PCCharacterManager.DialogWindows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System.ComponentModel;
using System.Collections;

namespace PCCharacterManager.ViewModels
{
	public class StarfinderCharacterCreatorViewModel : CharactorCreatorViewModelBase, INotifyDataErrorInfo
	{
		private readonly DialogServiceBase _dialogService;

		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				OnPropertyChanged(ref _name, value);
				BasicStringFieldValidation(nameof(Name), value);
			}
		}

		private StarfinderRaceData _selectedRaceData;
		public StarfinderRaceData SelectedRaceData
		{
			get
			{
				return _selectedRaceData;
			}
			set
			{
				OnPropertyChanged(ref _selectedRaceData, value);
			}
		}

		private StarfinderClassData _selectedClassData;
		public StarfinderClassData SelectedClassData
		{
			get
			{
				return _selectedClassData;
			}
			set
			{
				OnPropertyChanged(ref _selectedClassData, value);
			}
		}

		private StarfinderThemeData _selectedThemeData;
		public StarfinderThemeData SelectedThemeData
		{
			get
			{
				return _selectedThemeData;
			}
			set
			{
				OnPropertyChanged(ref _selectedThemeData, value);
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

		public StarfinderRaceData[] RaceNamesToDisplay { get; }
		public StarfinderClassData[] ClassNamesToDisplay { get; }
		public StarfinderThemeData[] ThemeNamesToDisplay { get; }

		public int[] AbilityScores { get; } = new int[6];

		public ICommand RollAbilityScoresCommand { get; private set; }

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public StarfinderCharacterCreatorViewModel(DialogServiceBase dialogService)
		{
			_dialogService = dialogService;
			propertyNameToError = new Dictionary<string, List<string>>();

			RaceNamesToDisplay = ReadWriteJsonCollection<StarfinderRaceData>.ReadCollection(StarfinderResources.RaceDataJson).ToArray();
			ClassNamesToDisplay = ReadWriteJsonCollection<StarfinderClassData>.ReadCollection(StarfinderResources.CharacterClassDataJson).ToArray();
			ThemeNamesToDisplay = ReadWriteJsonCollection<StarfinderThemeData>.ReadCollection(StarfinderResources.ThemeDataJson).ToArray();

			_name = string.Empty;
			_selectedRaceData = RaceNamesToDisplay[0];
			_selectedClassData = ClassNamesToDisplay[0];
			_selectedThemeData = ThemeNamesToDisplay[0];

			RollAbilityScoresCommand = new RelayCommand(AbilityRoll);

			BasicStringFieldValidation(nameof(Name), Name);
		}

		public override StarfinderCharacter? Create()
		{
			int increaseAmount = 0;
			string abilityName = string.Empty;

			bool validName = !string.IsNullOrWhiteSpace(_name) && !string.IsNullOrEmpty(_name);

			if (!validName)
			{
				MessageBox.Show("Name cannot be empty or only whitespace", "Invalid Name", 
					MessageBoxButton.OK, MessageBoxImage.Error);

				return null;
			}

			StarfinderCharacter character = new(SelectedClassData, SelectedRaceData, _selectedThemeData)
			{
				Name = _name
			};

			int hitPoints = _selectedRaceData.HitPoints + _selectedClassData.HitPoints;
			int staminaPoints = _selectedClassData.StaminaPoints;

			character.StaminaPoints.Desc = staminaPoints.ToString();
			character.Health.SetMaxHealth(hitPoints);

			// set ability scores
			for (int i = 0; i < AbilityScores.Length; i++)
			{
				character.Abilities[i].Score = AbilityScores[i];
			}

			// set class skills
			foreach (var classSkill in _selectedClassData.ClassSkills)
			{
				if (classSkill.Contains("Profession")) continue;

				try
				{
					StarfinderAbility.FindSkill(character.Abilities, classSkill).ClassSkill = true;
				} 
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
					return null;
				}
			}

			// class skill profession
			string[] skills = _selectedClassData.ClassSkills.Where(x => x.Contains("Profession")).ToArray();
			foreach (var item in skills)
			{
				string[] options = StringFormater.CreateGroup(item, '^');
				options[0] = options[0].Substring(options[0].IndexOf('(') + 1);
				options[options.Length - 1] = options[options.Length - 1].Substring(0, options[options.Length - 1].Length - 1);
				
				
				DialogWindowSelectStingValueViewModel windowVM = new(options);

				string result = string.Empty;
				_dialogService.ShowDialog<SelectStringValueDialogWindow, 
					DialogWindowSelectStingValueViewModel>(windowVM, r =>
				{
					result = r;
				});

				if (result == false.ToString())
					return null;

				string selected = windowVM.SelectedItems.First();

				character.CharacterClass.Features.Add(new DnD5eCharacterClassFeature("Class skill profession", selected, 1));
			}

			character.KeyAbilityScore = _selectedClassData.KeyAbilityScore;

			// race ability score increases
			foreach (var item in _selectedRaceData.AbilityScoreIncreases)
			{
				increaseAmount = StringFormater.FindQuantity(item);
				abilityName = StringFormater.RemoveQuantity(item);
				try
				{
					Ability.FindAbility(character.Abilities, abilityName).Score += increaseAmount;
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
					return null;
				}
			}

			// race features
			foreach (var item in _selectedRaceData.Features)
			{
				character.Race.Features.Add(item);
			}


			// theme ability score improvements
			increaseAmount = StringFormater.FindQuantity(_selectedThemeData.AbilityScoreImprovement);
			abilityName = StringFormater.RemoveQuantity(_selectedThemeData.AbilityScoreImprovement);

			try
			{
				Ability.FindAbility(character.Abilities, abilityName).Score += increaseAmount;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
				return null;
			}

			if (SelectedClassData.Note.Title != string.Empty)
			{
				character.NoteManager.NoteSections[0].Notes.Add(SelectedClassData.Note);
			}

			character.Id = CharacterIDGenerator.GenerateID();

			return character;
		}

		private void AbilityRoll()
		{
			RollDie rollDie = new();

			for (int i = 0; i < 6; i++)
			{
				AbilityScores[i] = rollDie.AbilityScoreRoll();
				OnPropertyChanged(nameof(AbilityScores));
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
	}
}
