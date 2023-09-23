using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Feature : ObservableObject
	{
		public string Name
		{
			get => property.Name;
			set => property.Name = value;
		}
		
		public string Description
		{
			get => property.Desc;
			set => property.Desc = value;
		}
		
		private string featureType;
		public string FeatureType
		{
			get
			{
				return featureType;
			}
			set
			{
				OnPropertyChanged(ref featureType, value);
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

		private readonly Property property;

		public Feature(Property _property, string _featureType, string _level)
		{
			property = _property;
			featureType = _featureType;
			level = _level;
		}
	}
}
