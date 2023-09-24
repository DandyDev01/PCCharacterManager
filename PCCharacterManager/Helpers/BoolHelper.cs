using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Helpers
{
    public class BoolHelper : ObservableObject
    {
		private bool value;
		public bool Value
		{
			get
			{
				return value;
			}
			set
			{
				OnPropertyChanged(ref this.value, value);
			}
		}

		public BoolHelper(bool _value)
		{
			value = _value;
		}

	}
}
