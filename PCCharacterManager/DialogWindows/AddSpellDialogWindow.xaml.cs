using PCCharacterManager.ViewModels;
using System.Windows;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for AddSpellDialogWindow.xaml
	/// </summary>
	public partial class AddSpellDialogWindow : Window
	{
		public AddSpellDialogWindow()
		{
			InitializeComponent();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Add_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowAddSpellViewModel vm = DataContext as DialogWindowAddSpellViewModel;

			if (vm is not null)
			{
				vm.AddNewSpell();
				DialogResult = true;
			}

			Close();
		}
	}
}
