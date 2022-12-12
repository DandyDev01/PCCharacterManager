using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCCharacterManager.ViewModels;
using PCCharacterManager.Models;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class RemoveItemFromSpellBookCommand : BaseCommand
	{
		private readonly CharacterSpellBookViewModel vm;
		private readonly SpellType spellType;

		public RemoveItemFromSpellBookCommand(CharacterSpellBookViewModel _vm, SpellType _spellType)
		{
			vm = _vm;
			spellType = _spellType;
		}

		public override void Execute(object parameter)
		{
			switch (spellType)
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
			if (vm.PrevSelectedSpell == null)
				return;

			var messageBox = MessageBox.Show("Are you sure you want to delete " + vm.PrevSelectedSpell.Spell.Name, "Delete Spell", MessageBoxButton.YesNo);
			if (messageBox == MessageBoxResult.No)
				return;

			vm.SpellBook.RemoveSpell(vm.PrevSelectedSpell.Spell);
			vm.SpellsToDisplay.Remove(vm.PrevSelectedSpell);
			vm.FilteredSpells[vm.PrevSelectedSpell.Spell.School].Remove(vm.PrevSelectedSpell);
		}

		/// <summary>
		/// delete the selected cantrip
		/// </summary>
		private void DeleteCantrip()
		{
			if (vm.PrevSelectedCantrip == null)
				return;

			var messageBox = MessageBox.Show("Are you sure you want to delete " + vm.PrevSelectedCantrip.Spell.Name, "Delete Spell", MessageBoxButton.YesNo);
			if (messageBox == MessageBoxResult.No)
				return;

			vm.FilteredSpells[vm.PrevSelectedCantrip.Spell.School].Remove(vm.PrevSelectedCantrip);
			vm.SpellBook.RemoveCantrip(vm.PrevSelectedCantrip.Spell);
			vm.CantripsToDisplay.Remove(vm.PrevSelectedCantrip);
		}
	}
}
