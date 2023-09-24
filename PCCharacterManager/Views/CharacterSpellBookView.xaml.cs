using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for CharacterSpellBookView.xaml
	/// </summary>
	public partial class CharacterSpellBookView : UserControl
	{
		private readonly ICommand focusSearchCommand;

		public CharacterSpellBookView()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
			delKeyBinding.Command = new RelayCommand(DeleteItem);
		}

		public void FocusSearch()
		{
			this.searchBox.Focus();
		}

		private void DeleteItem()
		{
			CharacterSpellBookViewModel? vm = DataContext as CharacterSpellBookViewModel;

			if (vm is null)
				return;

			vm.DeleteSpellCommand.Execute(null);
			vm.DeleteCantripCommand.Execute(null);
		}
	}
}
