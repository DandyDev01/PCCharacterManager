using PCCharacterManager.ViewModels;
using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for CreateCharacterDialogWindow.xaml
	/// </summary>
	public partial class CreateCharacterDialogWindow : Window
	{
		public CreateCharacterDialogWindow()
		{
			InitializeComponent();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Create_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowCharacterCreaterViewModel? vm = DataContext as DialogWindowCharacterCreaterViewModel;

			if (vm is not null)
			{
				vm.Create();
				DialogResult = true;
				DataContext = null;
			}

			Close();
		}
	}
}
