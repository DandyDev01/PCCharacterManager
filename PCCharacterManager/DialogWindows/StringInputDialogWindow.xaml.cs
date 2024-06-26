﻿using PCCharacterManager.ViewModels;
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
			
		}

		private void inputBox_GotFocus(object sender, RoutedEventArgs e)
		{
			inputBox.CaretIndex = inputBox.Text.Length;
		}

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowStringInputViewModel vm = DataContext as DialogWindowStringInputViewModel;

			if (vm is not null)
				vm.Answer = string.Empty;

			Close();
		}
	}
}
