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
	/// Interaction logic for StringInputDialogWindow.xaml
	/// </summary>
	public partial class StringInputDialogWindow : Window
	{
		public StringInputDialogWindow()
		{
			InitializeComponent();
			inputBox.Focus();
			inputBox.TextChanged += SetIndex;
		}

		private void SetIndex(object sender, TextChangedEventArgs e)
		{
			inputBox.CaretIndex = inputBox.Text.Length;
		}
	}
}
