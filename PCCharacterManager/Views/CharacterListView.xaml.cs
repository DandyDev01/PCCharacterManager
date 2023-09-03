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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for CharacterListView.xaml
	/// </summary>
	public partial class CharacterListView : UserControl
	{
		public CharacterListView()
		{
			InitializeComponent();
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ListView.SelectedItem is not CharacterItemViewModel characterItemVM)
				return;

			while (ListView.Items.Count < 1)
			{
				if (DataContext is not CharacterListViewModel characterListVM)
					return;

				characterListVM.CreateCharacterCommand.Execute(null);
			}

			if (ListView.Items[0] is not CharacterItemViewModel firstCharacterItemVM)
				return;

			characterItemVM = firstCharacterItemVM;

			characterItemVM.SelectCharacterCommand?.Execute(null);
		}
	}
}
