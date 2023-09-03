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
		private double lastSpellRemoveTimeInSeconds;
		private double lastCantripRemoveTimeInSeconds;

		public RemoveItemFromSpellBookCommand(CharacterSpellBookViewModel _vm, SpellType _spellType)
		{
			vm = _vm;
			spellType = _spellType;
			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastSpellRemoveTimeInSeconds = timeSpan.TotalSeconds - 10;
			lastCantripRemoveTimeInSeconds = timeSpan.TotalSeconds - 10;
		}

		public override void Execute(object? parameter)
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

			double currTimeSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			double timePassed = currTimeSeconds - lastSpellRemoveTimeInSeconds;
			if (timePassed > 5)
			{
				var messageBox = MessageBox.Show("Are you sure you want to delete " + vm.PrevSelectedSpell.Spell.Name, "Delete Spell", MessageBoxButton.YesNo);
				if (messageBox == MessageBoxResult.No)
					return;
			}

			vm.SpellBook.RemoveSpell(vm.PrevSelectedSpell.Spell);
			vm.SpellsToDisplay.Remove(vm.PrevSelectedSpell);
			vm.SelectedSpell = null;

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastSpellRemoveTimeInSeconds = timeSpan.TotalSeconds;
		}

		/// <summary>
		/// delete the selected cantrip
		/// </summary>
		private void DeleteCantrip()
		{
			if (vm.PrevSelectedCantrip == null)
				return;

			double currTimeSeconds = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
			double timePassed = currTimeSeconds - lastSpellRemoveTimeInSeconds;
			if (timePassed > 5)
			{
				var messageBox = MessageBox.Show("Are you sure you want to delete " + vm.PrevSelectedCantrip.Spell.Name, "Delete Spell", MessageBoxButton.YesNo);
				if (messageBox == MessageBoxResult.No)
					return;
			}

			vm.SpellBook.RemoveCantrip(vm.PrevSelectedCantrip.Spell);
			vm.CantripsToDisplay.Remove(vm.PrevSelectedCantrip);
			vm.SelectedCantrip = null;

			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
			lastSpellRemoveTimeInSeconds = timeSpan.TotalSeconds;
		}
	}
}
