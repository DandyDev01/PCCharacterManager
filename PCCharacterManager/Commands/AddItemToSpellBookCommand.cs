using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCCharacterManager.ViewModels;
using PCCharacterManager.Models;
using System.Windows;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;

namespace PCCharacterManager.Commands
{
	public class AddItemToSpellBookCommand : BaseCommand
	{
		private readonly CharacterSpellBookViewModel vm;
		private readonly SpellType spellType;

		public AddItemToSpellBookCommand(CharacterSpellBookViewModel _vm, SpellType _spellType)
		{
			vm = _vm;
			spellType = _spellType;
		}

		public override void Execute(object parameter)
		{
			switch (spellType)
			{
				case SpellType.SPELL:
					AddSpellWindow();
					break;
				case SpellType.CANTRIP:
					AddCantripWindow();
					break;
			}
		}

		/// <summary>
		/// Open window to add new Spell
		/// </summary>
		private void AddSpellWindow()
		{
			Window window = new AddSpellDialogWindow();
			DialogWindowAddSpellViewModel data = new DialogWindowAddSpellViewModel(window, SpellType.SPELL);
			window.DataContext = data;

			var result = window.ShowDialog();

			if (result == false) return;

			SpellItemEditableViewModel temp = new SpellItemEditableViewModel(data.NewSpell);
			vm.SpellsToDisplay.Add(temp);
			vm.SpellBook.AddSpell(data.NewSpell);
		}

		/// <summary>
		/// open a widow to add a new cantrip
		/// </summary>
		private void AddCantripWindow()
		{
			Window window = new AddSpellDialogWindow();
			DialogWindowAddSpellViewModel data = new DialogWindowAddSpellViewModel(window, SpellType.CANTRIP);
			window.DataContext = data;

			var result = window.ShowDialog();
			
			if(result == false) return;

			SpellItemEditableViewModel temp = new SpellItemEditableViewModel(data.NewSpell);
			vm.CantripsToDisplay.Add(temp);
			vm.SpellBook.AddContrip(data.NewSpell);
		}
	}
}
