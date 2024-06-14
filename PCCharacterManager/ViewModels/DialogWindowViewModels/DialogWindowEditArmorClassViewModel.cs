using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
    public class DialogWindowEditArmorClassViewModel : ObservableObject
    {
		private int _armor;
		public int Armor
		{
			get
			{
				return _armor;
			}
			set
			{
				OnPropertyChanged(ref _armor, value);
			}
		}

		private int _shild;
		public int Shild
		{
			get
			{
				return _shild;
			}
			set
			{
				OnPropertyChanged(ref _shild, value);
			}
		}

		private int _misc;
		public int Misc
		{
			get
			{
				return _misc;
			}
			set
			{
				OnPropertyChanged(ref _misc, value);
			}
		}

		private int _temp;
		public int Temp
		{
			get
			{
				return _temp;
			}
			set
			{
				OnPropertyChanged(ref _temp, value);
			}
		}

		public DialogWindowEditArmorClassViewModel(ArmorClass armorClass)
		{
			_armor = armorClass.Armor;
			_shild = armorClass.Shild;
			_misc = armorClass.Misc;
			_temp = armorClass.Temp;
		}
    }
}
