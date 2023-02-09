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

		private StarfinderThemeData selectedThemeData;
		public StarfinderThemeData SelectedThemeData
		{
			get
			{
				return selectedThemeData;
			}
			set
			{
				OnPropertyChanged(ref selectedThemeData, value);
			}
		}

		public string[] RaceNamesToDisplay { get; private set; }
		public string[] ClassNamesToDisplay { get; private set; }
		public string[] ThemeNamesToDisplay { get; private set; }

		private readonly StarfinderRaceData[] races;
		private readonly StarfinderClassData[] classes;
		private readonly StarfinderThemeData[] themes;

		public StarfinderCharacterCreatorViewModel()
		{
			races = ReadWriteJsonCollection<StarfinderRaceData>.ReadCollection(StarfinderResources.RaceDataJson).ToArray();
			classes = ReadWriteJsonCollection<StarfinderClassData>.ReadCollection(StarfinderResources.CharacterClassDataJson).ToArray();
			themes = ReadWriteJsonCollection<StarfinderThemeData>.ReadCollection(StarfinderResources.ThemeDataJson).ToArray();

			// NOTE: will probably remove properties and bind to property in combo box.
			RaceNamesToDisplay = Utilitys.GetPropertyValue<string>(races, nameof(StarfinderRaceData.Name));
			ClassNamesToDisplay = Utilitys.GetPropertyValue<string>(classes, nameof(StarfinderClassData.Name));
			ThemeNamesToDisplay = Utilitys.GetPropertyValue<string>(themes, nameof(StarfinderThemeData.Name));

			name = string.Empty;
			selectedRaceData = races[0];
			selectedClassData = classes[0];
			selectedThemeData = themes[0];
		}
	}
}
