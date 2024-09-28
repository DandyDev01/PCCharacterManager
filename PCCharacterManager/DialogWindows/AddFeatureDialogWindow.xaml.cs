using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for AddFeatureDialogWindow.xaml
	/// </summary>
	public partial class AddFeatureDialogWindow : Window
	{
		public AddFeatureDialogWindow()
		{
			InitializeComponent();
		}

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowAddFeatureViewModel vm = DataContext as DialogWindowAddFeatureViewModel;

			if (vm is not null)
			{
				vm.Ok();
				DialogResult = true;
			}

			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
