using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public class JsonCharacterDataService : ICharacterDataService
	{
		private readonly JsonDnD5eCharacterDataService _dnD5echaracterDataService;
		private readonly JsonStarFinderCharacterDataService _starFinderCharacterDataService;

		public JsonCharacterDataService(CharacterStore characterStore)
		{
			_dnD5echaracterDataService = new JsonDnD5eCharacterDataService(characterStore);
			_starFinderCharacterDataService = new JsonStarFinderCharacterDataService();
		}

		public void Add(DnD5eCharacter newCharacter)
		{
			if(newCharacter is StarfinderCharacter starfinderCharacter)
			{
				_starFinderCharacterDataService.Add(starfinderCharacter);
			}
			else if(newCharacter is DnD5eCharacter)
			{
				_dnD5echaracterDataService.Add(newCharacter);
			}
		}

		public bool Delete(DnD5eCharacter character)
		{
			if (character is StarfinderCharacter starfinderCharacter)
			{
				_starFinderCharacterDataService.Delete(starfinderCharacter);
				return true;
			}
			else if (character is DnD5eCharacter)
			{
				_dnD5echaracterDataService.Delete(character);
				return true;
			}

			return false;
		}

		public IEnumerable<string> GetCharacterFilePaths()
		{
			List<string> paths = new List<string>();
			paths.AddRange(Directory.GetFiles(DnD5eResources.CharacterDataDir));
			paths.AddRange(Directory.GetFiles(StarfinderResources.CharacterDataDir));

			return paths;
		}

		public IEnumerable<DnD5eCharacter> GetCharacters()
		{
			List<StarfinderCharacter> starfinderCharacters = new List<StarfinderCharacter>();
			string[] starfinderCharacterEntries = Directory.GetFiles(StarfinderResources.CharacterDataDir);
			foreach (string characterEntry in starfinderCharacterEntries)
			{
				var item = ReadWriteJsonFile<StarfinderCharacter>.ReadFile(characterEntry);
				if (item != null) starfinderCharacters.Add(item);
			}

			List<DnD5eCharacter> characters = new List<DnD5eCharacter>();
			string[] characterEntries = Directory.GetFiles(DnD5eResources.CharacterDataDir);
			foreach (string characterEntry in characterEntries)
			{
				var character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterEntry);
				if (character != null) characters.Add(character);
			}

			characters.AddRange(starfinderCharacters);

			return characters;
		}

		public void Save(IEnumerable<DnD5eCharacter> characters)
		{
			foreach (var item in characters)
			{
				if (item is StarfinderCharacter starfinderCharacter)
				{
					_starFinderCharacterDataService.Save(starfinderCharacter);
				}
				else if (item is DnD5eCharacter)
				{
					_dnD5echaracterDataService.Save(item);
				}
			}
		}

		public void Save(DnD5eCharacter character)
		{
			if (character is StarfinderCharacter starfinderCharacter)
			{
				_starFinderCharacterDataService.Save(starfinderCharacter);
			}
			else if (character is DnD5eCharacter)
			{
				_dnD5echaracterDataService.Save(character);
			}
		}
	}
}
