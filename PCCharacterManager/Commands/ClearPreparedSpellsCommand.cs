using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class ClearPreparedSpellsCommand : BaseCommand
	{
		private readonly CharacterSpellBookViewModel _characterSpellBookViewModel;

		public ClearPreparedSpellsCommand(CharacterSpellBookViewModel characterSpellBookViewModel) 
		{
			_characterSpellBookViewModel = characterSpellBookViewModel;
		}

		public override void Execute(object? parameter)
		{
			_characterSpellBookViewModel.SpellBook.ClearPreparedSpells();

			foreach (var spellItemView in _characterSpellBookViewModel.SpellsToDisplay)
			{
				spellItemView.IsPrepared = false;
			}
		}
	}
}
