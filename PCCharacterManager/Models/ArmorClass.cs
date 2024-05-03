using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class ArmorClass : ObservableObject
	{
		private string _armorClassValue = string.Empty;
		public string ArmorClassValue
		{
			get
			{
				return _armorClassValue;
			}
			set
			{
				OnPropertyChanged(ref _armorClassValue, value);
			}
		}
	}
}
