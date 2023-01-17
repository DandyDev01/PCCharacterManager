using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class SpellItemEditableViewModel : ObservableObject
	{
		private Spell? spell;
		public Spell? Spell
		{
			get { return spell; }
			set { OnPropertyChanged(ref spell, value); }
		}

		private bool isPrepared;
		public bool IsPrepared
		{
			get { return isPrepared; }
			set { OnPropertyChanged(ref isPrepared, value); }
		}

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			private set
			{
				OnPropertyChanged(ref isEditMode, value);
				OnPropertyChaged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		public ICommand PrepareCommand { get; set; }
		public Action<Spell>? Prepare;

		public SpellItemEditableViewModel()
		{
			PrepareCommand = new RelayCommand(InvokePrepare);
			isEditMode = false;
		}

		public SpellItemEditableViewModel(Spell _spell)
		{
			spell = _spell;
			PrepareCommand = new RelayCommand(InvokePrepare);
			isEditMode = false;
		}

		private void InvokePrepare()
		{
			IsPrepared = !IsPrepared;
			Prepare?.Invoke(spell);
		}

		public void Edit()
		{
			IsEditMode = !isEditMode;
		}

		public void Bind(Spell _spell)
		{
			spell = _spell;
			isEditMode = false;
		}
	}
}
