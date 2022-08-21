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
		private readonly ICharacterDataService dataService;
		private readonly CharacterStore characterStore;

		public AddItemToSpellBookCommand(CharacterSpellBookViewModel _vm, ICharacterDataService _dataService,
				CharacterStore _characterStore, SpellType _spellType)
		{
			vm = _vm;
			spellType = _spellType;
			dataService = _dataService;
			characterStore = _characterStore;
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
			DialogWindowAddSpellViewModel data = new DialogWindowAddSpellViewModel(window, characterStore, SpellType.SPELL,
				dataService, characterStore.SelectedCharacter);

			window.DataContext = data;

			window.ShowDialog();
			SpellItemEditableViewModel temp = new SpellItemEditableViewModel(data.NewSpell);
			vm.FilteredSpells[temp.Spell.School].Add(temp);
			vm.SpellsToDisplay.Add(temp);
		}

		/// <summary>
		/// open a widow to add a new cantrip
		/// </summary>
		private void AddCantripWindow()
		{
			Window window = new AddSpellDialogWindow();
			DialogWindowAddSpellViewModel data = new DialogWindowAddSpellViewModel(window, characterStore, SpellType.CANTRIP,
				dataService, characterStore.SelectedCharacter);

			window.DataContext = data;

			window.ShowDialog();
			SpellItemEditableViewModel temp = new SpellItemEditableViewModel(data.NewSpell);
			vm.CantripItems.Add(temp);
			vm.CantripsToDisplay.Add(temp);
		}
	}
}
