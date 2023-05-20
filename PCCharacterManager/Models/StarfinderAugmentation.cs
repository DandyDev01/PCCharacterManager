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
		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				OnPropertyChanged(ref name, value);
			}
		}

		private string description;
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				OnPropertyChanged(ref description, value);
			}
		}

		private string level;
		public string Level
		{
			get
			{
				return level;
			}
			set
			{
				OnPropertyChanged(ref level, value);
			}
		}

		private string price;
		public string Price
		{
			get
			{
				return price;
			}
			set
			{
				OnPropertyChanged(ref price, value);
			}
		}

		private AugmentationCategory category;
		public AugmentationCategory Category
		{
			get
			{
				return category;
			}
			set
			{
				OnPropertyChanged(ref category, value);
			}
		}

		private AugmentationSystem[] systems;
		public AugmentationSystem[] Systems
		{
			get
			{
				return systems;
			}
			set
			{
				OnPropertyChanged(ref systems, value);
			}
		}

		public StarfinderAugmentation()
		{
			name = string.Empty;
			description = string.Empty;
			level = string.Empty;
			price = string.Empty;
			category = AugmentationCategory.BIOTECH;
			systems = Array.Empty<AugmentationSystem>();
		}
	}
}
