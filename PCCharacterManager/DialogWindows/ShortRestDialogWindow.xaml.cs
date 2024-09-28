using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for ShortRestDialogWindow.xaml
	/// </summary>
	public partial class ShortRestDialogWindow : Window
	{
		public ShortRestDialogWindow()
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
