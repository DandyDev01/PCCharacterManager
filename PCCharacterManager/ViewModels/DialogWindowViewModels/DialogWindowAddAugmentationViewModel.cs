using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowAddAugmentationViewModel : ObservableObject
	{
		private StarfinderAugmentation _augmentation;
		public StarfinderAugmentation Augmentation { get { return _augmentation; } }

		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				OnPropertyChanged(ref _name, value);
			}
		}

		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				OnPropertyChanged(ref _description, value);
			}
		}

		private string _level;
		public string Level
		{
			get
			{
				return _level;
			}
			set
			{
				OnPropertyChanged(ref _level, value);
			}
		}

		private string _price;
		public string Price
		{
			get
			{
				return _price;
			}
			set
			{
				OnPropertyChanged(ref _price, value);
			}
		}

		private AugmentationCategory _category;
		public AugmentationCategory Category
		{
			get
			{
				return _category;
			}
			set
			{
				OnPropertyChanged(ref _category, value);
			}
		}


		public Array AugmentationCategoriesToDisplay { get; } = Enum.GetValues(typeof(AugmentationCategory));
		public Array AugmentationSystemToDisplay = Enum.GetValues(typeof(AugmentationSystem));
		public ObservableCollection<ListViewSelectableItemViewModel> SelectableAugmentationSystems { get; private set; }

		public readonly List<string> systems;

		public DialogWindowAddAugmentationViewModel()
		{
			systems = new List<string>();
			_augmentation = new StarfinderAugmentation();
			_name = string.Empty;
			_description = string.Empty;
			_level = string.Empty;
			_price = string.Empty;
			SelectableAugmentationSystems = new ObservableCollection<ListViewSelectableItemViewModel>();
			foreach (AugmentationSystem system in AugmentationSystemToDisplay)
			{
				SelectableAugmentationSystems.Add(new ListViewSelectableItemViewModel(system.ToString().ToLower()));
			}
		}
	}
}
