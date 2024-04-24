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
using System.Windows.Shapes;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for AddItemDialogWindow.xaml
	/// </summary>
	public partial class AddItemDialogWindow : Window
	{
		private ICommand focusSearchCommand;

		public AddItemDialogWindow()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
		}

		public void FocusSearch()
		{
			placeholderText.Visibility = Visibility.Collapsed;
			searchBox.Visibility = Visibility.Visible;
			this.searchBox.Focus();
		}


		private void searchBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (searchBox.Text == "")
			{
				placeholderText.Visibility = Visibility.Visible;
				searchBox.Visibility = Visibility.Collapsed;
			}
		}

		private void searchBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (searchBox.Text == "")
			{
				placeholderText.Visibility = Visibility.Collapsed;
				searchBox.Visibility = Visibility.Visible;
				FocusSearch();
			}
		}

	}
}
