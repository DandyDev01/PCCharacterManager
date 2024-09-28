using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for EditCharacterDialogWindow.xaml
	/// </summary>
	public partial class EditCharacterDialogWindow : Window
	{
		public EditCharacterDialogWindow()
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
