using PCCharacterManager.ViewModels.DialogWindowViewModels;
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
    /// Interaction logic for ChangeHealthDialogWindow.xaml
    /// </summary>
    public partial class ChangeHealthDialogWindow : Window
    {
        public ChangeHealthDialogWindow()
        {
            InitializeComponent();
			inputBox.Focus();
        }

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowChangeHealthViewModel vm = DataContext as DialogWindowChangeHealthViewModel;

			if (vm is null)
				return;

			try
			{
				vm.Amount = int.Parse(vm.Answer);
			}
			catch
			{
				MessageBox.Show("value must be a whole number", "invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			DialogResult = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
