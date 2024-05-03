using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowAddSpellViewModel : ObservableObject
	{
		public Array SpellSchools { get; } = Enum.GetValues(typeof(SpellSchool));
		private readonly SpellType _spellFilterType;
		private readonly Window _window;

		private Spell _newSpell;
		public Spell NewSpell
		{
			get { return _newSpell; }
			set { OnPropertyChanged(ref _newSpell, value); }
		}

		private string _spellComponents;
		public string SpellComponents
		{
			get { return _spellComponents; }
			set
			{
				OnPropertyChanged(ref _spellComponents, value);

			}
		}

		private SpellSchool _selectedSchool;
		public SpellSchool SelectedSchool 
		{
			set
			{
				_selectedSchool = value;
			}
		}

		public ICommand AddSpellCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		public DialogWindowAddSpellViewModel(Window window, SpellType spellFilterType)
		{
			_window = window;
			_newSpell = new Spell();
			_spellComponents = string.Empty;

			_spellFilterType = spellFilterType;

			AddSpellCommand = new RelayCommand(AddNewSpell);
			CancelCommand = new RelayCommand(Close);
		}

		private void AddNewSpell()
		{
			_newSpell.School = _selectedSchool;
			if (_spellFilterType == SpellType.CANTRIP) _newSpell.IsPrepared = true;

			_window.DialogResult = true;
			_window.Close();
		}

		private void Close()
		{
			_window.DialogResult = false;
			_window.Close();
		}
	}
}
