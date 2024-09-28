using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for DnD5eLevelupCharacterDialogWindow.xaml
	/// </summary>
	public partial class DnD5eLevelupCharacterDialogWindow : Window
	{
		public DnD5eLevelupCharacterDialogWindow()
		{
			InitializeComponent();
		}

		private void Levelup_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			var dataContext = DataContext as DialogWindowDnD5eCharacterLevelupViewModel;

			if (dataContext is not null && dataContext.HasAddedClass)
				dataContext.RemoveAddedClasses();

			Close();
		}
	}
}
