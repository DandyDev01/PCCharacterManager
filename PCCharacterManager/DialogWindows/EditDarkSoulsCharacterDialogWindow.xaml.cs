﻿using System;
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
