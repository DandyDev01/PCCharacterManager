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

		private string abilityScoreImprovement;
		private StarfinderThemeData themeData;

		public string AbilityScoreImprovement
		{
			get
			{
				return abilityScoreImprovement;
			}
			set
			{
				OnPropertyChanged(ref abilityScoreImprovement, value);
			}
		}

		public ObservableCollection<Property> Features { get; }

		public StarfinderTheme()
		{
			name = string.Empty;
			abilityScoreImprovement = string.Empty;
			Features = new ObservableCollection<Property>();
		}

		public StarfinderTheme(string _name, string _abilityScoreImprovement)
		{
			name = _name;
			abilityScoreImprovement = _abilityScoreImprovement;
			Features = new ObservableCollection<Property>();
		}

		public StarfinderTheme(StarfinderThemeData themeData)
		{
			this.themeData = themeData;
		}
	}
}
