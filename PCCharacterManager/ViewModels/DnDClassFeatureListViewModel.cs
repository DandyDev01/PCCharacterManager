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
			Window featureNameWindow = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel(featureNameWindow,
				"Name");
			featureNameWindow.DataContext = windowVM;
			featureNameWindow.ShowDialog();

			if (featureNameWindow.DialogResult == false)
				return;

			Window featureDescriptionWindow = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel(featureDescriptionWindow,
				"Description");
			featureDescriptionWindow.DataContext = windowVM1;
			featureDescriptionWindow.ShowDialog();

			if (featureDescriptionWindow.DialogResult == false)
				return;

			Window featureLevelWindow = new StringInputDialogWindow();
			DialogWindowStringInputViewModel windowVM2 = new DialogWindowStringInputViewModel(featureLevelWindow, "Feature Level");
			featureLevelWindow.DataContext = windowVM2;
			featureLevelWindow.ShowDialog();

			if (featureLevelWindow.DialogResult == false)
				return;

			int level;
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
			if (SelectedItem == null)
				return;

			if (SelectedItem is DnD5eCharacterClassFeature feature)
				ItemsToDisplay.Remove(feature);
		}

		private void EditItem()
		{
			if (SelectedItem == null)
				return;

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
