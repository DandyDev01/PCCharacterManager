﻿using PCCharacterManager.Utility;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for CharacterInventoryView.xaml
	/// </summary>
	public partial class CharacterInventoryView : UserControl
	{
		private ICommand focusSearchCommand;

		public CharacterInventoryView()
		{
			InitializeComponent();
			focusSearchCommand = new RelayCommand(FocusSearch);
			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.F, ModifierKeys.Control));
		}

		public void FocusSearch()
		{
			this.searchBox.Focus();
		}
	}
}
