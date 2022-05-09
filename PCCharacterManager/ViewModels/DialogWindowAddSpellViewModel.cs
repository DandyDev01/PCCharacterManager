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
	public class DialogWindowAddSpellViewModel : TabItemViewModel
	{
		private SpellFilterType spellFilterType;
		private Spell newSpell;
		public Spell NewSpell
		{
			get { return newSpell; }
			set { OnPropertyChaged(ref newSpell, value); }
		}
		private readonly Window window;

		private string spellComponents;
		public string SpellComponents
		{
			get { return spellComponents; }
			set
			{
				OnPropertyChaged(ref spellComponents, value);

			}
		}

		private SpellBook spellBook;

		public ICommand AddSpellCommand { get; private set; }
		public ICommand CancelCommand { get; private set; }

		public DialogWindowAddSpellViewModel(Window _window, CharacterStore _characterStore, SpellFilterType _spellFilterType,
			ICharacterDataService _dataService, Character _selectedCharacter = null) : base(_characterStore, _dataService, _selectedCharacter)
		{
			window = _window;
			newSpell = new Spell();
			spellBook = selectedCharacter.SpellBook;

			spellFilterType = _spellFilterType;

			AddSpellCommand = new RelayCommand(AddNewSpell);
			CancelCommand = new RelayCommand(Close);
		}

		private void AddNewSpell()
		{

			switch (spellFilterType)
			{
				case SpellFilterType.SPELL:
					spellBook.AddSpell(newSpell);
					break;
				case SpellFilterType.CANTRIP:
					newSpell.IsPrepared = true;
					spellBook.AddContrip(newSpell);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			window.Close();
		}

		private void Close()
		{
			window.Close();
		}
	}
}
