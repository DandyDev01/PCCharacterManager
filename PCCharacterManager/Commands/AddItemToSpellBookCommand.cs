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
		private readonly CharacterSpellBookViewModel _spellBookViewModel;
		private readonly DialogServiceBase _dialogService;
		private readonly SpellType _spellType;

		public AddItemToSpellBookCommand(CharacterSpellBookViewModel spellBookViewModel, SpellType spellType, DialogServiceBase dialogService)
		{
			_spellBookViewModel = spellBookViewModel;
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
			DialogWindowAddSpellViewModel data = new(SpellType.SPELL);
			string result = string.Empty;
			_dialogService.ShowDialog<AddSpellDialogWindow, DialogWindowAddSpellViewModel>(data, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			SpellItemEditableViewModel temp = new(data.NewSpell);
			_spellBookViewModel.SpellsToDisplay.Add(temp);
			_spellBookViewModel.SpellBook.AddSpell(data.NewSpell);
		}

		/// <summary>
		/// open a widow to add a new cantrip
		/// </summary>
		private void AddCantripWindow()
		{
			DialogWindowAddSpellViewModel data = new(SpellType.CANTRIP);
			string result = string.Empty;
			_dialogService.ShowDialog<AddSpellDialogWindow, DialogWindowAddSpellViewModel>(data, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;

			SpellItemEditableViewModel temp = new(data.NewSpell);
			_spellBookViewModel.CantripsToDisplay.Add(temp);
			_spellBookViewModel.SpellBook.AddContrip(data.NewSpell);
		}
	}
}
