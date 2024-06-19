﻿using PCCharacterManager.Utility;
using PCCharacterManager.ViewModels.CharacterCreatorViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
    public class CharacterTypeHelper : ObservableObject
    {
		private bool _isStarfinder;
		public bool IsStarfinder
		{
			get
			{
				return _isStarfinder;
			}
			private set
			{
				OnPropertyChanged(ref _isStarfinder, value);
			}
		}

		private bool _isDnD5e = true;
		public bool IsDnD5e
		{
			get
			{
				return _isDnD5e;
			}
			private set
			{
				OnPropertyChanged(ref _isDnD5e, value);
			}
		}

		private bool _isDarkSouls;
		public bool IsDarkSouls
		{
			get
			{
				return _isDarkSouls;
			}
			set
			{
				OnPropertyChanged(ref _isDarkSouls, value);
			}
		}

		public void SetCharacterTypeFlags(CharacterType characterType)
		{
			switch (characterType)
			{
				case CharacterType.starfinder:
					IsDnD5e = false;
					IsStarfinder = true;
					IsDarkSouls = false;
					break;
				case CharacterType.DnD5e:
					IsDnD5e = true;
					IsStarfinder = false;
					IsDarkSouls = false;
					break;
				case CharacterType.dark_souls:
					IsDnD5e = false;
					IsStarfinder = false;
					IsDarkSouls = true;
					break;
			}
		}

		public static string BuildPath(DnD5eCharacter character)
		{
			string path = "/" + character.Name + character.Id + ".json";

			switch (character.CharacterType) 
			{
				case CharacterType.DnD5e:
					return DnD5eResources.CharacterDataDir + path;
				case CharacterType.dark_souls:
					return DarkSoulsResources.CharacterDataDir + path;
				case CharacterType.starfinder:
					return StarfinderResources.CharacterDataDir + path;
				default:
					throw new Exception("Character Pathing Issue.");
			}
		}

	}
}
