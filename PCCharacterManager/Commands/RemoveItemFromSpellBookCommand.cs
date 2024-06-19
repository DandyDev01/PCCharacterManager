using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCCharacterManager.ViewModels;
using PCCharacterManager.Models;
using System.Windows;
using PCCharacterManager.Services;

namespace PCCharacterManager.Commands
{
	public class RemoveItemFromSpellBookCommand : BaseCommand
	{
		private readonly CharacterSpellBookViewModel _characterSpellBookViewModel;
		private readonly DialogServiceBase _dialogService;
		private readonly SpellType _spellType;
		private double _lastSpellRemoveTimeInSeconds;
		private double _lastCantripRemoveTimeInSeconds;

		public RemoveItemFromSpellBookCommand(CharacterSpellBookViewModel characterSpellBookViewModel, DialogServiceBase dialogService, SpellType spellType)
		{
			_characterSpellBookViewModel = characterSpellBookViewModel;
			_dialogService = dialogService;
			_spellType = spellType;

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			_lastSpellRemoveTimeInSeconds = timeSpan.TotalSeconds - 10;
			_lastCantripRemoveTimeInSeconds = timeSpan.TotalSeconds - 10;
		}

		public override void Execute(object? parameter)
		{
			switch (_spellType)
			{
				case SpellType.SPELL:
					DeleteSpell();
					break;
				case SpellType.CANTRIP:
					DeleteCantrip();
					break;
			}
		}

		/// <summary>
		/// delete the selected spell
		/// </summary>
		private void DeleteSpell()
		{
			if (_characterSpellBookViewModel.PrevSelectedSpell == null)
				return;

			double currTimeSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			double timePassed = currTimeSeconds - _lastSpellRemoveTimeInSeconds;
			if (timePassed > 5)
			{
				var messageBox = _dialogService.ShowMessage("Are you sure you want to delete " + 
					_characterSpellBookViewModel.PrevSelectedSpell.Spell.Name, "Delete Spell", 
					MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (messageBox == MessageBoxResult.No)
					return;
			}

			_characterSpellBookViewModel.SpellBook.RemoveSpell(_characterSpellBookViewModel.PrevSelectedSpell.Spell);
			_characterSpellBookViewModel.SpellsToDisplay.Remove(_characterSpellBookViewModel.PrevSelectedSpell);
			_characterSpellBookViewModel.SelectedSpell = null;

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			_lastSpellRemoveTimeInSeconds = timeSpan.TotalSeconds;
		}

		/// <summary>
		/// delete the selected cantrip
		/// </summary>
		private void DeleteCantrip()
		{
			if (_characterSpellBookViewModel.PrevSelectedCantrip == null)
				return;

			double currTimeSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			double timePassed = currTimeSeconds - _lastSpellRemoveTimeInSeconds;
			if (timePassed > 5)
			{
				var messageBox = _dialogService.ShowMessage("Are you sure you want to delete " + 
					_characterSpellBookViewModel.PrevSelectedCantrip.Spell.Name, "Delete Spell", 
					MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (messageBox == MessageBoxResult.No)
					return;
			}

			_characterSpellBookViewModel.SpellBook.RemoveCantrip(_characterSpellBookViewModel.PrevSelectedCantrip.Spell);
			_characterSpellBookViewModel.CantripsToDisplay.Remove(_characterSpellBookViewModel.PrevSelectedCantrip);
			_characterSpellBookViewModel.SelectedCantrip = null;

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			_lastSpellRemoveTimeInSeconds = timeSpan.TotalSeconds;
		}
	}
}
