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
		}

		private void DarkMode_Click(object sender, RoutedEventArgs e)
		{
			ResourceDictionary theme = new ResourceDictionary();
			theme.Source = new Uri("Themes/DarkMode.xaml", UriKind.Relative);

			App.Current.Resources.Clear();
			App.Current.Resources.MergedDictionaries.Add(theme);
		}

		private void LightMode_Click(object sender, RoutedEventArgs e)
		{
			ResourceDictionary theme = new ResourceDictionary();
			theme.Source = new Uri("Themes/LightMode.xaml", UriKind.Relative);

			App.Current.Resources.Clear();
			App.Current.Resources.MergedDictionaries.Add(theme);
		}
	}
}
