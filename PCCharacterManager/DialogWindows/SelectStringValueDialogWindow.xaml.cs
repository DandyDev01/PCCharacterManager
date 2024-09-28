using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for SelectStringValueDialogWindow.xaml
	/// </summary>
	public partial class SelectStringValueDialogWindow : Window
	{
		public SelectStringValueDialogWindow()
		{
			InitializeComponent();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
