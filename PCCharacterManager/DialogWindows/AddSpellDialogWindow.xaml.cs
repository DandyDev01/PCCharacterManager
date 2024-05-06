using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
