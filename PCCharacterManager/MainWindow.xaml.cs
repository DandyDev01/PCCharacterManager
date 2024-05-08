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
			DialogServiceBase.RegisterDialog<DialogWindowCharacterCreaterViewModel, CreateCharacterDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowAddAugmentationViewModel, AddAugmentationDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowAddConditionViewModel, AddConditionDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowAddFeatureViewModel, AddFeatureDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowAddItemViewModel, AddItemDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowAddSpellViewModel, AddSpellDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowChangeHealthViewModel, ChangeHealthDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowCharacterCreaterViewModel, CreateCharacterDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowEditCharacterViewModel, EditCharacterDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowDnD5eCharacterLevelupViewModel, DnD5eLevelupCharacterDialogWindow>();
			//DialogService.RegisterDialog<DialogWindowSelectAbilityViewModel, SelectAbilityToIncreaseScoreDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowSelectStingValueViewModel, SelectStringValueDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowStringInputViewModel, StringInputDialogWindow>();
			InitializeComponent();
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
