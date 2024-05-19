using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Helpers
{
    static class ThemeChanger
    {
		public readonly static string DarkMode = "Themes/DarkMode.xaml";
		public readonly static string LightMode = "Themes/LightMode.xaml";

		public static void ChangeTheme(string uri)
		{
			ResourceDictionary theme = new ResourceDictionary();
			theme.Source = new Uri(uri, UriKind.Relative);

			App.Current.Resources.Clear();
			App.Current.Resources.MergedDictionaries.Add(theme);
		}
    }
}
