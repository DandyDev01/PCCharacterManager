using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class ItemEditableViewModel : ObservableObject
	{
		private Item boundItem;
		public Item BoundItem
		{
			get { return boundItem; }
			set { OnPropertyChaged(ref boundItem, value); }
		}

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				OnPropertyChaged(ref isEditMode, value);
				OnPropertyChaged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		private string displayName;
		public string DisplayName
		{
			get { return displayName; }
			set
			{
				boundItem.Name = value;
				OnPropertyChaged(ref displayName, value);
			}
		}

		private string displayDesc;
		public string DisplayDesc
		{
			get { return displayDesc; }
			set
			{
				boundItem.Desc = value;
				OnPropertyChaged(ref displayDesc, value);
			}
		}

		private int displayQuantity;
		public int DisplayQuantity
		{
			get { return displayQuantity; }
			set
			{
				BoundItem.Quantity = value;
				OnPropertyChaged(ref displayQuantity, value);
			}
		}

		private List<PropertyEditableViewModel> properties;
		public List<PropertyEditableViewModel> Properties
		{
			get { return properties; }
			set { OnPropertyChaged(ref properties, value); }
		}

		public Action<Item> RemoveAction { get; set; }

		public ICommand EditCommand { get; private set; }
		public ICommand RemoveCommand { get; private set; }
		public ICommand AddPropertyCommand { get; private set; }
		public ICommand RemovePropertyCommand { get; private set; }

		private PropertyEditableViewModel selectedProperty;
		public PropertyEditableViewModel SelectedProperty
		{
			get { return selectedProperty; }
			set
			{
				OnPropertyChaged(ref selectedProperty, value);
			}
		}

		public ItemEditableViewModel(Item _item)
		{
			boundItem = _item;

			displayQuantity = boundItem.Quantity;
			displayDesc = boundItem.Desc;
			displayName = boundItem.Name;

			EditCommand = new RelayCommand(Edit);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);
			RemoveCommand = new RelayCommand(Remove);
			Properties = new List<PropertyEditableViewModel>();
			PopulateProperties();
		}

		private void AddProperty()
		{

			BoundItem.AddProperty(new Property()
			{
				Name = "",
				Desc = ""
			});

			PopulateProperties();
		}

		private void RemoveProperty()
		{
			BoundItem.RemoveProperty(SelectedProperty.BoundProperty);

			PopulateProperties();
		}

		private void PopulateProperties()
		{

			if (BoundItem == null) return;
			if (BoundItem.Properties == null) return;

			Properties = new List<PropertyEditableViewModel>();

			foreach (var property in BoundItem.Properties)
			{
				PropertyEditableViewModel temp = new PropertyEditableViewModel(property);
				temp.IsEditMode = IsEditMode;
				Properties.Add(temp);
			}
		}

		private void Remove()
		{
			RemoveAction?.Invoke(BoundItem);
		}

		public void Edit()
		{

			IsEditMode = !isEditMode;

			foreach (var property in Properties)
			{
				property.IsEditMode = IsEditMode;
			}

		}
	}
}
