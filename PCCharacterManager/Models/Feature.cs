using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Feature : Property
	{
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
		
		public Feature(string _name, string _desc, string _featureType, string _level) : base(_name, _desc)
		{
			featureType = _featureType;
			level = _level;
		}
	}
}
