using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class ItemEditableViewModel : ItemViewModel
	{
		private readonly PropertyEditableVMPool propertyVMPool;

		public ObservableCollection<PropertyEditableViewModel> DisplayProperties { get; }	

		public ICommand EditCommand { get; private set; }
		public ICommand RemoveCommand { get; private set; }
		public ICommand AddPropertyCommand { get; private set; }
		public ICommand RemovePropertyCommand { get; private set; }

		private PropertyEditableViewModel? selectedProperty;
		public PropertyEditableViewModel? SelectedProperty
		{
			get { return selectedProperty; }
			set
			{
				OnPropertyChaged(ref selectedProperty, value);
			}
		}

		public ItemEditableViewModel(Item _item, PropertyEditableVMPool _propertyVMPool)
		{
			propertyVMPool = _propertyVMPool;
			boundItem = _item;

			displayQuantity = boundItem.Quantity;
			displayDesc = boundItem.Desc;
			displayName = boundItem.Name;
			DisplayProperties = new ObservableCollection<PropertyEditableViewModel>();

			EditCommand = new RelayCommand(Edit);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);
			RemoveCommand = new RelayCommand(Remove);

			foreach (var property in boundItem.Properties)
			{
				PropertyEditableViewModel temp = propertyVMPool.GetItem();
				temp.Bind(property);
				temp.IsEditMode = IsEditMode;
				DisplayProperties.Add(temp);
			}
		}

		public ItemEditableViewModel(PropertyEditableVMPool _propertyVMPool)
		{
			propertyVMPool = _propertyVMPool;

			displayQuantity = 0;
			displayDesc = string.Empty;
			displayName = string.Empty;

			EditCommand = new RelayCommand(Edit);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);
			RemoveCommand = new RelayCommand(Remove);
			DisplayProperties = new ObservableCollection<PropertyEditableViewModel>();
		}

		public void Bind(Item item)
		{
			DisplayProperties.Clear();
			BoundItem = item;
			displayQuantity = item.Quantity;
			displayDesc = item.Desc;
			displayName = item.Name;
			foreach (Property property in item.Properties)
			{
				PropertyEditableViewModel temp = propertyVMPool.GetItem();
				temp.Bind(property);
				DisplayProperties.Add(temp);
			}
		}

		private void AddProperty()
		{
			PropertyEditableViewModel temp = propertyVMPool.GetItem();

			Property newProperty = new Property()
			{
				Name = "name",
				Desc = "desc"
			};
			newProperty.Hidden = IsEditMode;
			temp.Bind(newProperty);

			BoundItem.AddProperty(newProperty);
			DisplayProperties.Add(temp);
		}

		private void RemoveProperty()
		{
			if (SelectedProperty == null) return;

			BoundItem.RemoveProperty(SelectedProperty.BoundProperty);
			DisplayProperties.Remove(SelectedProperty);
			propertyVMPool.Return(SelectedProperty);
		}

		private void Remove()
		{
			throw new NotImplementedException();
		}

		public void Edit()
		{

			IsEditMode = !isEditMode;

			foreach (var property in DisplayProperties)
			{
				property.IsEditMode = IsEditMode;
			}

		}
	}
}
