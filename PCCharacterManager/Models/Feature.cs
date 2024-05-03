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
			get => _property.Name;
			set => _property.Name = value;
		}
		
		public string Description
		{
			get => _property.Desc;
			set => _property.Desc = value;
		}
		
		private string _featureType;
		public string FeatureType
		{
			get
			{
				return _featureType;
			}
			set
			{
				OnPropertyChanged(ref _featureType, value);
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

		private readonly Property _property;

		public Feature(Property _property, string _featureType, string _level)
		{
			this._property = _property;
			this._featureType = _featureType;
			this._level = _level;
		}
	}
}
