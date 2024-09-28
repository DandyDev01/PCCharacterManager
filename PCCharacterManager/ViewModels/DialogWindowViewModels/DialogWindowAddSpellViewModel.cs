using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowAddSpellViewModel : ObservableObject
	{
		public Array SpellSchools { get; } = Enum.GetValues(typeof(SpellSchool));
		private readonly SpellType _spellFilterType;

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

		public DialogWindowAddSpellViewModel(SpellType spellFilterType)
		{
			_newSpell = new Spell();
			_spellComponents = string.Empty;

			_spellFilterType = spellFilterType;
		}

		public void AddNewSpell()
		{
			_newSpell.School = _selectedSchool;
			if (_spellFilterType == SpellType.CANTRIP)
				_newSpell.IsPrepared = true;
		}
	}
}
