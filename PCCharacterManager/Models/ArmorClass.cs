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
		private string armorClassValue = string.Empty;
		public string ArmorClassValue
		{
			get
			{
				return armorClassValue;
			}
			set
			{
				OnPropertyChanged(ref armorClassValue, value);
			}
		}
	}
}
