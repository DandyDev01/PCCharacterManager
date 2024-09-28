using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for EditDarkSoulsCharacterDialogWindow.xaml
	/// </summary>
	public partial class EditDarkSoulsCharacterDialogWindow : Window
	{
		public EditDarkSoulsCharacterDialogWindow()
		{
			InitializeComponent();
		}


		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
