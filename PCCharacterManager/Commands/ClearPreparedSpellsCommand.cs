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
		private readonly CharacterSpellBookViewModel spellBookVM;

		public ClearPreparedSpellsCommand(CharacterSpellBookViewModel _spellBookVM) 
		{
			spellBookVM = _spellBookVM;
		}

		public override void Execute(object? parameter)
		{
			spellBookVM.SpellBook.ClearPreparedSpells();

			foreach (var spellItemView in spellBookVM.SpellsToDisplay)
			{
				spellItemView.IsPrepared = false;
			}
		}
	}
}
