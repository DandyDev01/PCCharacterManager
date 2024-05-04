using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class PropertyEditableViewModel : ObservableObject
	{
		private Property _boundProperty;
		public Property BoundProperty
		{
			get { return _boundProperty; }
			set { OnPropertyChanged(ref _boundProperty, value); }
		}

		private string _displayName;
		public string DisplayName
		{
			get { return _displayName; }
			set
			{ 
				OnPropertyChanged(ref _displayName, value); 
				_boundProperty.Name = value;
			}
		}

		private string _displayDesc;
		public string DisplayDesc
		{
			get { return _displayDesc; }
			set
			{
				OnPropertyChanged(ref _displayDesc, value);
				_boundProperty.Desc = value;
			}
		}

		private bool _displayHidden;
		public bool DisplayHidden
		{
			get { return _displayHidden; }
			set
			{
				OnPropertyChanged(ref _displayHidden, value);
				_boundProperty.Hidden = value;
			}
		}

		private bool _isEditMode;
		public bool IsEditMode
		{
			get { return _isEditMode; }
			set
			{
				OnPropertyChanged(ref _isEditMode, value);
				OnPropertyChanged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !_isEditMode; }
		}

		public PropertyEditableViewModel(Property property)
		{
			_boundProperty = property;
			_displayDesc = property.Desc;
			_displayName = property.Name;
			_displayHidden = property.Hidden;
			IsEditMode = false;
		}

		public PropertyEditableViewModel() 
		{
			_displayName = string.Empty;
			_displayDesc = string.Empty;
			_displayHidden = false;
			_boundProperty = new Property();
		}

		public void Bind(Property property)
		{
			_boundProperty = property;
			DisplayDesc = property.Desc;
			DisplayName = property.Name;
			DisplayHidden = property.Hidden;
			IsEditMode = property.Hidden;
		}

		public void Edit()
		{
			IsEditMode = !_isEditMode;
		}
	}
}
