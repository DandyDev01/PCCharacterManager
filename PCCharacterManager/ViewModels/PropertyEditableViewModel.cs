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
			set { OnPropertyChaged(ref boundProperty, value); }
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

		public PropertyEditableViewModel(Property _boundProperty)
		{
			boundProperty = _boundProperty;
			IsEditMode = false;
		}

		public void Edit()
		{
			IsEditMode = !isEditMode;
		}
	}
}
