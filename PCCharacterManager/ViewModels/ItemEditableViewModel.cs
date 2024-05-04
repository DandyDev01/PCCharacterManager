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
		private readonly PropertyEditableVMPool _propertyVMPool;

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
				OnPropertyChanged(ref selectedProperty, value);
			}
		}

		public ItemEditableViewModel(Item item, PropertyEditableVMPool propertyVMPool)
		{
			_propertyVMPool = propertyVMPool;
			_boundItem = item;

			_displayQuantity = _boundItem.Quantity;
			_displayDesc = _boundItem.Desc;
			_displayName = _boundItem.Name;
			DisplayProperties = new ObservableCollection<PropertyEditableViewModel>();

			EditCommand = new RelayCommand(Edit);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);
			RemoveCommand = new RelayCommand(Remove);

			foreach (var property in _boundItem.Properties)
			{
				PropertyEditableViewModel temp = this._propertyVMPool.GetItem();
				temp.Bind(property);
				temp.IsEditMode = IsEditMode;
				DisplayProperties.Add(temp);
			}
		}

		public ItemEditableViewModel(PropertyEditableVMPool _propertyVMPool)
		{
			this._propertyVMPool = _propertyVMPool;

			_displayQuantity = 0;
			_displayDesc = string.Empty;
			_displayName = string.Empty;

			EditCommand = new RelayCommand(Edit);
			AddPropertyCommand = new RelayCommand(AddProperty);
			RemovePropertyCommand = new RelayCommand(RemoveProperty);
			RemoveCommand = new RelayCommand(Remove);
			DisplayProperties = new ObservableCollection<PropertyEditableViewModel>();
		}

		private void AddProperty()
		{
			if (BoundItem == null)
				return;

			PropertyEditableViewModel propertyItem = _propertyVMPool.GetItem();

			Property newProperty = new Property()
			{
				Name = "name",
				Desc = "desc"
			};
			newProperty.Hidden = IsEditMode;
			propertyItem.Bind(newProperty);


			BoundItem.AddProperty(newProperty);
			DisplayProperties.Add(propertyItem);
		}

		private void RemoveProperty()
		{
			if (SelectedProperty == null || BoundItem == null) 
				return;

			BoundItem.RemoveProperty(SelectedProperty.BoundProperty);
			DisplayProperties.Remove(SelectedProperty);
			_propertyVMPool.Return(SelectedProperty);
		}

		private void Remove()
		{
			throw new NotImplementedException();
		}

		public void Edit()
		{

			IsEditMode = !_isEditMode;

			foreach (var property in DisplayProperties)
			{
				property.IsEditMode = IsEditMode;
			}

		}
	}
}
