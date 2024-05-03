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
		private readonly CharacterInfoViewModel _characterInfoVM;
		private readonly Window _window;

		public ObservableCollection<string> FeatureTypeOptions { get; }

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
				BasicStringFieldValidation(nameof(Name), _name);
			}
		}

		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				OnPropertyChanged(ref _description, value);
				BasicStringFieldValidation(nameof(Description), _description);
			}
		}

		private string _level;
		public string Level
		{
			get
			{
				return _level;
			}
			set
			{
				OnPropertyChanged(ref _level, value);
				LevelValidation();
			}
		}

		private string _featureType;
		public string FeatureType
		{
			get
			{
				return _featureType;
			}
			set
			{
				OnPropertyChanged(ref _featureType, value);
				LevelValidation();
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

		public Dictionary<string, List<string>> propertyNameToError;
		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
		public bool HasErrors => propertyNameToError.Any();

		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

		public DialogWindowAddFeatureViewModel(Window window, CharacterInfoViewModel characterInfoVM) 
		{
			_characterInfoVM = characterInfoVM;
			_window = window;

			propertyNameToError = new Dictionary<string, List<string>>();

			FeatureTypeOptions = new ObservableCollection<string>()
			{
				_characterInfoVM.SelectedCharacter.CharacterClass.Name,
				_characterInfoVM.SelectedCharacter.Race.Name,
				_characterInfoVM.SelectedCharacter.Race.RaceVariant.Name
			};

			_name = string.Empty;
			_description = string.Empty;
			_level = string.Empty;
			_featureType = this._characterInfoVM.SelectedCharacter.CharacterClass.Name;

			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);

			BasicStringFieldValidation(nameof(Name), _name);
			BasicStringFieldValidation(nameof(Description), _description);
			LevelValidation();
		}

		private void Cancel()
		{
			_window.DialogResult = false;
		}

		private void Ok()
		{
			if (_characterInfoVM.SelectedCharacter is null)
				return;

			Property property = new Property(_name, _description);
			Feature feature = new Feature(property, _featureType, _level);
			_characterInfoVM.AllFeatures.Add(feature);

			
			if (_featureType == _characterInfoVM.SelectedCharacter.CharacterClass.Name)
			{
				DnD5eCharacterClassFeature classFeature = new(feature.Name, feature.Description, int.Parse(feature.Level));
				_characterInfoVM.SelectedCharacter.CharacterClass.Features.Add(classFeature);
			}
			else if (_featureType == _characterInfoVM.SelectedCharacter.Race.Name)
			{
				_characterInfoVM.SelectedCharacter.Race.Features.Add(property);
			}
			else if (_featureType == _characterInfoVM.SelectedCharacter.Race.RaceVariant.Name)
			{
				_characterInfoVM.SelectedCharacter.Race.RaceVariant.Properties.Add(property);
			}

			_window.DialogResult = true;
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
			if (_characterInfoVM.SelectedCharacter is null)
				return;

			propertyNameToError.Remove(nameof(Level));

			List<string> errors = new();
			propertyNameToError.Add(nameof(Level), errors);

			if (FeatureType == _characterInfoVM.SelectedCharacter.CharacterClass.Name) 
			{
				try
				{
					int i = int.Parse(_level);
					_level = i.ToString();
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
				_level = "-";
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
