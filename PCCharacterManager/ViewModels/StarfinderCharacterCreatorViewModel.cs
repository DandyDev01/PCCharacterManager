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

		public StarfinderRaceData[] RaceNamesToDisplay { get; }
		public StarfinderClassData[] ClassNamesToDisplay { get; }
		public StarfinderThemeData[] ThemeNamesToDisplay { get; }

		public StarfinderCharacterCreatorViewModel()
		{
			RaceNamesToDisplay = ReadWriteJsonCollection<StarfinderRaceData>.ReadCollection(StarfinderResources.RaceDataJson).ToArray();
			ClassNamesToDisplay = ReadWriteJsonCollection<StarfinderClassData>.ReadCollection(StarfinderResources.CharacterClassDataJson).ToArray();
			ThemeNamesToDisplay = ReadWriteJsonCollection<StarfinderThemeData>.ReadCollection(StarfinderResources.ThemeDataJson).ToArray();

			name = string.Empty;
			selectedRaceData = RaceNamesToDisplay[0];
			selectedClassData = ClassNamesToDisplay[0];
			selectedThemeData = ThemeNamesToDisplay[0];
		}
	}
}
