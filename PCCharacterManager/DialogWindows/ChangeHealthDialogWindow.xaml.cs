using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for ChangeHealthDialogWindow.xaml
	/// </summary>
	public partial class ChangeHealthDialogWindow : Window
	{
		public ChangeHealthDialogWindow()
		{
			InitializeComponent();
			inputBox.Focus();
		}

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowChangeHealthViewModel vm = DataContext as DialogWindowChangeHealthViewModel;

			if (vm is null)
				return;

			try
			{
				vm.Amount = int.Parse(vm.Answer);
			}
			catch
			{
				MessageBox.Show("value must be a whole number", "invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			DialogResult = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
