using PCCharacterManager.DialogWindows;
using PCCharacterManager.Helpers;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCCharacterManager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DialogService.RegisterDialog<DialogWindowAddAugmentationViewModel, AddAugmentationDialogWindow>();
			DialogService.RegisterDialog<DialogWindowAddConditionViewModel, AddConditionDialogWindow>();
			DialogService.RegisterDialog<DialogWindowAddFeatureViewModel, AddFeatureDialogWindow>();
			DialogService.RegisterDialog<DialogWindowAddItemViewModel, AddItemDialogWindow>();
			DialogService.RegisterDialog<DialogWindowAddSpellViewModel, AddSpellDialogWindow>();
			DialogService.RegisterDialog<DialogWindowChangeHealthViewModel, ChangeHealthDialogWindow>();
			DialogService.RegisterDialog<DialogWindowCharacterCreaterViewModel, CreateCharacterDialogWindow>();
			DialogService.RegisterDialog<DialogWindowEditCharacterViewModel, EditCharacterDialogWindow>();
			//DialogService.RegisterDialog<DialogWindowSelectAbilityViewModel, SelectAbilityToIncreaseScoreDialogWindow>();
			DialogService.RegisterDialog<DialogWindowSelectStingValueViewModel, SelectStringValueDialogWindow>();
			DialogService.RegisterDialog<DialogWindowStringInputViewModel, StringInputDialogWindow>();
		}

		private void DarkMode_Click(object sender, RoutedEventArgs e)
		{
			ThemeChanger.ChangeTheme("Themes/DarkMode.xaml");
		}

		private void LightMode_Click(object sender, RoutedEventArgs e)
		{
			ThemeChanger.ChangeTheme("Themes/LightMode.xaml");
		}
	}
}
