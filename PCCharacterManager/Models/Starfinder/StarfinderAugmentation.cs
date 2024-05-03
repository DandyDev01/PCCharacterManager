using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public enum AugmentationCategory { BIOTECH, CYBERNETICS, MAGITECH, NECROGRAFTS, PERSONAL_UPGRADES, SPECIES_GRAFTS };
	public enum AugmentationSystem 
	{ 
		BRAIN, THROAT, ARM_LEFT, ARM_RIGHT, LEG_LEFT, LEG_RIGHT,
		LUNGS, HAND_LEFT, HAND_RIGHT, ARMS_ALL, LEGS_ALL, HANDS_ALL,
		EYES_ALL, EYE_LEFT, EYE_RIGHT, SPINAL_COLUMN, FEET_LEFT, FEET_RIGHT,
		FEET_ALL, SKIN, HEART, EARS_ALL, EAR_LEFT, EAR_RIGHT 
	}

	public class StarfinderAugmentation : ObservableObject
	{
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				OnPropertyChanged(ref _name, value);
			}
		}

		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				OnPropertyChanged(ref _description, value);
			}
		}

		private string _level;
		public string Level
		{
			get
			{
				return _level;
			}
			set
			{
				OnPropertyChanged(ref _level, value);
			}
		}

		private string _price;
		public string Price
		{
			get
			{
				return _price;
			}
			set
			{
				OnPropertyChanged(ref _price, value);
			}
		}

		private AugmentationCategory _category;
		public AugmentationCategory Category
		{
			get
			{
				return _category;
			}
			set
			{
				OnPropertyChanged(ref _category, value);
			}
		}

		private string[] _systems;
		public string[] Systems
		{
			get
			{
				return _systems;
			}
			set
			{
				OnPropertyChanged(ref _systems, value);
			}
		}

		public StarfinderAugmentation()
		{
			_name = string.Empty;
			_description = string.Empty;
			_level = string.Empty;
			_price = string.Empty;
			_category = AugmentationCategory.BIOTECH;
			_systems = Array.Empty<string>();
		}
	}
}
