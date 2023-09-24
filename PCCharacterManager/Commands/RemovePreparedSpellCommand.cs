using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Commands
{
	public class RemovePreparedSpellCommand : BaseCommand
    {
        private readonly CharacterSpellBookViewModel spellBookVM;

		public RemovePreparedSpellCommand(CharacterSpellBookViewModel _spellBookVM)
		{
			spellBookVM = _spellBookVM;
		}

		public override void Execute(object? parameter)
		{
			if (spellBookVM.SelectedPreparedSpell == null)
				return;

			if (spellBookVM.SpellBook.PreparedSpells.Contains(spellBookVM.SelectedPreparedSpell))
			{
				spellBookVM.SelectedPreparedSpell.IsPrepared = false;
				spellBookVM.SpellBook.PreparedSpells.Remove(spellBookVM.SelectedPreparedSpell);
				
				List<SpellItemEditableViewModel> spells = spellBookVM.SpellsToDisplay.ToList();
				SpellItemEditableViewModel? spellItem
					= spells.Find(x => x.Spell.Name.Equals(spellBookVM.SelectedPreparedSpell.Name));
				
				if (spellItem == null) 
					return;
				
				spellItem.IsPrepared = false;
				spellBookVM.SelectedPreparedSpell = null;
			}
		}
	}
}
