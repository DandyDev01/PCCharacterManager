using PCCharacterManager.ViewModels;
using System.Windows.Controls;

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

			characterItemVM.SelectCharacterCommand?.Execute(null);
		}
	}
}
