using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.ViewModels
{
	public class DnDClassFeatureListViewModel : PropertyListViewModel
	{
		public new ObservableCollection<DnD5eCharacterClassFeature> ItemsToDisplay { get; }

		public DnDClassFeatureListViewModel(string _listName, ObservableCollection<DnD5eCharacterClassFeature> _item) : base(_listName)
		{
			ItemsToDisplay = _item;
			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
		}

		private void AddItem()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(window,
				"Name");
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			Window window1 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel(window1,
				"Description");
			window1.DataContext = windowVM1;
			window1.ShowDialog();

			if (window1.DialogResult == false)
				return;

			Window window2 = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM2 = new DialogWindowStringInputViewModel(window2, "Feature Level");
			window2.DataContext = windowVM2;
			window2.ShowDialog();

			if (window2.DialogResult == false)
				return;

			int level = 0;
			try
			{
				level = int.Parse(windowVM2.Answer);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return;
			}
			DnD5eCharacterClassFeature feature =
				new DnD5eCharacterClassFeature(windowVM.Answer, windowVM1.Answer, level);
			ItemsToDisplay.Add(feature);
		}

		private void RemoveItem()
		{
			ItemsToDisplay.Remove(SelectedItem as DnD5eCharacterClassFeature);
		}

		private void EditItem()
		{
			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel(window, "Edit " + SelectedItem.Name);

			windowVM.Answer = SelectedItem.Desc;
			window.DataContext = windowVM;
			window.ShowDialog();

			if (window.DialogResult == false)
				return;

			SelectedItem.Desc = windowVM.Answer;
		}
	}
}
