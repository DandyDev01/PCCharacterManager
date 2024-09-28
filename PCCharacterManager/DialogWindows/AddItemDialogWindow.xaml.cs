using PCCharacterManager.Utility;
using System.Windows;
using System.Windows.Input;

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

		private void Add_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			//DialogWindowAddItemViewModel vm = DataContext as DialogWindowAddItemViewModel;

			//if (vm is not null)
			//{
			//	vm.InventoryVM.ReturnItemVMsToPool();
			//}

			DialogResult = false;
		}
	}
}
