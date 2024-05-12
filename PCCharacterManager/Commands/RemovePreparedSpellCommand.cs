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
        private readonly CharacterSpellBookViewModel _characterSpellBookViewModel;

		public RemovePreparedSpellCommand(CharacterSpellBookViewModel characterSpellBookViewModel)
		{
			_characterSpellBookViewModel = characterSpellBookViewModel;
		}

		public override void Execute(object? parameter)
		{
			if (_characterSpellBookViewModel.SelectedPreparedSpell == null)
				return;

			if (_characterSpellBookViewModel.SpellBook.PreparedSpells.Contains(_characterSpellBookViewModel.SelectedPreparedSpell))
			{
				_characterSpellBookViewModel.SelectedPreparedSpell.IsPrepared = false;
				_characterSpellBookViewModel.SpellBook.PreparedSpells.Remove(_characterSpellBookViewModel.SelectedPreparedSpell);
				
				List<SpellItemEditableViewModel> spells = _characterSpellBookViewModel.SpellsToDisplay.ToList();
				SpellItemEditableViewModel? spellItem
					= spells.Find(x => x.Spell.Name.Equals(_characterSpellBookViewModel.SelectedPreparedSpell.Name));
				
				if (spellItem == null) 
					return;
				
				spellItem.IsPrepared = false;
				_characterSpellBookViewModel.SelectedPreparedSpell = null;
			}
		}
	}
}
