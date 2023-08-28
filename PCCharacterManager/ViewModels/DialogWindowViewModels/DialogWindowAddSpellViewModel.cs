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
		private readonly SpellType spellFilterType;
		private readonly Window window;

		private Spell newSpell;
		public Spell NewSpell
		{
			get { return newSpell; }
			set { OnPropertyChanged(ref newSpell, value); }
		}

		private string spellComponents;
		public string SpellComponents
		{
			get { return spellComponents; }
			set
			{
				OnPropertyChanged(ref spellComponents, value);

			}
		}

		private SpellSchool selectedSchool;
		public SpellSchool SelectedSchool 
		{
			set
			{
				selectedSchool = value;
			}
		}

		public ICommand AddSpellCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		public DialogWindowAddSpellViewModel(Window _window, SpellType _spellFilterType)
		{
			window = _window;
			newSpell = new Spell();
			spellComponents = string.Empty;

			spellFilterType = _spellFilterType;

			AddSpellCommand = new RelayCommand(AddNewSpell);
			CancelCommand = new RelayCommand(Close);
		}

		private void AddNewSpell()
		{
			newSpell.School = selectedSchool;
			if (spellFilterType == SpellType.CANTRIP) newSpell.IsPrepared = true;

			window.DialogResult = true;
			window.Close();
		}

		private void Close()
		{
			window.DialogResult = false;
			window.Close();
		}
	}
}
