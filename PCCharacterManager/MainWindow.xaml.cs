using PCCharacterManager.DialogWindows;
using PCCharacterManager.Helpers;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System.Windows;

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
			DialogServiceBase.RegisterDialog<DialogWindowListViewSelectItemViewModel, SelectStringValueDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowStringInputViewModel, StringInputDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowShortRestViewModel, ShortRestDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowEditArmorClassViewModel, EditArmorClassDialogWindow>();
			DialogServiceBase.RegisterDialog<DialogWindowEditDarkSoulsCharacterViewModel, EditDarkSoulsCharacterDialogWindow>();
			InitializeComponent();
		}

		private void DarkMode_Click(object sender, RoutedEventArgs e)
		{
			ThemeChanger.ChangeTheme(ThemeChanger.DarkMode);
		}

		private void LightMode_Click(object sender, RoutedEventArgs e)
		{
			ThemeChanger.ChangeTheme(ThemeChanger.LightMode);
		}
	}
}
