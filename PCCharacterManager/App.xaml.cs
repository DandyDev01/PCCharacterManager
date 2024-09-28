using PCCharacterManager.Helpers;
using System.Windows;

namespace PCCharacterManager
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private App()
		{
			ThemeChanger.ChangeTheme("Themes/LightMode.xaml");
		}
	}
}
