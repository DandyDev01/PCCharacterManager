using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.ViewModels
{
	public class StarfinderCharacterCreatorViewModel : ObservableObject
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

		private StarfinderRaceData selectedRaceData;
		public StarfinderRaceData SelectedRaceData
		{
			get
			{
				return selectedRaceData;
			}
			set
			{
				OnPropertyChanged(ref selectedRaceData, value);
			}
		}

		private StarfinderClassData selectedClassData;
		public StarfinderClassData SelectedClassData
		{
			get
			{
				return selectedClassData;
			}
			set
			{
				OnPropertyChanged(ref selectedClassData, value);
			}
		}

		private StarfinderThemeData selectedTheme;
		public StarfinderThemeData SelectedTheme
		{
			get
			{
				return selectedTheme;
			}
			set
			{
				OnPropertyChanged(ref selectedTheme, value);
			}
		}

		public string[] RaceNamesToDisplay { get; private set; }
		public string[] ClassNamesToDisplay { get; private set; }
		public string[] ThemeNamesToDisplay { get; private set; }

		private StarfinderRaceData[] races;
		private StarfinderClassData[] classes;
		private StarfinderThemeData[] themes;

		public StarfinderCharacterCreatorViewModel()
		{
			races = ReadWriteJsonCollection<StarfinderRaceData>.ReadCollection(StarfinderResources.RaceDataJson).ToArray();
			classes = ReadWriteJsonCollection<StarfinderClassData>.ReadCollection(StarfinderResources.CharacterClassDataJson).ToArray();
			themes = ReadWriteJsonCollection<StarfinderThemeData>.ReadCollection(StarfinderResources.ThemeDataJson).ToArray();

			RaceNamesToDisplay = Utilitys.GetPropertyValue<string>(races, nameof(StarfinderRaceData.Name));
			ClassNamesToDisplay = Utilitys.GetPropertyValue<string>(classes, nameof(StarfinderClassData.Name));
			ThemeNamesToDisplay = Utilitys.GetPropertyValue<string>(themes, nameof(StarfinderThemeData.Name));
		}
	}
}
