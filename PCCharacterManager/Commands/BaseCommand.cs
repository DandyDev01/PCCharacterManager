using PCCharacterManager.Utility;
using System;
using System.Windows.Input;

namespace PCCharacterManager.Commands
{
	public abstract class BaseCommand : ObservableObject, ICommand
	{
		public event EventHandler? CanExecuteChanged;

		public virtual bool CanExecute(object? parameter)
		{
			return true;
		}

		public abstract void Execute(object? parameter);

		protected void OnCanExecuteChaged()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
