using PCCharacterManager.Utility;
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
		private ICommand focusSearchCommand;

		public CharacterSpellBookView()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
		}

		public void FocusSearch()
		{
			this.searchBox.Focus();
		}

		private void SpellComboBox_OnMouseEnter(Object sender, MouseEventArgs e)
		{
			spellComboBox.IsDropDownOpen = true;	
		}

		private void CantripComboBox_OnMouseEnter(Object sender, MouseEventArgs e)
		{
			cantripComboBox.IsDropDownOpen = true;
		}

		private void cantripComboBox_MouseLeave(object sender, MouseEventArgs e)
		{
			cantripComboBox.IsDropDownOpen= false;
		}

		private void spellComboBox_MouseLeave(object sender, MouseEventArgs e)
		{
			spellComboBox.IsDropDownOpen = false;
		}
	}
}
