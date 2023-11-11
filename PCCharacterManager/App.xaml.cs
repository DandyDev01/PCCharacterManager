using PCCharacterManager.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
