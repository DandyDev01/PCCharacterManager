using PCCharacterManager.ViewModels;
using System.Windows;

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
			DialogWindowAddAugmentationViewModel? vm = DataContext as DialogWindowAddAugmentationViewModel;

			if (vm is null)
				return;

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
