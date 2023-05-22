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
		private StarfinderAugmentation augmentation;
		public StarfinderAugmentation Augmentation { get { return augmentation; } }

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				OnPropertyChanged(ref name, value);
			}
		}

		private string description;
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				OnPropertyChanged(ref description, value);
			}
		}

		private string level;
		public string Level
		{
			get
			{
				return level;
			}
			set
			{
				OnPropertyChanged(ref level, value);
			}
		}

		private string price;
		public string Price
		{
			get
			{
				return price;
			}
			set
			{
				OnPropertyChanged(ref price, value);
			}
		}

		private AugmentationCategory category;
		public AugmentationCategory Category
		{
			get
			{
				return category;
			}
			set
			{
				OnPropertyChanged(ref category, value);
			}
		}


		public Array AugmentationCategoriesToDisplay { get; } = Enum.GetValues(typeof(AugmentationCategory));
		public Array AugmentationSystemToDisplay = Enum.GetValues(typeof(AugmentationSystem));
		public ObservableCollection<ListViewSelectableItemViewModel> SelectableAugmentationSystems { get; private set; }

		private readonly Window window;
		private readonly List<AugmentationSystem> systems;
		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

		public DialogWindowAddAugmentationViewModel(Window _window)
		{
			window = _window;
			systems = new List<AugmentationSystem>();
			OkCommand = new RelayCommand(Ok);
			CancelCommand = new RelayCommand(Cancel);
			augmentation = new StarfinderAugmentation();
			name = string.Empty;
			description = string.Empty;
			level = string.Empty;
			price = string.Empty;
			SelectableAugmentationSystems = new ObservableCollection<ListViewSelectableItemViewModel>();
			foreach (AugmentationSystem system in AugmentationSystemToDisplay)
			{
				SelectableAugmentationSystems.Add(new ListViewSelectableItemViewModel(system.ToString().ToLower()));
			}
		}

		private void Cancel()
		{
			window.DialogResult = false;
			window.Close();
		}

		private void Ok()
		{
			augmentation.Name = name;
			augmentation.Description = description;
			augmentation.Level = level;
			augmentation.Price = price;
			augmentation.Category = Category;

			foreach (var item in SelectableAugmentationSystems)
			{
				if (item.IsSelected)
				{
					foreach (AugmentationSystem system in AugmentationSystemToDisplay)
					{
						if (system.ToString().ToLower() == item.BoundItem)
						{
							systems.Add(system);
						}
					}
				}
			}

			window.DialogResult = true;
			window.Close();
		}
	}
}
