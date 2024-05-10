using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
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
		private readonly DialogServiceBase _dialogService;

		public new ObservableCollection<DnD5eCharacterClassFeature> ItemsToDisplay { get; }

		public DnDClassFeatureListViewModel(string _listName, ObservableCollection<DnD5eCharacterClassFeature> _item, 
			DialogServiceBase dialogService) : base(_listName, dialogService)
		{
			ItemsToDisplay = _item;
			AddItemCommand = new RelayCommand(AddItem);
			RemoveItemCommand = new RelayCommand(RemoveItem);
			EditItemCommand = new RelayCommand(EditItem);
			_dialogService = dialogService;
		}

		private void AddItem()
		{
			DialogWindowStringInputViewModel windowVM = new DialogWindowStringInputViewModel("Name");
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;


			DialogWindowStringInputViewModel windowVM1 = new DialogWindowStringInputViewModel("Description");
			result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM1, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;


			DialogWindowStringInputViewModel windowVM2 = new DialogWindowStringInputViewModel("Feature Level");
			result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM2, r =>
			{
				result = r;
			});

			if (result == false.ToString())
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

			DialogWindowStringInputViewModel windowVM =
				new DialogWindowStringInputViewModel("Edit " + SelectedItem.Name);

			windowVM.Answer = SelectedItem.Desc;
			string result = string.Empty;
			_dialogService.ShowDialog<StringInputDialogWindow, DialogWindowStringInputViewModel>(windowVM, r =>
			{
				result = r;
			});

			if (result == false.ToString())
				return;


			SelectedItem.Desc = windowVM.Answer;
		}
	}
}
