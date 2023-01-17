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
		private Property boundProperty;
		public Property BoundProperty
		{
			get { return boundProperty; }
			set { OnPropertyChanged(ref boundProperty, value); }
		}

		private string displayName;
		public string DisplayName
		{
			get { return displayName; }
			set
			{ 
				OnPropertyChanged(ref displayName, value); 
				boundProperty.Name = value;
			}
		}

		private string displayDesc;
		public string DisplayDesc
		{
			get { return displayDesc; }
			set
			{
				OnPropertyChanged(ref displayDesc, value);
				boundProperty.Desc = value;
			}
		}

		private bool displayHidden;
		public bool DisplayHidden
		{
			get { return displayHidden; }
			set
			{
				OnPropertyChanged(ref displayHidden, value);
				boundProperty.Hidden = value;
			}
		}

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				OnPropertyChanged(ref isEditMode, value);
				OnPropertyChaged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		public PropertyEditableViewModel(Property _property)
		{
			boundProperty = _property;
			displayDesc = _property.Desc;
			displayName = _property.Name;
			displayHidden = _property.Hidden;
			IsEditMode = false;
		}

		public PropertyEditableViewModel() 
		{
			displayName = string.Empty;
			displayDesc = string.Empty;
			displayHidden = false;
		}

		public void Bind(Property _property)
		{
			boundProperty = _property;
			DisplayDesc = _property.Desc;
			DisplayName = _property.Name;
			DisplayHidden = _property.Hidden;
			IsEditMode = _property.Hidden;
		}

		public void Edit()
		{
			IsEditMode = !isEditMode;
		}
	}
}
