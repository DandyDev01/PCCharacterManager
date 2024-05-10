using PCCharacterManager.Models;
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
using System.Xml.Linq;

namespace PCCharacterManager.DialogWindows
{
	/// <summary>
	/// Interaction logic for AddAugmentationDialogWindow.xaml
	/// </summary>
	public partial class AddAugmentationDialogWindow : Window
	{
		public AddAugmentationDialogWindow()
		{
			InitializeComponent();
		}

		private void Ok_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogWindowAddAugmentationViewModel vm = DataContext as DialogWindowAddAugmentationViewModel;

			if (vm is not null)
			{
				vm.Augmentation.Name = vm.Name;
				vm.Augmentation.Description = vm.Description;
				vm.Augmentation.Level = vm.Level;
				vm.Augmentation.Price = vm.Price;
				vm.Augmentation.Category = vm.Category;

				foreach (var item in vm.SelectableAugmentationSystems)
				{
					if (item.IsSelected)
					{
						vm.systems.Add(item.BoundItem);
					}
				}
				vm.Augmentation.Systems = vm.systems.ToArray();

				DialogResult = true;
			}
			
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
