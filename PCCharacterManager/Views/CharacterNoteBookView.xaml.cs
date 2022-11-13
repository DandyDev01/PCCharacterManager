using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
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
using PCCharacterManager.ViewModels;
using PCCharacterManager.Models;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for CharacterNoteBookView.xaml
	/// </summary>
	public partial class CharacterNoteBookView : UserControl
	{
		private ICommand focusSearchCommand;

		public CharacterNoteBookView()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
		}

		public void FocusSearch()
		{
			this.searchBox.Focus();
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			CharacterNoteBookViewModel? temp = DataContext as CharacterNoteBookViewModel;
			
			if (temp == null) return;

			if (treeView.SelectedItem is not Note) return;

			temp.SelectedNote = treeView.SelectedItem as Note;
		}
	}
}
