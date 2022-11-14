using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public abstract class ItemViewModel : ObservableObject
	{
		protected Item boundItem;
		public Item BoundItem
		{
			get { return boundItem; }
			set { OnPropertyChaged(ref boundItem, value); }
		}

		protected bool isEditMode;
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

		protected string displayName;
		public string DisplayName
		{
			get { return displayName; }
			set
			{
				boundItem.Name = value;
				OnPropertyChaged(ref displayName, value);
			}
		}

		protected string displayDesc;
		public string DisplayDesc
		{
			get { return displayDesc; }
			set
			{
				boundItem.Desc = value;
				OnPropertyChaged(ref displayDesc, value);
			}
		}

		protected int displayQuantity;
		public int DisplayQuantity
		{
			get { return displayQuantity; }
			set
			{
				BoundItem.Quantity = value;
				OnPropertyChaged(ref displayQuantity, value);
			}
		}
	}
}
