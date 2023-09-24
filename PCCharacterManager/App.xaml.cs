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
			ResourceDictionary theme = new ResourceDictionary();
			theme.Source = new Uri("Themes/DarkMode.xaml", UriKind.Relative);

			App.Current.Resources.Clear();
			App.Current.Resources.MergedDictionaries.Add(theme);
		}
	}
}
