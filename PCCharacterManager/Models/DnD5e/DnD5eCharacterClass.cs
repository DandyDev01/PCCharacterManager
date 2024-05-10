using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class DnD5eCharacterClass : ObservableObject
	{
		private string _name;
		private HitDie _hitDie;
		private DnD5eCharacterClassLevel _level;

		[JsonProperty(nameof(HitDie))]
		[JsonConverter(typeof(StringEnumConverter))]
		public HitDie HitDie
		{
			get { return _hitDie; }
			set
			{
				OnPropertyChanged(ref _hitDie, value);
			}
		}
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
			}
		}
		public DnD5eCharacterClassLevel Level
		{
			get { return _level; }
			set
			{
				_level = value;
			}
		}

		public ObservableCollection<DnD5eCharacterClassFeature> Features
		{
			get;
			set;
		}

		public DnD5eCharacterClass() 
		{
			_name = string.Empty;
			_level = new DnD5eCharacterClassLevel();
			Features = new ObservableCollection<DnD5eCharacterClassFeature>();
		}

		public DnD5eCharacterClass(DnD5eCharacterClassData data)
		{
			_name = data.Name;
			_hitDie = data.HitDie;
			_level = data.Level;
			_level.Level = 1;

			Features = new ObservableCollection<DnD5eCharacterClassFeature>();
		}

		/// <summary>
		/// Update the character class name to show all the classes the character has a level in.
		/// Will also show the level of each class.
		/// </summary>
		/// <param name="nameOfClassToUpdate">Name of the class being updated.</param>
		/// <param name="level">Level of the class being updated.</param>
		public void UpdateCharacterClassName(string nameOfClassToUpdate, int level)
		{
			string[] classNames = Name.Split("/");
			classNames = Name.Split("/");
			for (int i = 0; i < classNames.Length; i++)
			{
				if (classNames[i].Contains(nameOfClassToUpdate))
				{
					int length = nameOfClassToUpdate.IndexOf(" ") == -1 ? nameOfClassToUpdate.Length : nameOfClassToUpdate.IndexOf(" ");
					classNames[i] = nameOfClassToUpdate.Substring(0, length).Trim() + " " + (level+1);
				}

				classNames[i] = classNames[i].Trim();
			}

			Name = string.Empty;
			for (int i = 0; i < classNames.Length; i++)
			{
				Name += classNames[i];
				if (i < classNames.Length - 1)
					Name += " / ";
			}
		}

		/// <summary>
		/// Get the names and levels of each of the classes.
		/// </summary>
		/// <returns>Key value pair array with the name:key and level:value of each class.</returns>
		/// <exception cref="ArithmeticException">Cannot get the level from one of the classes.</exception>
		public KeyValuePair<string, int>[] GetClassNamesAndLevels()
		{
			var characterClassNames = Name.Split('/');
			KeyValuePair<string, int>[] results = new KeyValuePair<string, int>[characterClassNames.Length];

			for (int i = 0; i < characterClassNames.Length; i++)
			{
				string name = characterClassNames[i].Trim();
				string levelStr = name.Substring(name.IndexOf(" "));
				int level = 0;

				if (int.TryParse(levelStr.Trim(), out level) == false)
					throw new ArithmeticException("Could not get level.");

				name = name.Substring(0, name.IndexOf(" "));
				results[i] = new KeyValuePair<string, int>(name, level);
			}

			return results;
		}
	}
}
