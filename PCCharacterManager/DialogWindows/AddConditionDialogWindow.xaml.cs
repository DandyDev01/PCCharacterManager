using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for AddConditionDialogWindow.xaml
	/// </summary>
	public partial class AddConditionDialogWindow : Window
	{
		public AddConditionDialogWindow()
		{
			InitializeComponent();
		}

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
