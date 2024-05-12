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
		private readonly CharacterSpellBookViewModel _characterSpellBookViewModel;
		private readonly DialogServiceBase _dialogService;
		private readonly SpellType _spellType;

		public AddItemToSpellBookCommand(CharacterSpellBookViewModel characterSpellBookViewModel, SpellType spellType, DialogServiceBase dialogService)
		{
			_characterSpellBookViewModel = characterSpellBookViewModel;
			_spellType = spellType;
			_dialogService = dialogService;
		}

		public override void Execute(object? parameter)
		{
			switch (_spellType)
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
			DialogWindowAddSpellViewModel dataContext = new(SpellType.SPELL);
			string result = string.Empty;
			_dialogService.ShowDialog<AddSpellDialogWindow, DialogWindowAddSpellViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			SpellItemEditableViewModel newSpellItem = new(dataContext.NewSpell);
			_characterSpellBookViewModel.SpellsToDisplay.Add(newSpellItem);
			_characterSpellBookViewModel.SpellBook.AddSpell(dataContext.NewSpell);
		}

		/// <summary>
		/// open a widow to add a new cantrip
		/// </summary>
		private void AddCantripWindow()
		{
			DialogWindowAddSpellViewModel dataContext = new(SpellType.CANTRIP);
			string result = string.Empty;
			_dialogService.ShowDialog<AddSpellDialogWindow, DialogWindowAddSpellViewModel>(dataContext, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			SpellItemEditableViewModel newSpellItem = new(dataContext.NewSpell);
			_characterSpellBookViewModel.CantripsToDisplay.Add(newSpellItem);
			_characterSpellBookViewModel.SpellBook.AddContrip(dataContext.NewSpell);
		}
	}
}
