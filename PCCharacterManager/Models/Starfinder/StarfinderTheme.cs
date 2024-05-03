using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class StarfinderTheme : ObservableObject
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

		private string _abilityScoreImprovement;
		private StarfinderThemeData _themeData;

		public string AbilityScoreImprovement
		{
			get
			{
				return _abilityScoreImprovement;
			}
			set
			{
				OnPropertyChanged(ref _abilityScoreImprovement, value);
			}
		}

		public ObservableCollection<Property> Features { get; }

		public StarfinderTheme()
		{
			_name = string.Empty;
			_abilityScoreImprovement = string.Empty;
			Features = new ObservableCollection<Property>();
			_themeData = new StarfinderThemeData();
		}

		public StarfinderTheme(string name, string abilityScoreImprovement)
		{
			_name = name;
			_abilityScoreImprovement = abilityScoreImprovement;
			Features = new ObservableCollection<Property>();
			_themeData = new StarfinderThemeData();
		}

		public StarfinderTheme(StarfinderThemeData themeData) : this()
		{
			_themeData = themeData;
		}
	}
}
