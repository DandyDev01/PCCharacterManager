using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
    public class DialogWindowAddFeatureViewModel : ObservableObject, INotifyDataErrorInfo
	{
		private readonly CharacterInfoViewModel characterInfoVM;
		private readonly Window window;

		public ObservableCollection<string> FeatureTypeOptions { get; }

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				OnPropertyChanged(ref name, value);
				BasicStringFieldValidation(nameof(Name), name);
			}
		}

		private string description;
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				OnPropertyChanged(ref description, value);
				BasicStringFieldValidation(nameof(Description), description);
			}
		}

		private string level;
		public string Level
		{
			get
			{
				return level;
			}
			set
			{
				OnPropertyChanged(ref level, value);
				LevelValidation();
			}
		}

		private string featureType;
		public string FeatureType
		{
			get
			{
				return featureType;
			}
			set
			{
				OnPropertyChanged(ref featureType, value);
				LevelValidation();
			}
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

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

		public DialogWindowAddFeatureViewModel(Window _window, CharacterInfoViewModel _characterInfoVM) 
		{
			characterInfoVM = _characterInfoVM;
			window = _window;

			propertyNameToError = new Dictionary<string, List<string>>();

			FeatureTypeOptions = new ObservableCollection<string>()
			{
				characterInfoVM.SelectedCharacter.CharacterClass.Name,
				characterInfoVM.SelectedCharacter.Race.Name,
				characterInfoVM.SelectedCharacter.Race.RaceVariant.Name
			};

			name = string.Empty;
			description = string.Empty;
			level = string.Empty;
			featureType = characterInfoVM.SelectedCharacter.CharacterClass.Name;

			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);

			BasicStringFieldValidation(nameof(Name), name);
			BasicStringFieldValidation(nameof(Description), description);
			LevelValidation();
		}

		private void Cancel()
		{
			window.DialogResult = false;
		}

		private void Ok()
		{
			if (characterInfoVM.SelectedCharacter is null)
				return;

			Property property = new Property(name, description);
			Feature feature = new Feature(property, featureType, level);
			characterInfoVM.AllFeatures.Add(feature);

			
			if (featureType == characterInfoVM.SelectedCharacter.CharacterClass.Name)
			{
				DnD5eCharacterClassFeature classFeature = new(feature.Name, feature.Description, int.Parse(feature.Level));
				characterInfoVM.SelectedCharacter.CharacterClass.Features.Add(classFeature);
			}
			else if (featureType == characterInfoVM.SelectedCharacter.Race.Name)
			{
				characterInfoVM.SelectedCharacter.Race.Features.Add(property);
			}
			else if (featureType == characterInfoVM.SelectedCharacter.Race.RaceVariant.Name)
			{
				characterInfoVM.SelectedCharacter.Race.RaceVariant.Properties.Add(property);
			}

			window.DialogResult = true;
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
			LevelValidation();
		}

		private void LevelValidation()
		{
			if (characterInfoVM.SelectedCharacter is null)
				return;

			propertyNameToError.Remove(nameof(Level));

			List<string> errors = new();
			propertyNameToError.Add(nameof(Level), errors);

			if (FeatureType == characterInfoVM.SelectedCharacter.CharacterClass.Name) 
			{
				try
				{
					int i = int.Parse(level);
					level = i.ToString();
					OnPropertyChanged(nameof(Level));
				}
				catch
				{
					propertyNameToError[nameof(Level)].Add("must be an whole number or '-'.");
					ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Level)));
					IsValid = !HasErrors;
				}
			}
			else
			{
				level = "-";
			}

			if (propertyNameToError[nameof(Level)].Any() == false)
			{
				propertyNameToError.Remove(nameof(Level));
			}

			IsValid = !HasErrors;
			OnPropertyChanged(nameof(Level));
		}
	}
}
